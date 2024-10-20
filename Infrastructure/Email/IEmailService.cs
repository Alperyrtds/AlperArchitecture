using Common.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alper.Infrastructure.Email;

public interface IEmailService
{
    AlperResult<bool> Send(string to, string subject, string body, CancellationToken cancellationToken = default);
}
