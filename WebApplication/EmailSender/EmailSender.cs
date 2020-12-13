using System.Threading.Tasks;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace WebApplication.EmailSender
{
    public class SmtpOptions
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }

        public SmtpOptions(string username, string password, string host, int port)
        {
            Username = username;
            Password = password;
            Host = host;
            Port = port;
        }

        public static SmtpOptions FromConfiguration(IConfiguration configuration)
        {
            var userName = configuration["Settings:SmtpUsername"];
            var password = configuration["Settings:SmtpPassword"];
            var host = configuration["Settings:SmtpHost"];
            var port = 25;
            return new SmtpOptions(userName, password, host, port);
        }
    }
    
    public class EmailSender : IMailSender
    {
        
        public IConfiguration Configuration { get; set; }
        public SmtpOptions _smtpOptions { get; set; }

        public EmailSender(IConfiguration configuration)
        {
            Configuration = configuration;
            _smtpOptions = SmtpOptions.FromConfiguration(configuration);
        }
        
        public async Task SendEmailAsync(string recipientEmail, string subject, string message)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("Janush F.M. Painting Site", _smtpOptions.Username));
            emailMessage.To.Add(new MailboxAddress("", recipientEmail));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };

            using var client = new SmtpClient();
            await client.ConnectAsync(_smtpOptions.Host, _smtpOptions.Port, false);
            await client.AuthenticateAsync(_smtpOptions.Username, _smtpOptions.Password);
            await client.SendAsync(emailMessage);
            await client.DisconnectAsync(true);
        }
    }
}