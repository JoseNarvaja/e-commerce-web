using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceWeb.Utility
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;
        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            string cuenta = _configuration.GetValue<string>("ElasticEmail:Cuenta");
            string key = _configuration.GetValue<string>("ElasticEmail:Key");

            var correoAEnviar = new MimeMessage();
            correoAEnviar.From.Add(MailboxAddress.Parse(cuenta));
            correoAEnviar.To.Add(MailboxAddress.Parse(email));
            correoAEnviar.Subject = subject;
            correoAEnviar.Body = new TextPart(MimeKit.Text.TextFormat.Html){Text= htmlMessage };

            using (var clienteSmtp = new SmtpClient())
            {
                clienteSmtp.Connect("smtp.elasticemail.com", 2525, MailKit.Security.SecureSocketOptions.StartTls);
                clienteSmtp.Authenticate(cuenta, key);
                clienteSmtp.Send(correoAEnviar);
                clienteSmtp.Disconnect(true);
            }

            return Task.CompletedTask;
        }
    }
}
