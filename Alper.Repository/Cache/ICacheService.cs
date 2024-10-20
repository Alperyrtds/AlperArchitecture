using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alper.Common.DTO;
using Common.Utils;

namespace Alper.Repository.Cache;

public interface ICacheService
{
    Task Remove(string key, CancellationToken cancellationToken = default);
    Task<T?> Get<T>(string key, CancellationToken cancellationToken = default);
    Task Set<T>(string key, T value, CancellationToken cancellationToken = default);
    AlperResult<MessageDto> RemoveAll();
}