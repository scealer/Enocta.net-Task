using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Mvc;
using KTOtomasyon.Models;

namespace KTOtomasyon.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(Users user)
        {/*
            using (var db = new KTOtomasyonEntities())
            {
                var userLogin = db.Users.FirstOrDefault(x => x.Mail == user.Mail && x.Password == user.Password && x.IsDeleted == false);

                if (userLogin != null)
                {
                    Session["UserId"] = userLogin.User_Id;
                    Session["Name"] = userLogin.Name;
                    //Shared.SendLogMail();
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.Mesaj = "Hatalı kullanıcı adı ya da şifre!";
                    return View();
                }
            }
            */
        }

        public ActionResult LogOut()
        {
            Session["UserId"] = null;
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Index", "Login");
        }

        public ActionResult SignUp()
        {
            return View();
        }

        public ActionResult UserProfile(int id)
        {
            if (Session["UserId"] == null)
            {

                return RedirectToAction("Index", "Login");
            }
            else
            {
                Users userdetail = null;

                using (var db = new KTOtomasyonEntities())
                {
                    userdetail = db.Users.Where(d => d.User_Id == id).First();
                }
                return View(userdetail);
            }
        }

        public ActionResult Support()
        {
            return View();
        }

    }
}
