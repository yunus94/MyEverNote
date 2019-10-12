using MyEvernote.BusinessLayer;
using MyEvernote.Entities;
using MyEvernote.Entities.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MyEvernote.WebApp.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            //CategoryController üzerinden gelen view talebi ve model
            //if (TempData["mm"]!=null)
            //{
            //    return View(TempData["mm"] as List<Note>);
            //}
            NoteManager nm = new NoteManager();
            return View(nm.GetAllNote().OrderByDescending(x => x.ModifiedOn).ToList());

            //Veritabanı ile yapılan işlerde kullanılabilir.
            //return View(nm.GetAllNoteQueryable().OrderByDescending(x => x.ModifiedOn).ToList()); 
        }
        public ActionResult ByCategory(int? id)
        {
            if (id == null)
            {

                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            CategoryManager cm = new CategoryManager();
            Category cat = cm.GetCategoryById(id.Value);
            if (cat == null)
            {
                return HttpNotFound();
                //return RedirectToAction("Index", "Home");
            }

            return View("Index", cat.Notes.OrderByDescending(x => x.ModifiedOn).ToList());
        }
        public ActionResult MostLiked()
        {
            NoteManager nm = new NoteManager();
            return View("Index", nm.GetAllNote().OrderByDescending(x => x.LikeCount).ToList());//Not sıralaması bittiğinde Index sayfasını döndürdüğünden her bir işlem için action oluşturmamıza gerek kalmıyor.
        }
        public ActionResult About()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                UserManager um = new UserManager();
                BusinessLayerResult<EvernoteUser> res = um.LoginUser(model);
                if (res.Errors.Count > 0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x));
                    return View(model);
                }
                Session["Login"] = res.Result;      //session a kullanıcı bilgisi saklama.
                return RedirectToAction("Index");   //yönlendirme. 
            }
            return View(model);
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                UserManager um = new UserManager();

                BusinessLayerResult<EvernoteUser> res = um.RegisterUser(model);
                if (res.Errors.Count > 0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x));
                    return View(model);
                }
                //EvernoteUser user = null;
                //try
                //{
                //   user= um.RegisterUser(model);
                //}
                //catch (Exception ex)
                //{
                //    ModelState.AddModelError("", ex.Message);
                //    throw;
                //}
                //if (model.Username == "yunus")
                //{
                //    ModelState.AddModelError("", "Kullanıcı adı kullanılıyor");
                //}

                //if (model.Email == "yunus@gmail.com")
                //{
                //    ModelState.AddModelError("", "Kullanıcı e-postası kullanılıyor");
                //}
                //foreach (var item in ModelState)
                //{
                //    if (item.Value.Errors.Count > 0)
                //    {
                //        return View(model);
                //    }
                //}

                return RedirectToAction("RegisterOk");
            }
            return View(model);
        }

        public ActionResult RegisterOk()
        {
            return View();
        }

        public ActionResult UserActivate(Guid activate_id)
        {
            return View();
        }

        public ActionResult Logout()
        {
            return View();
        }
    }
}