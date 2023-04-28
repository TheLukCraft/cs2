using Domain.Enums;

namespace Application.Interfaces
{
    public interface IEmailSenderService
    {
        Task<bool> SendAsync(string to, string subject, EmailTemplate emailTemplate, object model);
    }
}