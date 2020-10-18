using System.Threading.Tasks;

namespace CleanArchitecture.Core.Interfaces.Infrastructure.Email
{
    public interface IEmailService
    {
        Task SendEmailAsync(EmailSendModel email);
    }
}
