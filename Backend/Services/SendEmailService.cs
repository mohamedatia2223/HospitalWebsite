using System.Net.Mail;
using System.Net;

namespace Hospital.Services
{
    public class SendEmailService
    {
        public async static Task SendEmailAsync(Contact dto)
        {
            var mailMessage = new MailMessage
            {
                From = new MailAddress("Your Email"),
                Subject = dto.Subject,
                Body = $"From: {dto.Name}\n\nMessage:\n{dto.Message}",
                IsBodyHtml = false
            };

            mailMessage.To.Add(dto.Email);

            if (dto.Image != null && dto.Image.Length > 0)
            {
                var ms = new MemoryStream();
                await dto.Image.CopyToAsync(ms);
                ms.Position = 0;
                mailMessage.Attachments.Add(new Attachment(ms, dto.Image.FileName));
            }

            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("Your Email", "Your password"),
                EnableSsl = true,
            };

            await smtpClient.SendMailAsync(mailMessage);

        }
    }
}
