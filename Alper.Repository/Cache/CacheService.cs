using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Polly.CircuitBreaker;
using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alper.Common.DTO;
using Microsoft.Extensions.Configuration;
using Polly.Retry;
using StackExchange.Redis;
using System.Diagnostics;
using System.Text.Json;
using Common.Utils;

namespace Alper.Repository.Cache
{
    public sealed class CacheService : ICacheService
    {
        private readonly IDistributedCache _cache;
        private readonly ILogger<CacheService> _logger;
        private readonly DistributedCacheEntryOptions _redisOptions;
        private readonly ResiliencePipeline _pollyPipeline;
        private readonly CircuitBreakerStateProvider _stateProvider;

        public CacheService(IDistributedCache cache, ILogger<CacheService> logger, IConfiguration configuration)
        {
            _cache = cache;
            _logger = logger;

            if (!int.TryParse(configuration["Polly:CacheRetryCount"], out var retryCount))
            {
                retryCount = 3;
            }

            if (!int.TryParse(configuration["Polly:CacheWaitSecondsForEachRetry"], out var waitSecondsForEachRetry))
            {
                waitSecondsForEachRetry = 2;
            }


            if (!int.TryParse(configuration["Polly:CacheCircuitBreakerWaitMinutesBeforeRetry"],
                    out var circuitBreakerWaitMinutesBeforeRetry))
            {
                circuitBreakerWaitMinutesBeforeRetry = 1;
            }

            _stateProvider = new CircuitBreakerStateProvider();

            var cbOptions = new CircuitBreakerStrategyOptions
            {
                FailureRatio = 0.1,
                SamplingDuration = TimeSpan.FromSeconds(30),
                MinimumThroughput = 3,
                BreakDuration = TimeSpan.FromMinutes(circuitBreakerWaitMinutesBeforeRetry),
                ShouldHandle = new PredicateBuilder()
                    .Handle<RedisConnectionException>(),
                StateProvider = _stateProvider,
                OnClosed = _ =>
                {
                    _logger.LogInformation("Cache circuit closed");
                    return ValueTask.CompletedTask;
                },
                OnOpened = args =>
                {
                    _logger.LogWarning("Cache circuit opened. Break duration: {Duration} seconds",
                        args.BreakDuration.Seconds);
                    return ValueTask.CompletedTask;
                },
                OnHalfOpened = _ =>
                {
                    _logger.LogInformation("Cache circuit half open");
                    return ValueTask.CompletedTask;
                }
            };

            _pollyPipeline = new ResiliencePipelineBuilder()
                .AddRetry(new RetryStrategyOptions
                {
                    ShouldHandle = new PredicateBuilder()
                            .Handle<RedisConnectionException>(),
                    MaxRetryAttempts = retryCount,
                    BackoffType = DelayBackoffType.Constant,
                    DelayGenerator = args =>
                    {
                        var delay = (args.AttemptNumber + 1) * waitSecondsForEachRetry;
                        return new ValueTask<TimeSpan?>(TimeSpan.FromSeconds(delay));
                    },
                    OnRetry = args =>
                    {
                        _logger.LogInformation(
                        "Attempt Number: {AttemptNumber} Retry Delay: {DelaySeconds} seconds Duration: {Duration} seconds",
                        args.AttemptNumber + 1, args.RetryDelay.Seconds, args.Duration.Seconds);
                        return ValueTask.CompletedTask;
                    }
                }
                )
                .AddCircuitBreaker(cbOptions)
                .Build();

            if (!int.TryParse(configuration["Redis:CacheAbsoluteExpiration"], out var absoluteExpiration))
            {
                absoluteExpiration = 3;
            }

            if (!int.TryParse(configuration["Redis:CacheSlidingExpiration"], out var slidingExpiration))
            {
                slidingExpiration = 1;
            }

            _redisOptions = new DistributedCacheEntryOptions()
                .SetAbsoluteExpiration(
                    DateTime.Now.AddHours(
                        absoluteExpiration)) //Şu andan itibaren kaç saat sonra kesinlikle expire olacak
                .SetSlidingExpiration(
                    TimeSpan.FromHours(
                        slidingExpiration)); // belirtilen sür içerisinde cacheden veri alınmazsa expire olacak
        }

        public async Task Remove(string key, CancellationToken cancellationToken = default)
        {
            try
            {
                await Task.Run(() =>
                        _pollyPipeline.ExecuteAsync(async _ =>
                            await _cache.RemoveAsync(key, cancellationToken), cancellationToken),
                    cancellationToken);
            }
            catch (Exception e)
            {
                _logger.LogWarning("Cache Remove: {State}:  {Message}", _stateProvider.CircuitState,
                    e.Message);
            }
        }

        public async Task<T?> Get<T>(string key, CancellationToken cancellationToken = default)
        {
            byte[]? sonuc;

            try
            {
                sonuc = await _pollyPipeline.ExecuteAsync(async _ =>
                    await _cache.GetAsync(key, cancellationToken), cancellationToken);
            }
            catch (Exception e)
            {
                sonuc = default;
                _logger.LogWarning("Cache Get: {State}:  {Message}", _stateProvider.CircuitState,
                    e.Message);
            }

            return sonuc == null
                ? default
                : JsonSerializer.Deserialize<T>(
                    Encoding.UTF8.GetString(sonuc));
            //vmo: JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(sonuc));
        }

        public async Task Set<T>(string key, T value, CancellationToken cancellationToken = default)
        {
            var bytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(value));
            //vmo: Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(value));

            try
            {
                await Task.Run(() =>
                        _pollyPipeline.ExecuteAsync(async _ =>
                            await _cache.SetAsync(key, bytes, _redisOptions, cancellationToken), cancellationToken),
                    cancellationToken);
            }
            catch (Exception e)
            {
                _logger.LogWarning("Cache Set: {State}:  {Message}", _stateProvider.CircuitState,
                    e.Message);
            }
        }

        public AlperResult<MessageDto> RemoveAll()
        {
            try
            {
                var startInfo = new ProcessStartInfo
                {
                    //FileName = "/opt/homebrew/bin/redis-cli",
                    //Arguments = "-h 192.168.30.19 -p 6379 flushdb"
                    FileName = "/usr/bin/redis-cli",
                    Arguments = "flushdb"
                };

                var process = new Process { StartInfo = startInfo };

                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;

                var data = string.Empty;

                process.OutputDataReceived += (_, args) =>
                {
                    data = args.Data;
                    Log(data);
                };

                process.Start();
                process.BeginOutputReadLine();

                return data == "OK"
                    ? new MessageDto("Cache temizlendi")
                    : "Cache temizlenemedi";
            }
            catch (Exception e)
            {
                _logger.LogError("Cache temizlenemedi. {Message}", e.Message);
                return AlperResult<MessageDto>.Exception(e.Message);
            }
        }

        private void Log(string? data)
        {
            if (!string.IsNullOrWhiteSpace(data) && data != "null")
            {
                if (data == "OK")
                {
                    _logger.LogInformation("Redis cache temizlendi");
                }
                else
                {
                    _logger.LogError("Redis cache temizlenemedi {Data}", data);
                }
            }
        }
    }
}
