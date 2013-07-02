using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;

namespace DataDomainSecurity
{
    public class Email
    {
        public static void SendEmail_GMail(string fromGMail, string address, string subject, string message)
        {
            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
            //var loginInfo = new System.Net.NetworkCredential(email, password);

            mail.From = new MailAddress(fromGMail);
            mail.To.Add(address);
            mail.Subject = subject;
            mail.Body = message;

            SmtpServer.EnableSsl = true;
            SmtpServer.UseDefaultCredentials = false;
            //smtpClient.Credentials = loginInfo;
            SmtpServer.Send(mail);
        
        }
                

        public static void SendEmail(string address, string subject, string message)
        {
            string email = "jpaleta.isel@gmail.com";
            string password = "#$isel#$";

            var loginInfo = new System.Net.NetworkCredential(email, password);
            var msg = new MailMessage();
            var smtpClient = new SmtpClient("smtp.gmail.com", 587);

            msg.From = new MailAddress(email);
            msg.To.Add(new MailAddress(address));
            msg.Subject = subject;
            msg.Body = message;
            msg.IsBodyHtml = true;

            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = loginInfo;
            smtpClient.Send(msg);
        }


        // jp -> código do AccountController 
        public void SendActivationMail(DataDomainEntities.User user, bool isActivation)
        {
            //try
            //{
            //    System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();

            //    message.From = new System.Net.Mail.MailAddress("noreply@luxys.com");
            //    message.To.Add(new System.Net.Mail.MailAddress(user.Email));

            //    message.IsBodyHtml = false;
            //    message.BodyEncoding = Encoding.UTF8;
            //    if (isActivation)
            //    {

            //        message.Subject = "Chess Activation.";
            //        message.Body = "Activation for " + user.Nickname + Environment.NewLine +
            //                       "Please activate your account with following link: " /*+ baseurl*/ + Environment.NewLine +
            //                       "Username: " + user.Nickname + Environment.NewLine + "Password: " + user.Password;
            //    }
            //    else
            //    {
            //        message.Subject = "Your CHESS Password has been reset.";
            //        message.Body = "New password for " + user.Nickname + Environment.NewLine +
            //                       "Please activate your account with following link: " /*+ baseurl*/ + Environment.NewLine +
            //                       "Username: " + user.Nickname + Environment.NewLine + "Password: " + user.Password;
            //    }
            //    System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient("smtp.gmail.com");
            //    smtp.UseDefaultCredentials = false;
            //    var credentials = new System.Net.NetworkCredential("bizk0it0k0x0@gmail.com", "******");
            //    smtp.Credentials = credentials;
            //    smtp.EnableSsl = true;

            //    smtp.Port = 587;
            //    string mailState = "State";


            //    smtp.SendAsync(message, mailState);
            //}
            //catch (Exception e)
            //{
            //    Response.Write(e.Message);
            //}

            /*string From = "contactform@gmail.com";
            string To = user.Email;
            string Subject = user.Nickname;
            string Body = user.Nickname + " wrote:<br/><br/>" + "message";

            System.Net.Mail.MailMessage Email = new System.Net.Mail.MailMessage(From, To, Subject, Body);
            System.Net.Mail.SmtpClient SMPTobj = new System.Net.Mail.SmtpClient("smtp.gmail.com");

            SMPTobj.EnableSsl = false;
            SMPTobj.Credentials = new System.Net.NetworkCredential("bizk0it0k0x0@gmail.com", "******");

            try
            {
                SMPTobj.Send(Email);
            }
            catch (Exception ex)
            {
                //Debug.WriteLine(ex.Message);
                throw new Exception();
            }*/
        }
    }
}
