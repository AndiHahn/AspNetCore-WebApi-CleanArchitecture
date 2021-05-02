using System.Threading.Tasks;
using CleanArchitecture.Common.Models.Email;

namespace CleanArchitecture.Domain.Interfaces.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(EmailSendModel email);
    }
}
