using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using BuildingBlocks.Application.Data;
using BuildingBlocks.Application.Email;
using Dapper;

namespace BuildingBlocks.Infrastructure.Email;

/// <summary>
/// Đây chỉ là sample đơn giản để minh họa cách gửi email bằng cách lưu trữ vao cơ sở dữ liệu.
/// </summary>
/// <param name="configuration"></param>
/// <param name="sqlConnectionFactory"></param>
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
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    private MailMessage CreateMailMessage(EmailMsg msg)
    {
        return new MailMessage
        {
            Body    = msg.Content,
            From    = new MailAddress(configuration.FromEmail, configuration.Password),
            Subject = msg.Subject,
        };
    }

    private SmtpClient CreateSmtpClient()
    {
        return new SmtpClient(configuration.SmtpHost, configuration.SmtpPort)
        {
            Credentials    = new NetworkCredential(configuration.FromEmail, configuration.Password),
            DeliveryMethod = SmtpDeliveryMethod.Network,
            EnableSsl      = true,
        };
    }
}