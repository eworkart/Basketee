using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace Basketee.API.Services
{
    public class EmailServices
    {
        public const string APPSETTING_SMTP_HOST = "SmtpHost";
        public const string APPSETTING_SMTP_PORT = "SmtpPort";
        public const string APPSETTING_USER_NAME = "AccountUsername";
        public const string APPSETTING_PASSWORD = "AccountPassword";
        public const string APPSETTING_ENABLE_SSL = "EnableSsl";

        const string REPLY_TO = "admin@pertaminalpg.com";//TODO 

        public static void SendMailTest(string to, string subject, string content, string ccTo = null, byte[] docBytes = null, string docFileName = null)
        {
            using (MailMessage mm = new MailMessage())
            {
                mm.From = new MailAddress(REPLY_TO);
                mm.Priority = MailPriority.High;
                mm.To.Add(to);
                if (!string.IsNullOrWhiteSpace(ccTo))
                {
                    mm.CC.Add(ccTo);
                }
                mm.Subject = subject;
                mm.Body = content;

                if (docBytes != null)
                {
                    MemoryStream docStream = new MemoryStream(docBytes);
                    mm.Attachments.Add(new Attachment(docStream, docFileName));
                }

                string smtpServer = Common.GetAppSetting<string>(APPSETTING_SMTP_HOST, string.Empty);
                int smtpPort = Common.GetAppSetting<int>(APPSETTING_SMTP_PORT, 0);
                string smtpUsername = Common.GetAppSetting<string>(APPSETTING_USER_NAME, string.Empty);
                string smtpPswd = Common.GetAppSetting<string>(APPSETTING_PASSWORD, string.Empty);

                var client = new SmtpClient(smtpServer, smtpPort)
                {
                    Credentials = new NetworkCredential(smtpUsername, smtpPswd),
                    EnableSsl = Common.GetAppSetting<bool>(APPSETTING_ENABLE_SSL, true)
                };

                mm.IsBodyHtml = true;
                //SmtpClient smtp = new SmtpClient();
                client.Send(mm);
            }
        }

        public static void SendMail(string to, string subject, string content, string ccTo = null, string bccTo = null, byte[] docBytes = null, string docFileName = null)
        {
            string smtpUsername = Common.GetAppSetting<string>(APPSETTING_USER_NAME, string.Empty);
            string smtpPswd = Common.GetAppSetting<string>(APPSETTING_PASSWORD, string.Empty);

            SMTPServices.Service1Client smtpService = new SMTPServices.Service1Client();
            var response = smtpService.SendMessage(smtpUsername, smtpPswd, to, ccTo, bccTo, subject, content);
            smtpService.Close();
        }

        public static void SendMailWithAttachment(string to, string subject, string content,int order_id, byte[] fileBytes, string ccTo = null, string bccTo = null, byte[] docBytes = null, string docFileName = null)
        {
            //byte[] fileBytes = new byte[16 * 1024];
            //using (MemoryStream ms = new MemoryStream())
            //{
            //    int read;
            //    while ((read = file.Read(fileBytes, 0, fileBytes.Length)) > 0)
            //    {
            //        ms.Write(fileBytes, 0, read);
            //    }
            //    ms.ToArray();
            //}

            string smtpUsername = Common.GetAppSetting<string>(APPSETTING_USER_NAME, string.Empty);
            string smtpPswd = Common.GetAppSetting<string>(APPSETTING_PASSWORD, string.Empty);
            List<SMTPServices.CommonAttachmentDoc> smtpServicesAttachments = new List<SMTPServices.CommonAttachmentDoc>();
            SMTPServices.CommonAttachmentDoc smtpServicesAttachment = new SMTPServices.CommonAttachmentDoc();
            smtpServicesAttachment.filename= "e-Receipt" + order_id + ".pdf";
            smtpServicesAttachment.filebyte = fileBytes;
            //byte[] contentBytes = Encoding.ASCII.GetBytes(content);
            //smtpServicesAttachment.filebyte = contentBytes;
            smtpServicesAttachments.Add(smtpServicesAttachment);

            SMTPServices.Service1Client smtpService = new SMTPServices.Service1Client();
            var response = smtpService.SendMessageAtt(smtpUsername, smtpPswd, to, ccTo, bccTo, subject, content, smtpServicesAttachments.ToArray());
            smtpService.Close();
        }
    }
}
