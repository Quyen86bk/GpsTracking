using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Mail;
using NetCore.Library;

namespace NetCore.GpsTrackingModule.Library
{
    public class EmailHelper
    {
        public static void Send(List<string> toEmails, string subject, string content, string filePath = null)
        {
            try
            {
                MailMessage message = new MailMessage();
                SmtpClient smtp = new SmtpClient();

                message.From = new MailAddress("demo.hti.usb@gmail.com");

                foreach (var to in toEmails)
                    message.To.Add(new MailAddress(to));

                message.Subject = subject;
                message.IsBodyHtml = true;
                message.Body = content;

                smtp.Port = 587;
                smtp.Host = "smtp.gmail.com";
                smtp.EnableSsl = true;
                smtp.Credentials = new NetworkCredential("demo.hti.usb@gmail.com", "wppiydhalszkrpzz");

                if (filePath != null && new xfile(filePath).Exists)
                {
                    message.Attachments.Add(new Attachment(filePath));
                }

                smtp.Send(message);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            };
        }
    }
}
