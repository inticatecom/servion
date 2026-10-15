using API.Utilities;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace API.Services;

public sealed class EmailService
{
    private readonly string _smtpFromAddress =
        Environment.GetEnvironmentVariable("SMTP_FROM_ADDRESS") ??
        throw new Exception("Missing 'SMTP_FROM_ADDRESS' variable.");

    public async Task SendAsync(string recipient, string subject, string content)
    {
        var message = new MimeMessage();

        message.From.Add(new MailboxAddress(
            Environment.GetEnvironmentVariable("SMTP_FROM_NAME") ??
            throw new Exception("Missing 'SMTP_FROM_NAME' variable."),
            _smtpFromAddress
        ));
        message.To.Add(MailboxAddress.Parse(recipient));
        message.Subject = subject;
        message.Body = new TextPart("html")
        {
            Text = content
        };

        using var client = new SmtpClient();

        await client.ConnectAsync(
            EnvironmentVariableUtility.GetEnvironmentVariable<string>(EnvironmentVariableKey.SmtpHost),
            int.Parse(
                Environment.GetEnvironmentVariable("SMTP_PORT") ??
                throw new Exception("Missing 'SMTP_PORT' variable.")
            ),
            SecureSocketOptions.StartTls
        );

        await client.AuthenticateAsync(
            _smtpFromAddress,
            Environment.GetEnvironmentVariable("SMTP_PASSWORD") ??
            throw new Exception("Missing 'SMTP_PASSWORD' variable.")
        );

        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }
}