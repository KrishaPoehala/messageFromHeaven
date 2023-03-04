using Microsoft.Extensions.Options;
using reenbit.Function.Options;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace reenbit.Function.Services;

public class EmailSender : IEmailSender
{
    private readonly EmailSendOptions _options;
    public EmailSender(IOptions<EmailSendOptions> options)
    {
        _options = options.Value;
    }

    public async Task SendAsync(string from, string fileName)
    {
        var client = new SmtpClient(_options.HostName, _options.Port)
        {
            EnableSsl = true,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(_options.FromEmail, _options.FromPassword),
        };
        var message = new MailMessage()
        {
            From = new MailAddress(_options.FromEmail),
            Subject = "Message from heaven!",
            Body = $"{fileName} has been uploaded to blob!",
        };

        message.To.Add(new MailAddress(from));
        await client.SendMailAsync(message);
    }
}
