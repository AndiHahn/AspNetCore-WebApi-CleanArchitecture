using System.Threading.Tasks;
using CleanArchitecture.Core.Models;

namespace CleanArchitecture.Core.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(EmailSendModel email);
    }
}
