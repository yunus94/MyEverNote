using MyEvernote.BusinessLayer;
using MyEvernote.Entities;
using MyEvernote.Entities.Messages;
using MyEvernote.Entities.ValueObjects;
using MyEvernote.WebApp.ViewModels;
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
        public ActionResult ShowProfile()
        {
            EvernoteUser currentuser = Session["login"] as EvernoteUser;
            UserManager um = new UserManager();
            BusinessLayerResult<EvernoteUser> res = um.GetUserById(currentuser.Id);
            if (res.Errors.Count>0)
            {
                ErrorViewModel errorNotifyObj = new ErrorViewModel()
                {
                    Title = "Hata oluştu",
                    Items = res.Errors
                };
                return View("Error", errorNotifyObj);
            }
            return View(res.Result);
        }
        public ActionResult EditProfile()
        {
            EvernoteUser currentuser = Session["login"] as EvernoteUser;
            UserManager um = new UserManager();
            BusinessLayerResult<EvernoteUser> res = um.GetUserById(currentuser.Id);
            if (res.Errors.Count > 0)
            {
                ErrorViewModel errorNotifyObj = new ErrorViewModel()
                {
                    Title = "Hata oluştu",
                    Items = res.Errors
                };
                return View("Error", errorNotifyObj);
            }
            return View(res.Result);
        }
        [HttpPost]
        public ActionResult EditProfile(EvernoteUser model,HttpPostedFileBase ProfileImage)
        {
            if (ProfileImage!=null && 
                (ProfileImage.ContentType=="image/jpeg" ||
                 ProfileImage.ContentType == "image/jpg"||
                 ProfileImage.ContentType == "image/png"))
            {
                string filename = $"user_{model.Id}.{ProfileImage.ContentType.Split('/')[1]}";
                ProfileImage.SaveAs(Server.MapPath($"~/Images/{filename}"));
                model.ProfileImageFilename = filename;
            }
            UserManager um = new UserManager();
            BusinessLayerResult<EvernoteUser> res = um.UpdateProfile(model);
            if (res.Errors.Count>0)
            {
                ErrorViewModel errorNotifyObj = new ErrorViewModel()
                {
                    Items=res.Errors,
                    Title="Profil güncellenemedi.",
                    RedirectingUrl="/Home/EditProfile"
                };
                return View("Error", errorNotifyObj);
            }
            Session["login"] = res.Result;//profil güncellendiği için session ı da güncelledim.
            return RedirectToAction("ShowProfile");
        }
        public ActionResult DeleteProfile()
        {
            EvernoteUser currentuser = Session["login"] as EvernoteUser;
            UserManager um = new UserManager();
            BusinessLayerResult<EvernoteUser> res = um.DeleteUserById(currentuser.Id);
            if (res.Errors.Count > 0)
            {
                ErrorViewModel errorNotifyObj = new ErrorViewModel()
                {
                    Title = "Profil silinemedi",
                    Items = res.Errors,
                    RedirectingUrl="/Home/ShowProfile"
                };
                return View("Error", errorNotifyObj);
            }
            Session.Clear();
            return RedirectToAction("Index");
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
                    if (res.Errors.Find(x=>x.Code==ErrorMessageCode.UserIsNotActive)!=null)
                    {
                        ViewBag.Setlink = "/http://Home/Activate/123-123-12313";
                    }
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
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
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    return View(model);
                }
                OkViewModel notifyObj = new OkViewModel()
                {
                    Title = "Kayıt başarılı.",
                    RedirectingUrl = "/Home/Login",
                };
                notifyObj.Items.Add("Lütfen e-posta adresinize gönderdiğimiz aktivasyon linkine tıklayarak hesabınızı aktifleştiriniz. Hesabınızı aktifleştirmeden not paylaşamaz ve beğenme yapamazsınız.");
                return View("Ok",notifyObj);
            }
            return View(model);
        }

        public ActionResult RegisterOk()
        {
            return View();
        }

        public ActionResult UserActivate(Guid id)
        {
            UserManager um = new UserManager();
            BusinessLayerResult<EvernoteUser> res= um.ActivateUser(id);
            if (res.Errors.Count>0)
            {
                ErrorViewModel errorNotifyObj = new ErrorViewModel()
                {
                    Title = "Geçersiz işlem",
                    Items = res.Errors
                };
                return View("Error",errorNotifyObj);
            }
            OkViewModel okNotifyObj = new OkViewModel()
            {
                Title="Hesap aktifleştirildi.",
                RedirectingUrl="/Home/Login",
            };
            okNotifyObj.Items.Add("Hesabınız aktifleştirildi. Artık not paylaşabilir ve beğenme yapabilirsiniz.");
            return View("Ok",okNotifyObj);
        }

   
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index");
        }
    }
}