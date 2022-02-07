using CleanArchitecture.Shared.Core.Models;

namespace CleanArchitecture.Shared.Core.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(EmailSendModel email);
    }
}
