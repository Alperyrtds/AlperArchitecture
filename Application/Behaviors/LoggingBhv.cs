using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Alper.Application.Behaviors;

public sealed class LoggingBehavior<TRequest, TResponse>
    (ILogger<LoggingBehavior<TRequest, TResponse>> logger) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var requestName = request.GetType().Name;

        //pre logic

        logger.LogInformation("{Request} is starting", requestName);

        logger.LogInformation("Request: \n{Request}", request.ToString());

        var timer = Stopwatch.StartNew();

        //main event

        var response = await next();

        //post logic

        timer.Stop();

        logger.LogInformation("Response: \n{Response}", response!.ToString());

        logger.LogInformation("{Request} has finished in {Time} ms", requestName, timer.ElapsedMilliseconds);

        return response;
    }
}

