using System.Net;
using System.Net.Mail;
using BuildingBlocks.Application.Email;
using Dapper;

namespace BuildingBlocks.Infrastructure.Email;

/// <summary>
/// Đây chỉ là sample đơn giản để minh họa cách gửi email bằng cách lưu trữ vao cơ sở dữ liệu.
/// </summary>
/// <param name="configuration"></param>
public class EmailSender(
    EmailConfiguration configuration
    // ISqlConnectionFactory sqlConnectionFactory
) : IEmailSender
{
    public async Task SendMessage(EmailMsg msg)
    {
        try
        {
            using var smtpClient  = CreateSmtpClient();
            using var mailMessage = CreateMailMessage(msg);
            await smtpClient.SendMailAsync(mailMessage);
        }
        catch (SmtpException smtpEx)
        {
            throw new Exception($"SMTP Error: {smtpEx.StatusCode} - {smtpEx.Message}", smtpEx);
        }
        catch (Exception ex)
        {
            throw new Exception($"Email sending failed: {ex.Message}", ex);
        }
    }

    private MailMessage CreateMailMessage(EmailMsg msg)
    {
        if (string.IsNullOrWhiteSpace(msg.To)) throw new InvalidOperationException("Email recipient is empty");
        var mailMessage = new MailMessage
        {
            Body    = msg.Content,
            From    = new MailAddress(configuration.FromEmail, "Email"),
            Subject = msg.Subject,
        };

        mailMessage.To.Add(msg.To);
        return mailMessage;
    }

    private SmtpClient CreateSmtpClient()
    {
        return new SmtpClient(configuration.SmtpHost, configuration.SmtpPort)
        {
            UseDefaultCredentials = false,
            Credentials           = new NetworkCredential(configuration.FromEmail, configuration.Password),
            DeliveryMethod        = SmtpDeliveryMethod.Network,
            EnableSsl             = true,
        };
    }
}