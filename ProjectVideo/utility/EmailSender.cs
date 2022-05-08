using Microsoft.AspNetCore.Identity.UI.Services;
using System.Threading.Tasks;

namespace ProjectVideo.utility
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            return Execute(email, subject, htmlMessage);
        }

        public async Task Execute(string email, string subject, string htmlMessage)
        {
            MailjetClient client = new MailjetClient(Environment.GetEnvironmentVariable("****************************1234"), Environment.GetEnvironmentVariable("****************************abcd"))
            {
                Version = ApiVersion.V3_1,
            };
            MailjetRequest request = new MailjetRequest
            {
                Resource = Send.Resource,
            }
             .Property(Send.Messages, new JArray {
     new JObject {
      {
       "From",
       new JObject {
        {"Email", "sinousjimmy91@gmail.com"},
        {"Name", "SINOUS"}
       }
      }, {
       "To",
       new JArray {
        new JObject {
         {
          "Email",
          "sinousjimmy91@gmail.com"
         }, {
          "Name",
          "SINOUS"
         }
        }
       }
      }, {
       "Subject",
       "Greetings from Mailjet."
      }, {
       "TextPart",
       "My first Mailjet email"
      }, {
       "HTMLPart",
       "<h3>Dear passenger 1, welcome to <a href='https://www.mailjet.com/'>Mailjet</a>!</h3><br />May the delivery force be with you!"
      }, {
       "CustomID",
       "AppGettingStartedTest"
      }
     }
             });
            MailjetResponse response = await client.PostAsync(request);

        }
    }
}
