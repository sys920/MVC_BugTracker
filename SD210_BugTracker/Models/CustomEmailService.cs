using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;

namespace SD210_BugTracker.Models
{
    public class CustomEmailService : IIdentityMessageService                                    
    {
               
        public void Sending(MailMessage message)
        {
            string userName = System.Configuration.ConfigurationManager.AppSettings["senderEmail"].ToString();
            string userPassword = System.Configuration.ConfigurationManager.AppSettings["senderPassword"].ToString();
            //SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            SmtpClient client = new SmtpClient("smtp.mailtrap.io", 2525);
            client.EnableSsl = true;
            client.Timeout = 100000;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(userName, userPassword);

            client.Send(message);           
            
        }

        public Task SendAsync(IdentityMessage message)
        {
            throw new NotImplementedException();
        }
    }
}