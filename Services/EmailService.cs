using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace QlChoThueNha1.Services
{
    public class EmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendOtpAsync(string email, string otp)
        {
            var message = new MimeMessage();

            message.From.Add(
                MailboxAddress.Parse(
                    _config["EmailSettings:Email"]
                ));

            message.To.Add(
                MailboxAddress.Parse(email));

            message.Subject = "Quên mật khẩu";

            message.Body = new TextPart("plain")
            {
                Text = $"Mã OTP của bạn là: {otp}"
            };

            using var smtp = new SmtpClient();

            await smtp.ConnectAsync(
                _config["EmailSettings:SmtpServer"],
                int.Parse(_config["EmailSettings:Port"]),
                SecureSocketOptions.StartTls);

            await smtp.AuthenticateAsync(
                _config["EmailSettings:Email"],
                _config["EmailSettings:Password"]);

            await smtp.SendAsync(message);

            await smtp.DisconnectAsync(true);
        }
    }
}