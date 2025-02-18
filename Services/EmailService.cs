using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;

namespace Juan_Mvc_Project.Services
{
	public class EmailService
	{
		public void SendEmail(string to, string subject, string body)
		{
			var email = new MimeMessage();
			email.From.Add(MailboxAddress.Parse("markus.hoeger13@ethereal.email"));
			email.To.Add(MailboxAddress.Parse(to));
			email.Subject = subject;
			email.Body = new TextPart(TextFormat.Html) { Text = body };

			using var smtp = new SmtpClient();
			smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
			smtp.Authenticate("imnigarvalikhanova@gmail.com", "hfhlknkqhfkitxki");
			smtp.Send(email);
			smtp.Disconnect(true);

		}
	}
}
