﻿using MyEvernote.BusinessLayer;
using MyEvernote.Entities;
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
            
            return View("Index", cat.Notes.OrderByDescending(x=>x.ModifiedOn).ToList());
        }
        public ActionResult MostLiked()
        {
            NoteManager nm = new NoteManager();
            return View("Index",nm.GetAllNote().OrderByDescending(x => x.LikeCount).ToList());//Not sıralaması bittiğinde Index sayfasını döndürdüğünden her bir işlem için action oluşturmamıza gerek kalmıyor.
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
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterViewModel model)
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