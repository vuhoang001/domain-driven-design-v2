namespace BuildingBlocks.Application.Email;

public class EmailConfiguration(string fromEmail)
{
    public string FromEmail { get; } = fromEmail;
}