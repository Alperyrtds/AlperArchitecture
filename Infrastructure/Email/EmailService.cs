using Common.Utils;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;
using MimeKit.Text;


namespace Alper.Infrastructure.Email;

public sealed class EmailService(ILogger<EmailService> logger, IConfiguration configuration) : IEmailService
{
    public AlperResult<bool> Send(string to, string subject, string body, CancellationToken cancellationToken = default)
    {
        try
        {
            var message = new MimeMessage();
            var from = configuration["Email:From"] ?? "alper@alper.com.tr";
            var host = configuration["Email:Host"] ?? "alper.mail3.com.tr";
            int.TryParse(configuration["Email:Port"] ?? "587", out var port);
            var username = configuration["Email:UserName"] ?? "alper\\UygulamaTest";
            var password = configuration["Email:Password"] ?? "MdWa%6573%WtT";

            message.From.Add(new MailboxAddress("AlperArchitecture", from));
            message.To.Add(new MailboxAddress("", to));
            message.Subject = subject;
            message.Body = new TextPart(TextFormat.Html)
            {
                Text = body
            };

            using var client = new SmtpClient();
            client.ServerCertificateValidationCallback = Genarate.SslCertificateValidationCallback!;
            client.Connect(host, port, SecureSocketOptions.StartTlsWhenAvailable, cancellationToken);
            client.Authenticate(username, password, cancellationToken);
            client.Send(message);
            client.Disconnect(true, cancellationToken);

            return true;
        }
        catch (Exception e)
        {
            logger.LogError("To: {To} Subject: {Subject} Hata: {Error}", to, subject, e.Message);
            return e.Message;
        }
    }


}
