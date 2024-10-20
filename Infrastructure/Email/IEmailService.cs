using Common.Utils;

namespace Alper.Infrastructure.Email;

public interface IEmailService
{
    AlperResult<bool> Send(string to, string subject, string body, CancellationToken cancellationToken = default);
}
