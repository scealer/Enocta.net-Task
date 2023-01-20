using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using KTOtomasyon;

namespace KTOtomasyon.Controllers
{
    public class MailController : Controller
    {
        private KTOtomasyonEntities db = new KTOtomasyonEntities();

        // GET: Mail
        public ActionResult Index()
        {
            return View(db.Mails.ToList());
        }

        // GET: Mail/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mails mails = db.Mails.Find(id);
            if (mails == null)
            {
                return HttpNotFound();
            }
            return View(mails);
        }

        // GET: Mail/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Mail/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Mail_Id,MailSubject,MailBody,MailTo,MailCC,MailBCC,IsBodyHtml,IsSend,SendDate,ErrorMessage,CreatedDate,CreatedUser")] Mails mails)
        {
            if (ModelState.IsValid)
            {
                db.Mails.Add(mails);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(mails);
        }

        // GET: Mail/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mails mails = db.Mails.Find(id);
            if (mails == null)
            {
                return HttpNotFound();
            }
            return View(mails);
        }

        // POST: Mail/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Mail_Id,MailSubject,MailBody,MailTo,MailCC,MailBCC,IsBodyHtml,IsSend,SendDate,ErrorMessage,CreatedDate,CreatedUser")] Mails mails)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mails).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(mails);
        }

        // GET: Mail/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mails mails = db.Mails.Find(id);
            if (mails == null)
            {
                return HttpNotFound();
            }
            return View(mails);
        }

        // POST: Mail/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Mails mails = db.Mails.Find(id);
            db.Mails.Remove(mails);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        //Body ve subject mail gönderir
        public ActionResult NowSendMail()
        {
            try
            {
                SmtpClient smtp = new SmtpClient("smtp-mail.outlook.com", 587); //587
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential("simpleterzi3428@outlook.com", "********");

                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("simpleterzi3428@outlook.com", "Simple Terzi - Axis");
                mail.To.Add(new MailAddress("simpleterzi3428@outlook.com"));
                mail.Bcc.Add(new MailAddress("zubeyir.kocalioglu@gmail.com", "Zübeyir KOÇALİOĞLU"));

                //Mails addmail = new Mails();

                using (var db = new KTOtomasyonEntities())
                {
                    var Data = db.vTodayTotalOrder.OrderByDescending(x => x.Sira).ToList();
                    var LogData = db.Logs.OrderByDescending(x => x.Log_Id).ToList();

                    var ThisMessageBody = Data.First();
                    var ThisMessageBody2 = LogData.First();

                    mail.Subject = "Simple Terzi Sipariş Rapor";
                    mail.Body = "Bugün, Toplam sipariş miktarı '";
                    mail.Body += ThisMessageBody.SipMiktar + "' ve sipariş tutarı '" + ThisMessageBody.SipTutar + "'₺ dir.";
                    mail.Body += "Hata mesajı method adı :  '" + ThisMessageBody2.MethodName + "' ve Exception mesajı : '" + ThisMessageBody2.ExMessage;

                }
                smtp.Send(mail);

            }
            catch (Exception ex)
            {
                ex.AddToDBLog("SendMail", ex.Message);
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
