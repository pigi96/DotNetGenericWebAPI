using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;

namespace GenericWebAPI.Utilities;

public abstract class MailSystem
{
    private readonly SmtpClient _smtpClient;

    protected MailSystem(IConfiguration configuration)
    {
        _smtpClient = new SmtpClient(configuration.GetSection("WebGenAPI:Mail-System:Smtp-Client")["Host"], int.Parse(configuration.GetSection("WebGenAPI:Mail-System:Smtp-Client")["Port"]));
        _smtpClient.Credentials = new NetworkCredential(configuration.GetSection("WebGenAPI:Mail-System:Smtp-Client:Credentials")["E-Mail"], configuration.GetSection("WebGenAPI:Mail-System:Smtp-Client:Credentials")["Password"]);
        _smtpClient.EnableSsl = true;
    }
    
    protected async Task<bool> SendEmail(string sender, List<string> recipients, string subject, string body)
    {
        var mailMessage = new MailMessage();
        mailMessage.From = new MailAddress(sender);
        mailMessage.To.Add(string.Join(", ", recipients));
        mailMessage.Subject = subject;
        mailMessage.Body = body;
        mailMessage.IsBodyHtml = true;

        await _smtpClient.SendMailAsync(mailMessage);
        return true;
    }
}