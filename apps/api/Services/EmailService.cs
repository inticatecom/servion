using API.Utilities;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace API.Services;

/// <summary>
/// A service for sending out emails to clients.
/// </summary>
public sealed class EmailService
{
    /// <summary>
    /// The from address to use.
    /// </summary>
    private readonly string _smtpFromAddress =
        EnvironmentVariableUtility.GetEnvironmentVariable<string>(EnvironmentVariable.SmtpFromAddress);

    /// <summary>
    /// Sends an email with the provided information to the client.
    /// </summary>
    ///
    /// <param name="recipient">The recipient.</param>
    /// <param name="subject">The subject.</param>
    /// <param name="content">The content.</param>
    public async Task SendAsync(string recipient, string subject, string content)
    {
        var message = new MimeMessage();

        message.From.Add(new MailboxAddress(
            EnvironmentVariableUtility.GetEnvironmentVariable<string>(EnvironmentVariable.SmtpFromName),
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
            EnvironmentVariableUtility.GetEnvironmentVariable<string>(EnvironmentVariable.SmtpHost),
            EnvironmentVariableUtility.GetEnvironmentVariable<int>(EnvironmentVariable.SmtpPort),
            SecureSocketOptions.StartTls
        );

        await client.AuthenticateAsync(
            _smtpFromAddress,
            EnvironmentVariableUtility.GetEnvironmentVariable<string>(EnvironmentVariable.SmtpPassword)
        );

        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }
}