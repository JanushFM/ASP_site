using System.Threading.Tasks;

namespace WebApplication.EmailSender
{
    public interface IMailSender
    {
        Task SendEmailAsync(string recipientName, string subject, string message);
    }
}