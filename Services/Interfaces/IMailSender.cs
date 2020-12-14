using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IMailSender
    {
        Task SendEmailAsync(string recipientName, string subject, string message);
    }
}