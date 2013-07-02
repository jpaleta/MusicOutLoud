using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using DataAccessLayer;
using DataDomainEntities;

namespace MusicOutLoud.Controllers
{
    public class HomeController : Controller
    {

        private readonly IPlayListRepository _repo;
        // iv - 2012.11.14
        private readonly IAuthenticationRepository _repoAuthentication;

        public HomeController()
        {
            _repo = PlayListRepositoryLocator.Get();
            // iv - 2012.11.14
            _repoAuthentication = AuthenticationMemoryLocator.Get();
            /*_repoAuthentication.Add(new User { Nickname = "Administrator", Nome = "Administrator", Password = "12345", Email = "idalio@gmail.com", role = Role.Administrator });*/
        }
        //
        // GET: /Default1/

        public ActionResult Index()
        {
            var playlists = _repo.GetAll();

            if (User.Identity.IsAuthenticated)
            {
                string user = User.Identity.Name;

                User auxUser = null;
                var aux = _repoAuthentication.GetAll();
                foreach (User u in aux)
                {
                    if (u.Nickname == user)
                        auxUser = u;
                }

                if (auxUser != null)
                {
                    ViewBag.UserActive = auxUser.Active;
                    ViewBag.UserId = auxUser.Uid;
                }

                return View(playlists);
                //if (auxUser.Active)
                //{
                //    // ok
                //    return View(playlists);
                //}
                //else
                //{
                //    // error: activation account value
                //    return View("ActivationAccountError", auxUser);
                //    //return RedirectToAction("ActivationAccountError", "Account", new { User = auxUser });
                    
                //}
            }

            else 
            {
                return View(playlists);
            }


            
        }

        public ActionResult Delete(int id)
        {
            string user = User.Identity.Name;

            User auxUser = null;
            var aux = _repoAuthentication.GetAll();
            foreach (User u in aux)
            {
                if (u.Nickname == user)
                    auxUser = u;
            }

            if (auxUser != null)
            {
                ViewBag.UserActive = auxUser.Active;
                ViewBag.UserId = auxUser.Uid;
            }

            var curPlaylist = _repo.GetById(id);

            if (curPlaylist.Lists == null)
            {
                PlayList p = _repo.GetById(id);
                _repo.Remove(p);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                if (curPlaylist.Lists.Count == 0)
                {
                    PlayList p = _repo.GetById(id);
                    _repo.Remove(p);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    var playlists = _repo.GetAll();
                    ModelState.AddModelError("removePlaylist", "This playlist contains musics and it can't be removed.");
                    //return RedirectToAction("Board", _repo.GetById(boardId));
                    return View("Index", playlists);
                }
            }
 
        }

        [HttpPost]
        public ActionResult CreatePlayList(string newPlayListName)
        {
            string user = User.Identity.Name;
            var isExisting = false;
            User auxUser = null;
            var aux = _repoAuthentication.GetAll();
            foreach (User u in aux)
            {
                if (u.Nickname == user)
                    auxUser = u;
            }

            if (auxUser != null)
            {
                ViewBag.UserActive = auxUser.Active;
                ViewBag.UserId = auxUser.Uid;
            }
 
            //if (_repo.VerifyUniqueName(newPlayListName))
            //{
                //jf 18-11-2012
                //adicionado o toShare 
                foreach (PlayList b in _repo.GetAll())
                {
                    if (b.Name.Equals(newPlayListName) && b.Owner.Equals(auxUser.Nickname))
                        isExisting = true;
                }
                if (!isExisting)
                {
                    var tdAdd = new PlayList { Name = newPlayListName, toShare = false };
                    tdAdd.Name = newPlayListName;
                    tdAdd.Owner = User.Identity.Name;
                    _repo.Add(tdAdd);
                }
            //}
            //else
            //{
            //    ModelState.AddModelError("newPlayListName", "You need to insert a valid name");
            //    var playlists = _repo.GetAll();
            //    return View("Index", playlists);
            //}

            return RedirectToAction("Index");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        //[HttpGet]
        [HttpPost]
        public ActionResult SearchPlaylistByUser(string newPlayListName) {

            string user = User.Identity.Name;
            //PlayList auxPlaylist = new PlayList();
            User auxUser = null;
            var auxRepo = _repo.GetAll();
            var aux = _repoAuthentication.GetAll();
            foreach (User u in aux)
            {
                if (u.Nickname == user)
                    auxUser = u;
            }

            if (auxUser != null)
            {
                ViewBag.UserActive = auxUser.Active;
                ViewBag.UserId = auxUser.Uid;
            }
            bool isExisting = true;
            //var isExisting = _repo.VerifyUniqueName(newPlayListName);
            //isExisting = _repo.GetPlayListByNameAndUser(auxUser, newPlayListName);


            foreach (PlayList b in auxRepo)
            {
                if (b.Name.Equals(newPlayListName) && b.Owner.Equals(auxUser.Nickname))
                    isExisting = false;
            }


            //if (auxPlaylist.Name != null)
            //{
            //    isExisting = true;
            //}

            //return Json(new { IsExisting = isExisting }, JsonRequestBehavior.AllowGet);
            //return isExisting;
            //return Content(isExisting.ToString());
            //return Json(isExisting);
            if (isExisting)
                return Json(new { Success = true });
            else
                return Json(new { Success = false });

        }

        [HttpPost]
        public ActionResult GetPlaylistsByUser()
        {
            List<PlayList> auxPl = new List<PlayList> ();
            string user = User.Identity.Name;
            //PlayList auxPlaylist = new PlayList();
            User auxUser = null;
            var auxRepo = _repo.GetAll();
            var aux = _repoAuthentication.GetAll();
            foreach (User u in aux)
            {
                if (u.Nickname == user)
                    auxUser = u;
            }

            if (auxUser != null)
            {
                ViewBag.UserActive = auxUser.Active;
                ViewBag.UserId = auxUser.Uid;
            }
            //var isExisting = _repo.VerifyUniqueName(newPlayListName);
            //isExisting = _repo.GetPlayListByNameAndUser(auxUser, newPlayListName);


            foreach (PlayList b in auxRepo)
            {
                if (b.Owner.Equals(auxUser.Nickname))
                    auxPl.Add(b);
            }

            return Json(auxPl);

        }
        //----------------------------------------------------
        //
        // GET: /Default1/Details/5

        //public ActionResult Details(int id)
        //{
        //    return View();
        //}

        ////
        //// GET: /Default1/Create

        //public ActionResult Create()
        //{
        //    return View();
        //}

        ////
        //// POST: /Default1/Create

        //[HttpPost]
        //public ActionResult Create(FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add insert logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        ////
        //// GET: /Default1/Edit/5

        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}

        ////
        //// POST: /Default1/Edit/5

        //[HttpPost]
        //public ActionResult Edit(int id, FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add update logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //
        // GET: /Default1/Delete/5

        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        //
        // POST: /Default1/Delete/5

        //[HttpPost]
        //public ActionResult Delete(int id, FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add delete logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
    }
}
