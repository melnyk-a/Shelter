using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using Shelter.Application.Abstractions.Email;

namespace Shelter.Infrastructure.Email;

internal sealed class EmailService : IEmailService
{
    public EmailSettings _emailSettings { get; }

    public EmailService(IOptions<EmailSettings> mailSettings)
    {
        _emailSettings = mailSettings.Value;
    }

    public async Task<bool> SendAsync(Domain.Users.Email recipient, string subject, string body)
    {
        var client = new SendGridClient(_emailSettings.ApiKey);

        var to = new EmailAddress(recipient.Value);
        var from = new EmailAddress
        {
            Email = _emailSettings.FromAddress,
            Name = _emailSettings.FromName
        };

        var sendGridMessage = MailHelper.CreateSingleEmail(
            from,
            to,
            subject,
            body,
            body);

        var response = await client.SendEmailAsync(sendGridMessage);

        return response.StatusCode == System.Net.HttpStatusCode.Accepted ||
            response.StatusCode == System.Net.HttpStatusCode.OK;
    }
}
