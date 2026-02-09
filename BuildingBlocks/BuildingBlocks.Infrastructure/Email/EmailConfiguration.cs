namespace BuildingBlocks.Infrastructure.Email;

public class EmailConfiguration(
    string fromEmail,
    string password,
    string smtpHost,
    int smtpPort)
{
    public string FromEmail { get; } = fromEmail;
    public string Password { get; } = password;
    public string SmtpHost { get; } = smtpHost;
    public int SmtpPort { get; } = smtpPort;

    public EmailConfiguration() : this(string.Empty, string.Empty, string.Empty, 0)
    {
    }
}