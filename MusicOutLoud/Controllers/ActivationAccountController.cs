using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace MusicOutLoud.Controllers
{
    public class ActivationAccountController : Controller
    {
        //
        // GET: /ActivationAccount/

        public ActionResult ActivationAccount()
        {
            return View();
        }

        /*public ActionResult Index()
        {
            return View();
        }*/
        public ActionResult SendMail(int id)
        {
            var _auxUser = DataAccessLayer.AuthenticationMemoryLocator.Get();
            var user = _auxUser.GetById(id);
            string subject = "Welcome to MusicOutLoud!";
            StringBuilder message = new StringBuilder();
            message.AppendFormat("<p>Dear {0},</p>", user.Nickname);
            message.Append("<p>Thank you for registering with MusicOutLoud</p>");
            message.Append("<p></p>");
            message.Append("<p>You're almost there, just one step left! Please verify your e-mail address</p>");
            message.Append("<p></p>");
            message.Append("<p>Click on the link below to confirm your registration e-mail.</p>");
            message.Append("<p></p>");
            message.AppendFormat("<a href='http://localhost:60356/Account/ActivationAccount?nickname={0}' target='_blank' >Confirm MusicOutLoud registration</a>", user.Nickname);
            message.Append("<p></p>");
            message.Append("<p>Thank you,</p>");
            message.Append("<p>The MusicOutLoud Team</p>");

            //DataDomainSecurity.Email.SendEmail(user.Email, subject, message.ToString());

            //return RedirectToAction("Registered", "Registered");

            try
            {
                DataDomainSecurity.Email.SendEmail(user.Email, subject, message.ToString());
                return RedirectToAction("Registered", "Registered");
            }
            catch (Exception e)
            {
                ViewBag.SendMailError = "An error in the registration occurred. Please try again later.";
                return View("ActivationAccountError", user);
            }
        }

    }
}
