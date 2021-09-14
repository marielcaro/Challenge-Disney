using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Disney.Controllers
{

    [ApiController]
    [Route(template:"aí/[controller]")]
    public class SendGridTestController:ControllerBase
    {
        public SendGridTestController()
        {


        }

        [HttpGet]
        public async Task<IActionResult> PruebaEmail()
        {
            //Código traído de la página de SendGrid
            var apiKey = "Poner api key aquí"; //API KEY  - No me dejan colocarla porque sino me suspenden la cuenta
            var client = new SendGridClient(apiKey); //crea una instancia del send grid
            var from = new EmailAddress("marielcaro8@gmail.com", "Example User"); //origen- emisor
            var subject = "Sending with Twilio SendGrid is Fun";
            var to = new EmailAddress("marielcaro8@gmail.com", "Example User"); //destino- receptor
            var plainTextContent = "and easy to do anywhere, even with C#"; //contenido
            var htmlContent = "<strong>and easy to do anywhere, even with C#</strong>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);

            return Ok();
        }
    }
}
