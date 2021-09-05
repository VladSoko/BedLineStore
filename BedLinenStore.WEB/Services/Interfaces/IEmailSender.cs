using System.Threading.Tasks;

namespace BedLinenStore.WEB.Services.Interfaces
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string htmlMessage);
    }
}