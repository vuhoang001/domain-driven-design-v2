namespace BuildingBlocks.Application.Email;

public interface IEmailSender
{
   Task SendMessage(EmailMsg emailMsg);
}