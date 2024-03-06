namespace Shelter.Application.Abstractions.Email;

public interface IEmailService
{
    Task<bool> SendAsync(Domain.Users.Email recipient, string subject, string body);
}
