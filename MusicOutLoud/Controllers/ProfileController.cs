using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataAccessLayer;
using DataDomainEntities;
using System.IO;
using System.Web.Security;
using MusicOutLoud.Filters;
using MusicOutLoud.Models;

namespace MusicOutLoud.Controllers
{
    public class ProfileController : Controller
    {
        private readonly IAuthenticationRepository _repo;

        public ProfileController()
        {
            _repo = AuthenticationMemoryLocator.Get();
        }

        //
        // GET: /Profile/

        public ActionResult Profile()
        {
            string name = User.Identity.Name;

            User user = null;

            foreach (User u in _repo.GetAll())
            {
                if (u.Nickname.Equals(name))
                {
                    user = u;
                }

            }

            //var _roles = AuthenticationMemoryLocator.Get().GetAll();
            //user.listOfRoles = new SelectList(DataDomainEntities.Role.

            //    perfil = DataDomainEntities.Perfil.Registered;


            //var aux = _repo.GetById(id);
            //aux.listOfListsInBoard = null;
            //var _repoList = ListsRepositoryLocator.Get().GetAllByBoardId(aux.IdBoard);
            //aux.listOfListsInBoard = new SelectList(_repoList, "Id", "Name");

            var model = new Tuple<IEnumerable<User>, User>(_repo.GetAll(), user);

            if (user.Active)
            {
                return View(model);
            }
            else 
            {
                return RedirectToAction("Index","Home");
            }
        }

        [HttpPost]
        public ActionResult AddImage(HttpPostedFileBase image, int userId)
        {
            User user = _repo.GetById(userId);

            if (image != null)
            {
                if (Request.Files.Count > 0)
                {
                    ////user.Photo = Request.Files[0].FileName.ToString();
                    ////foreach (var file in Request.Files) {
                    //    for (int i = 0; i != Request.Files.Count; ++i ){
                    //        //    // Do your file stuff 
                    //        user.Photo = Request.Files[i].FileName.ToString();
                    //} 
                    var fileName = Path.GetFileName(image.FileName);

                    //var path = Path.Combine(Server.MapPath("~/App_Data/App_LocalResources"), fileName);
                    var path = Path.Combine(Server.MapPath("~/Content/images"), fileName);
                    image.SaveAs(path);
                    user.Photo = fileName;
                }
            }

            return RedirectToAction("Profile", user);
        }

        public ActionResult DeleteUser(int userId)
        {
            User user = _repo.GetById(userId);

            _repo.Remove(user);

            return RedirectToAction("Profile", user);
        }

        public ActionResult DeleteAccount(int userId)
        {
            User user = _repo.GetById(userId);

            _repo.Remove(user);

            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }

        public ActionResult ChangeRole(int userId)
        {
            User user = _repo.GetById(userId);
            var aux = Request.Form[1];

            if (aux == "Registered" || aux == "Administrator")
            {
                user.Active = true;
            }
            else 
            {
                if (aux == "Anonimous")
                    user.Active = false;
            }
            _repo.SetRole(user, aux);

            return RedirectToAction("Profile", user);
        }

        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Account/ChangePassword

        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(LocalPasswordModel model)
        {
            if (ModelState.IsValid)
            {

                // ChangePassword will throw an exception rather
                // than return false in certain failure scenarios.
                bool changePasswordSucceeded = false;
                try
                {
                    string name = User.Identity.Name;

                    User user = null;

                    foreach (User u in _repo.GetAll())
                    {
                        if (u.Nickname.Equals(name))
                        {
                            user = u;
                        }

                    }

                    if (user.Password == model.OldPassword && model.NewPassword == model.ConfirmPassword && model.NewPassword.Length == 6) 
                    {
                        user.Password = model.NewPassword;
                        changePasswordSucceeded = true;
                    }
                    //MembershipUser currentUser = Membership.GetUser(User.Identity.Name, true /* userIsOnline */);
                    //changePasswordSucceeded = currentUser.ChangePassword(model.OldPassword, model.NewPassword);
                }
                catch (Exception)
                {
                    changePasswordSucceeded = false;
                }

                if (changePasswordSucceeded)
                {
                    return RedirectToAction("ChangePasswordSuccess");
                }
                else
                {
                    ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ChangePasswordSuccess

        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }

    }
}
