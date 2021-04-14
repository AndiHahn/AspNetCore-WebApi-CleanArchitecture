using System.Threading.Tasks;
using CleanArchitecture.Core.Models;

namespace CleanArchitecture.Core.Interfaces.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(EmailSendModel email);
    }
}
