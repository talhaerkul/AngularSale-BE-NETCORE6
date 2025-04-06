using AngularSaleAPI.Application.Abstractions.Services.UserServices;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace AngularSaleAPI.Infrastructure.Services.Mail
{
    public class MailService : IMailService
    {
        readonly IConfiguration _configuration;

        public MailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendMailAsync(string to, string subject, string body, bool isBodyHtml = true)
        {
            await SendMailAsync(new[] {to}, subject, body, isBodyHtml);
        }

        public async Task SendMailAsync(string[] tos, string subject, string body, bool isBodyHtml = true)
        {
            MailMessage mail = new();
            mail.IsBodyHtml = isBodyHtml;
            mail.Subject = subject;
            mail.Body = body;
            foreach (var to in tos)
                mail.To.Add(to);
            mail.From = new MailAddress(_configuration["Mail:ErkulSaleMail"], _configuration["Mail:ErkulSaleName"], System.Text.Encoding.UTF8);
        
            SmtpClient smtp = new SmtpClient();
            smtp.Credentials = new NetworkCredential(_configuration["Mail:ErkulSaleMail"], _configuration["Mail:ErkulSalePassword"]);
            smtp.Port = int.Parse(_configuration["Mail:Port"]);
            smtp.EnableSsl = true;
            smtp.Host = _configuration["Mail:Host"];
            await smtp.SendMailAsync(mail);
        }

        public async Task SendPasswordResetMailAsync(string to, string userId, string resetToken)
        {
            var url = $"{_configuration["Mail:Domain"]}/{userId}/{resetToken}";
            StringBuilder mail = new StringBuilder();
            mail.AppendLine($"<h1>Password Reset </h1>\r\n<p>We have received a request to reset your password. Please confirm the reset to choose a new password. Otherwise, you can ignore this email. </p>\r\n<a href=\"{url}\">RESET PASSWORD</a>");
            await SendMailAsync(to, "Angular Sale Account Password Reset", mail.ToString());
        }

        public async Task SendCompletedOrderMailAsync(string to, string userName, string orderCode, DateTime orderDate)
        {
            var m = $"<h1>Your Order {orderCode} has been shipped</h1>\r\n<h2>Dear {userName},</h2>\r\n<p>This is an e-mail notification to inform you that your order {orderCode} has been shipped by the seller on {DateTime.Now}. You are advised to contact the seller for shipment information.</p>\r\n";
            StringBuilder mail = new StringBuilder();
            mail.AppendLine(m);
            await SendMailAsync(to, $"Your Order {orderCode} has been shipped", mail.ToString());
        }
    }
}
