using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace Web_ALPA.Models.General
{
    public class Email
    {
        public bool EnviarCorreo(string CorreoOrigen, string Password, string CorreoDestino, string Asunto, string MensajeBody)
        {
            try
            {                
                MailMessage email = new MailMessage();
                SmtpClient smtp = new SmtpClient();

                //email.To.Add(new MailAddress(CorreoDestino));
              
                email.From = new MailAddress(CorreoOrigen);
                email.Subject = Asunto;
                email.SubjectEncoding = System.Text.Encoding.UTF8;
                email.Body = MensajeBody;
                email.IsBodyHtml = true;
                email.Priority = MailPriority.Normal;
                //FileStream fs = new FileStream("E:\\TestFolder\\test.pdf", FileMode.Open, FileAccess.Read);
                //Attachment a = new Attachment(fs, "test.pdf", MediaTypeNames.Application.Octet);
                //email.Attachments.Add(a);
                smtp.Host = "outlook.office365.com";
                smtp.Port = 587;
                //smtp.Timeout = 50;
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(CorreoOrigen, Password);
                //smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                string[] emailTo = CorreoDestino.Split(',');
                for (int i = 0; i < emailTo.GetLength(0); i++)
                    email.To.Add(emailTo[i]);

                smtp.Send(email);
                email.Dispose();
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}