using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataAccessLayer;
using DataDomainEntities;
using System.Xml;
using System.Text.RegularExpressions;

namespace MusicOutLoud.Controllers
{
    public class PlayListController : Controller
    {
        private readonly IPlayListRepository _repo;
        // iv - 2012.11.11
        //private readonly IListsRepository _repoList;
        //private readonly ICardsArchiveRepository _repoArchive;

        private readonly IAuthenticationRepository _repoUser;

        public PlayListController()
        {
            _repo = PlayListRepositoryLocator.Get();
            //_repoList = ListsRepositoryLocator.Get();
            //_repoArchive = CardsArchiveRepositoryLocator.Get();
            _repoUser = AuthenticationMemoryLocator.Get();
        }


        //
        // GET: /PlayList/

        public ActionResult PlayList(int id)
        {
            var b = new PlayList { Id = id };

            var playlist = _repo.GetById(b.Id);

            IEnumerable<Music> lm;

            try
            {
                lm = playlist.Lists;
            }
            catch (NullReferenceException e)
            {
                lm = null;
            }

            string name = User.Identity.Name;

            User user = null;

            foreach (User u in _repoUser.GetAll())
            {
                if (u.Nickname.Equals(name))
                {
                    user = u;
                }

            }

            var pl = playlist;
            //var model = new Tuple<IEnumerable<User>, User, Board>(_repoUser.GetAll(), user, board);

            var playlists = _repo.GetAll();
            
            //IEnumerable<SelectListItem> items;
            //items = playlists
            //           .Select(c => new SelectListItem
            //           {
            //               Value = c.Name,
            //               Text = c.Name,
            //               Selected = false,
            //           });
            //ViewBag.PlayList_list = items;

            List<PlayList> ddList = new List<PlayList>();
            foreach (PlayList p in playlists)
            {
                if (p.Owner == user.Nickname)
                {
                    ddList.Add(p);
                }
            }

            IEnumerable<SelectListItem> items;
            items = ddList
                       .Select(c => new SelectListItem
                       {
                           Value = c.Name,
                           Text = c.Name,
                           Selected = false,
                       });
            ViewBag.PlayList_list = items;

            var model = new Tuple<IEnumerable<User>, User, PlayList>(_repoUser.GetAll(), user, pl);

            return View(model);
        }


        [HttpPost]
        public ActionResult PlayList(int id, string searchValue, FormCollection collection)
        {
            //try
            //{
            // TODO: Add insert logic here
            if (searchValue != "")
            {
                string searchString = searchValue.Replace(" ", "+");

                string parameterS = "http://ws.spotify.com/search/1/track?q=" + searchString;

                List<Music> musicList = new List<Music>();

                XmlDocument doc = new XmlDocument();

                doc.Load(parameterS);

                XmlNodeList companyList = doc.GetElementsByTagName("track");

                foreach (XmlNode node in companyList)
                {
                    XmlElement companyElement = (XmlElement)node;

                    string name = companyElement.GetElementsByTagName("name")[0].InnerText;
                    //string tracknumber = companyElement.GetElementsByTagName("track-number")[0].InnerText;
                    string length = companyElement.GetElementsByTagName("length")[0].InnerText;
                    //string popularity = companyElement.GetElementsByTagName("popularity")[0].InnerText;
                    //string xmlCompanyID = companyElement.Attributes["ID"].InnerText;
                    string artist = companyElement.GetElementsByTagName("artist")[0].InnerText;
                    string released = companyElement.GetElementsByTagName("released")[0].InnerText;
                    string albumname = companyElement.GetElementsByTagName("album")[0].FirstChild.InnerText;
                    Music m = new Music();

                    m.Name = name;
                    m.Length = length;
                    m.Artist = artist;
                    Album a = new Album();

                    a.Artist = artist;
                    a.Name = albumname;
                    a.Year = released;
                    m.album = a;


                    musicList.Add(m);

                }
                ViewBag.MusicList = musicList;
                ViewBag.SearchParameter = searchValue;
            }

            var b = new PlayList { Id = id };

            var playlist = _repo.GetById(b.Id);

            var playlists = _repo.GetAll();

            //IEnumerable<SelectListItem> items;
            //items = playlists
            //           .Select(c => new SelectListItem
            //           {
            //               Value = c.Name,
            //               Text = c.Name,
            //               Selected = false,
            //           });
            //ViewBag.PlayList_list = items;

            string uname = User.Identity.Name;

            User user = null;

            foreach (User u in _repoUser.GetAll())
            {
                if (u.Nickname.Equals(uname))
                {
                    user = u;
                }

            }

            List<PlayList> ddList = new List<PlayList>();
            foreach (PlayList p in playlists)
            {
                if (p.Owner == user.Nickname)
                {
                    ddList.Add(p);
                }
            }

            IEnumerable<SelectListItem> items;
            items = ddList
                       .Select(c => new SelectListItem
                       {
                           Value = c.Name,
                           Text = c.Name,
                           Selected = false,
                       });
            ViewBag.PlayList_list = items;

            var model = new Tuple<IEnumerable<User>, User, PlayList>(_repoUser.GetAll(), user, playlist);

            return View(model);
        }

        [HttpPost]
        public ActionResult AddMusics(int id, FormCollection collection)
        {
            var playlists = _repo.GetAll();
            //IEnumerable<SelectListItem> items;
            //items = playlists
            //           .Select(c => new SelectListItem
            //           {
            //               Value = c.Name,
            //               Text = c.Name,
            //               Selected = false,
            //           });
            //ViewBag.PlayList_list = items;

            var b = new PlayList { Id = id };

            var playlist = _repo.GetById(b.Id);

            
            List<Music> list = playlist.Lists;

            if (list == null) 
            {
                list = new List<Music>();
            }
           

            int count = 0;

            foreach (string result in collection) 
            {
                if (result.Contains("#&#") && collection[count].Contains("true")) 
                {
                    Music m = new Music();
                    Album a = new Album();

                    string[] res = Regex.Split(result, "#&#");
                    m.Name = res[0];
                    m.Length = res[1];
                    m.Artist = res[2];
                    a.Artist = res[2];
                    a.Name = res[3];
                    a.Year = res[4];
                    m.album = a;
                    m.Id = list.Count;

                    list.Add(m);   
                }
                ++count;
            }

            playlist.Lists = list;

            string uname = User.Identity.Name;

            User user = null;

            foreach (User u in _repoUser.GetAll())
            {
                if (u.Nickname.Equals(uname))
                {
                    user = u;
                }

            }

            List<PlayList> ddList = new List<PlayList>();
            foreach (PlayList p in playlists)
            {
                if (p.Owner == user.Nickname)
                {
                    ddList.Add(p);
                }
            }

            IEnumerable<SelectListItem> items;
            items = ddList
                       .Select(c => new SelectListItem
                       {
                           Value = c.Name,
                           Text = c.Name,
                           Selected = false,
                       });
            ViewBag.PlayList_list = items;

            var model = new Tuple<IEnumerable<User>, User, PlayList>(_repoUser.GetAll(), user, playlist);

            return View("PlayList", model);

            //return View("PlayList",playlist);
        }

        public ActionResult Description(string newDescription, int id)
        {
            var playlists = _repo.GetAll();
            //IEnumerable<SelectListItem> items;
            //items = playlists
            //           .Select(c => new SelectListItem
            //           {
            //               Value = c.Name,
            //               Text = c.Name,
            //               Selected = false,
            //           });
            //ViewBag.PlayList_list = items;

            var b = new PlayList { Id = id };

            var playlist = _repo.GetById(b.Id);

            playlist.Description= newDescription;

            string uname = User.Identity.Name;

            User user = null;

            foreach (User u in _repoUser.GetAll())
            {
                if (u.Nickname.Equals(uname))
                {
                    user = u;
                }

            }

            List<PlayList> ddList = new List<PlayList>();
            foreach (PlayList p in playlists)
            {
                if (p.Owner == user.Nickname)
                {
                    ddList.Add(p);
                }
            }

            IEnumerable<SelectListItem> items;
            items = ddList
                       .Select(c => new SelectListItem
                       {
                           Value = c.Name,
                           Text = c.Name,
                           Selected = false,
                       });
            ViewBag.PlayList_list = items;

            var model = new Tuple<IEnumerable<User>, User, PlayList>(_repoUser.GetAll(), user, playlist);

            return View("PlayList", model);

            //return View("PlayList", playlist);
        }
   
        public ActionResult Delete(int id, int musicId)
        {
            var playlists = _repo.GetAll();
            //IEnumerable<SelectListItem> items;
            //items = playlists
            //           .Select(c => new SelectListItem
            //           {
            //               Value = c.Name,
            //               Text = c.Name,
            //               Selected = false,
            //           });
            //ViewBag.PlayList_list = items;

            var b = new PlayList { Id = id };

            var playlist = _repo.GetById(b.Id);

            playlist.Lists.RemoveAt(musicId);

            int count = 0;
            foreach (Music m in playlist.Lists) 
            {
                m.Id = count;
                ++count;
            }

            

            string uname = User.Identity.Name;

            User user = null;

            foreach (User u in _repoUser.GetAll())
            {
                if (u.Nickname.Equals(uname))
                {
                    user = u;
                }

            }

            List<PlayList> ddList = new List<PlayList>();
            foreach (PlayList p in playlists)
            {
                if (p.Owner == user.Nickname)
                {
                    ddList.Add(p);
                }
            }

            IEnumerable<SelectListItem> items;
            items = ddList
                       .Select(c => new SelectListItem
                       {
                           Value = c.Name,
                           Text = c.Name,
                           Selected = false,
                       });
            ViewBag.PlayList_list = items;

            var model = new Tuple<IEnumerable<User>, User, PlayList>(_repoUser.GetAll(), user, playlist);

            return View("PlayList", model);
        }

        public ActionResult MoveUp(int id, int musicId)
        {
            var playlists = _repo.GetAll();
            //IEnumerable<SelectListItem> items;
            //items = playlists
            //           .Select(c => new SelectListItem
            //           {
            //               Value = c.Name,
            //               Text = c.Name,
            //               Selected = false,
            //           });
            //ViewBag.PlayList_list = items;

            var b = new PlayList { Id = id };

            var playlist = _repo.GetById(b.Id);

            if (musicId != 0)
            {
                Music tempMusic = new Music();
                Album tempAlbum = new Album();
                tempMusic.Id = playlist.Lists.ElementAt(musicId).Id;
                tempMusic.Name = playlist.Lists.ElementAt(musicId).Name;
                tempMusic.Length = playlist.Lists.ElementAt(musicId).Length;
                tempMusic.Artist = playlist.Lists.ElementAt(musicId).Artist;
                tempAlbum.Artist = playlist.Lists.ElementAt(musicId).album.Artist;
                tempAlbum.Name = playlist.Lists.ElementAt(musicId).album.Name;
                tempAlbum.Year = playlist.Lists.ElementAt(musicId).album.Year;

                playlist.Lists.ElementAt(musicId).Id = playlist.Lists.ElementAt(musicId-1).Id;
                playlist.Lists.ElementAt(musicId).Name = playlist.Lists.ElementAt(musicId-1).Name;
                playlist.Lists.ElementAt(musicId).Length = playlist.Lists.ElementAt(musicId-1).Length;
                playlist.Lists.ElementAt(musicId).Artist = playlist.Lists.ElementAt(musicId-1).Artist;
                playlist.Lists.ElementAt(musicId).album.Artist = playlist.Lists.ElementAt(musicId-1).album.Artist;
                playlist.Lists.ElementAt(musicId).album.Name = playlist.Lists.ElementAt(musicId-1).album.Name;
                playlist.Lists.ElementAt(musicId).album.Year = playlist.Lists.ElementAt(musicId-1).album.Year;

                playlist.Lists.ElementAt(musicId - 1).Id = tempMusic.Id;
                playlist.Lists.ElementAt(musicId - 1).Name = tempMusic.Name;
                playlist.Lists.ElementAt(musicId - 1).Length = tempMusic.Length;
                playlist.Lists.ElementAt(musicId - 1).Artist = tempMusic.Artist;
                playlist.Lists.ElementAt(musicId - 1).album.Artist = tempAlbum.Artist;
                playlist.Lists.ElementAt(musicId - 1).album.Name = tempAlbum.Name;
                playlist.Lists.ElementAt(musicId - 1).album.Year = tempAlbum.Year;
                //_repo.Swap(cardId, cardId - 1);

                int count = 0;
                foreach (Music m in playlist.Lists)
                {
                    m.Id = count;
                    ++count;
                }
            }

            string uname = User.Identity.Name;

            User user = null;

            foreach (User u in _repoUser.GetAll())
            {
                if (u.Nickname.Equals(uname))
                {
                    user = u;
                }

            }

            List<PlayList> ddList = new List<PlayList>();
            foreach (PlayList p in playlists)
            {
                if (p.Owner == user.Nickname)
                {
                    ddList.Add(p);
                }
            }

            IEnumerable<SelectListItem> items;
            items = ddList
                       .Select(c => new SelectListItem
                       {
                           Value = c.Name,
                           Text = c.Name,
                           Selected = false,
                       });
            ViewBag.PlayList_list = items;

            var model = new Tuple<IEnumerable<User>, User, PlayList>(_repoUser.GetAll(), user, playlist);

            return View("PlayList", model);

            //return View("PlayList", playlist);
            

            
        }

        public ActionResult MoveDown(int id, int musicId)
        {
            var playlists = _repo.GetAll();
            //IEnumerable<SelectListItem> items;
            //items = playlists
            //           .Select(c => new SelectListItem
            //           {
            //               Value = c.Name,
            //               Text = c.Name,
            //               Selected = false,
            //           });
            //ViewBag.PlayList_list = items;

            var b = new PlayList { Id = id };

            var playlist = _repo.GetById(b.Id);

            if (musicId != playlist.Lists.Count - 1)
            {
                Music tempMusic = new Music();
                Album tempAlbum = new Album();
                tempMusic.Id = playlist.Lists.ElementAt(musicId).Id;
                tempMusic.Name = playlist.Lists.ElementAt(musicId).Name;
                tempMusic.Length = playlist.Lists.ElementAt(musicId).Length;
                tempMusic.Artist = playlist.Lists.ElementAt(musicId).Artist;
                tempAlbum.Artist = playlist.Lists.ElementAt(musicId).album.Artist;
                tempAlbum.Name = playlist.Lists.ElementAt(musicId).album.Name;
                tempAlbum.Year = playlist.Lists.ElementAt(musicId).album.Year;

                playlist.Lists.ElementAt(musicId).Id = playlist.Lists.ElementAt(musicId + 1).Id;
                playlist.Lists.ElementAt(musicId).Name = playlist.Lists.ElementAt(musicId + 1).Name;
                playlist.Lists.ElementAt(musicId).Length = playlist.Lists.ElementAt(musicId + 1).Length;
                playlist.Lists.ElementAt(musicId).Artist = playlist.Lists.ElementAt(musicId + 1).Artist;
                playlist.Lists.ElementAt(musicId).album.Artist = playlist.Lists.ElementAt(musicId + 1).album.Artist;
                playlist.Lists.ElementAt(musicId).album.Name = playlist.Lists.ElementAt(musicId + 1).album.Name;
                playlist.Lists.ElementAt(musicId).album.Year = playlist.Lists.ElementAt(musicId + 1).album.Year;

                playlist.Lists.ElementAt(musicId + 1).Id = tempMusic.Id;
                playlist.Lists.ElementAt(musicId + 1).Name = tempMusic.Name;
                playlist.Lists.ElementAt(musicId + 1).Length = tempMusic.Length;
                playlist.Lists.ElementAt(musicId + 1).Artist = tempMusic.Artist;
                playlist.Lists.ElementAt(musicId + 1).album.Artist = tempAlbum.Artist;
                playlist.Lists.ElementAt(musicId + 1).album.Name = tempAlbum.Name;
                playlist.Lists.ElementAt(musicId + 1).album.Year = tempAlbum.Year;
                //_repo.Swap(cardId, cardId + 1);

                int count = 0;
                foreach (Music m in playlist.Lists)
                {
                    m.Id = count;
                    ++count;
                }
            }

            string uname = User.Identity.Name;

            User user = null;

            foreach (User u in _repoUser.GetAll())
            {
                if (u.Nickname.Equals(uname))
                {
                    user = u;
                }

            }

            List<PlayList> ddList = new List<PlayList>();
            foreach (PlayList p in playlists)
            {
                if (p.Owner == user.Nickname)
                {
                    ddList.Add(p);
                }
            }

            IEnumerable<SelectListItem> items;
            items = ddList
                       .Select(c => new SelectListItem
                       {
                           Value = c.Name,
                           Text = c.Name,
                           Selected = false,
                       });
            ViewBag.PlayList_list = items;

            var model = new Tuple<IEnumerable<User>, User, PlayList>(_repoUser.GetAll(), user, playlist);

            return View("PlayList", model);
            //return View("PlayList", playlist);
            
            
        }

        public ActionResult MoveToList(int id, int musicId, FormCollection collection)
        {
            var playlists = _repo.GetAll();
            //IEnumerable<SelectListItem> items;
            //items = playlists
            //           .Select(c => new SelectListItem
            //           {
            //               Value = c.Name,
            //               Text = c.Name,
            //               Selected = false,
            //           });
            //ViewBag.PlayList_list = items;

            var b = new PlayList { Id = id };

            var playlist = _repo.GetById(b.Id);

            //Get Music Info
            //------------------

            Music tempMusic = new Music();
            Album tempAlbum = new Album();
            tempMusic.Id = playlist.Lists.ElementAt(musicId).Id;
            tempMusic.Name = playlist.Lists.ElementAt(musicId).Name;
            tempMusic.Length = playlist.Lists.ElementAt(musicId).Length;
            tempMusic.Artist = playlist.Lists.ElementAt(musicId).Artist;
            tempAlbum.Artist = playlist.Lists.ElementAt(musicId).album.Artist;
            tempAlbum.Name = playlist.Lists.ElementAt(musicId).album.Name;
            tempAlbum.Year = playlist.Lists.ElementAt(musicId).album.Year;

            //------------------

            //Remove Music from current playlist
            //----------------------------------

            playlist.Lists.RemoveAt(musicId);

            int count = 0;
            foreach (Music m in playlist.Lists)
            {
                m.Id = count;
                ++count;
            }

            //----------------------------------

            //Add Music to selected Playlist
            //-------------------------------

            var selected_playlist = _repo.GetPlayListByName(collection[0]);


            List<Music> list = selected_playlist.Lists;

            if (list == null)
            {
                list = new List<Music>();
            }


            Music sm = new Music();
            Album sa = new Album();

            sm.Name = tempMusic.Name;
            sm.Length = tempMusic.Length;
            sm.Artist = tempMusic.Artist;
            sa.Artist = tempAlbum.Artist;
            sa.Name = tempAlbum.Name;
            sa.Year = tempAlbum.Year;
            sm.album = sa;
            sm.Id = list.Count;

            list.Add(sm);

            selected_playlist.Lists = list;

            //-------------------------------
            string uname = User.Identity.Name;

            User user = null;

            foreach (User u in _repoUser.GetAll())
            {
                if (u.Nickname.Equals(uname))
                {
                    user = u;
                }

            }

            List<PlayList> ddList = new List<PlayList>();
            foreach (PlayList p in playlists)
            {
                if (p.Owner == user.Nickname)
                {
                    ddList.Add(p);
                }
            }

            IEnumerable<SelectListItem> items;
            items = ddList
                       .Select(c => new SelectListItem
                       {
                           Value = c.Name,
                           Text = c.Name,
                           Selected = false,
                       });
            ViewBag.PlayList_list = items;

            var model = new Tuple<IEnumerable<User>, User, PlayList>(_repoUser.GetAll(), user, playlist);

            return View("PlayList", model);
            //return View("PlayList", playlist);
        }


        public ActionResult SharePlayList(int id)
        {
            var playlists = _repo.GetAll();
            //IEnumerable<SelectListItem> items;
            //items = playlists
            //           .Select(c => new SelectListItem
            //           {
            //               Value = c.Name,
            //               Text = c.Name,
            //               Selected = false,
            //           });
            //ViewBag.PlayList_list = items;

            //var b = new PlayList { Id = id };

            //var playlist = _repo.GetById(b.Id);
            PlayList b = _repo.GetById(id);

            b.toShare = true;

            string name = User.Identity.Name;

            User user = null;

            foreach (User u in _repoUser.GetAll())
            {
                if (u.Nickname.Equals(name))
                {
                    user = u;
                }

            }

            List<PlayList> ddList = new List<PlayList>();
            foreach (PlayList p in playlists)
            {
                if (p.Owner == user.Nickname)
                {
                    ddList.Add(p);
                }
            }

            IEnumerable<SelectListItem> items;
            items = ddList
                       .Select(c => new SelectListItem
                       {
                           Value = c.Name,
                           Text = c.Name,
                           Selected = false,
                       });
            ViewBag.PlayList_list = items;

            var model = new Tuple<IEnumerable<User>, User, PlayList>(_repoUser.GetAll(), user, _repo.GetById(id));

            return View("PlayList", model);
        }

        public ActionResult CancelSharingPlayList(int id)
        {
            var playlists = _repo.GetAll();
            

            //var b = new PlayList { Id = id };

            //var playlist = _repo.GetById(b.Id);
            PlayList b = _repo.GetById(id);

            b.toShare = false;

            if (b.AuthorisedUsersRead != null)
                b.AuthorisedUsersRead.Clear();

            if (b.AuthorisedUsersWrite != null)
                b.AuthorisedUsersWrite.Clear();

            string name = User.Identity.Name;

            User user = null;

            foreach (User u in _repoUser.GetAll())
            {
                if (u.Nickname.Equals(name))
                {
                    user = u;
                }

            }

            List<PlayList> ddList = new List<PlayList>();
            foreach(PlayList p in playlists)
            {
                if (p.Owner == user.Nickname) 
                {
                    ddList.Add(p);
                }
            }

            IEnumerable<SelectListItem> items;
            items = ddList
                       .Select(c => new SelectListItem
                       {
                           Value = c.Name,
                           Text = c.Name,
                           Selected = false,
                       });
            ViewBag.PlayList_list = items;

            var model = new Tuple<IEnumerable<User>, User, PlayList>(_repoUser.GetAll(), user, _repo.GetById(id));

            return View("PlayList", model);
        }

        public ActionResult UpdateUsersSharingPlayList(int id)
        {
            var playlists = _repo.GetAll();
            //IEnumerable<SelectListItem> items;
            //items = playlists
            //           .Select(c => new SelectListItem
            //           {
            //               Value = c.Name,
            //               Text = c.Name,
            //               Selected = false,
            //           });
            //ViewBag.PlayList_list = items;

            //var b = new PlayList { Id = id };

            //var playlist = _repo.GetById(b.Id);
            PlayList b = _repo.GetById(id);
            int i = 0;
            var auxArray = Request.Form[0].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            //b.toShare = false;

            string name = User.Identity.Name;

            User user = null;

            foreach (User u in _repoUser.GetAll())
            {
                if (u.Nickname.Equals(name) == false)
                {
                    Permition aux;
                    Permition.TryParse(auxArray[i++], out aux);
                    _repo.SetUserPermition(id, u.Nickname, aux);
                }
                else
                {
                    user = u;
                }

            }

            List<PlayList> ddList = new List<PlayList>();
            foreach (PlayList p in playlists)
            {
                if (p.Owner == user.Nickname)
                {
                    ddList.Add(p);
                }
            }

            IEnumerable<SelectListItem> items;
            items = ddList
                       .Select(c => new SelectListItem
                       {
                           Value = c.Name,
                           Text = c.Name,
                           Selected = false,
                       });
            ViewBag.PlayList_list = items;

            var model = new Tuple<IEnumerable<User>, User, PlayList>(_repoUser.GetAll(), user, _repo.GetById(id));

            return View("PlayList", model);
        }

        //
        // GET: /PlayList/Details/5

        //public ActionResult Details(int id)
        //{
        //    return View();
        //}

        ////
        //// GET: /PlayList/Create

        //public ActionResult Create()
        //{
        //    return View();
        //}

        ////
        //// POST: /PlayList/Create

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
        //// GET: /PlayList/Edit/5

        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}

        ////
        //// POST: /PlayList/Edit/5

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
        // GET: /PlayList/Delete/5

        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        //
        // POST: /PlayList/Delete/5

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
