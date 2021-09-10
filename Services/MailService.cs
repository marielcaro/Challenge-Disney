using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Disney.Entities;
using Disney.Interfaces;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Disney.Services
{
    public class MailService : IMailService
    {
        private readonly ISendGridClient _client;

        public MailService (ISendGridClient client)
        {
            _client = client;
        }

        public async Task SendMail(User user)
        {
            //Traído de la documentación oficial de SendGrid
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("marielcaro8@gmail.com", "Mariel"),
                Subject = "Disney Challenge - Bienvenida",
                PlainTextContent = $"¡Bienvenid@ {user.UserName} al Challenge de Disney!",
            };
            msg.AddTo(new EmailAddress(user.Email, "Test User"));

            await _client.SendEmailAsync(new SendGridMessage());
        }
    }
}
