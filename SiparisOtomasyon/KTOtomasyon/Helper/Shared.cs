using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

namespace KTOtomasyon.Controllers
{

    public class ReturnValue
    {
        public string error;        //Hata mesajı içeriği
        public string message;      //Server taraflı işlem sonu dönen mesaj içeriği
        public bool requiredLogin;  //Session kontrolü
        public object retObject;    //İşlem sonu dönen data objesi
        public bool success;        //İşlem başarı kontrolü

        public ReturnValue() { }
    }

    public static class Shared
    {
        //Oturum kontrolu yapar.
        public static bool CheckSession()
        {
            if (HttpContext.Current.Session["UserId"] == null)
            {
                return false;
            }
            return true;
        }

        //Veritabanını yedekler.
        public static void BackupDB()
        {
            SqlConnection sqlcon = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLKTOtomasyon"].ConnectionString);
            SqlCommand sqlcmd;

            string destdir = "C:\\backupdb";

            if (!System.IO.Directory.Exists(destdir))
            {
                System.IO.Directory.CreateDirectory("C:\\backupdb");
            }
            try
            {
                sqlcon.Open();
                sqlcmd = new SqlCommand("backup database KTOtomasyon to disk='" + destdir + "\\" + DateTime.Now.ToString("dd.MM.yyyy_HHmmss") + ".bak'", sqlcon);
                sqlcmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                AddToDBLog(ex, "Shared.BackupDB", ex.Message);
            }
        }

        //Veritabanını restore eder.
        public static void RestoreDB()
        {
            try
            {
                SqlConnection sqlcon = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLKTOtomasyon"].ConnectionString);
                SqlCommand sqlcmd;

                //string destdir = "C:\\backupdb\\11082014_121403.Bak";

                sqlcon.Open();
                sqlcmd = new SqlCommand("Restore database KTOtomasyon from disk='C:\\backupdb\\14032018_082848.Bak' ", sqlcon);
                sqlcmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                AddToDBLog(ex, "Shared.RestoreDB", ex.Message);
            }
        }


        //Veritabanına log ekler.
        public static void AddToDBLog(this Exception exc, string MethodName, string Message = "")
        {
            Logs logum = new Logs();

            using (var db = new KTOtomasyonEntities())
            {
                logum.CreatedDate = DateTime.Now;
                logum.CreatedUser = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
                logum.ExMessage = exc.ToString();
                logum.MethodName = MethodName;
                logum.Message = Message;

                db.Logs.Add(logum);
                db.SaveChanges();
            }
        }

        //Veritabanına mail ekler.
        public static void AddToDBMail(string subject, string body, string fromadd, string toadd)
        {
            Mails mailim = new Mails();

            using (var db = new KTOtomasyonEntities())
            {
                mailim.CreatedDate = DateTime.Now;
                mailim.CreatedUser = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
                mailim.MailTo = toadd.ToString();
                mailim.MailFrom = fromadd.ToString();
                mailim.MailBody = body.ToString();
                mailim.MailSubject = subject.ToString();
                mailim.SendDate = DateTime.Now;
                mailim.IsSend = true;
                mailim.IsBodyHtml = false;

                db.Mails.Add(mailim);
                db.SaveChanges();
            }
        }

        //Sisteme giriş emaili gönderir
        public static void SendLogMail()
        {
            SmtpClient smtp = new SmtpClient("smtp-mail.outlook.com", 587);
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new System.Net.NetworkCredential("simpleterzi3428@outlook.com", "3428simple");
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.EnableSsl = true;

            smtp.Timeout = 600000;
           
            SqlConnection ConnStr = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLKTOtomasyon"].ConnectionString);
            string body = "";

            try
            {

                body += "<br>";

                String SQL2 = "SELECT Sira, SipMiktar, SipTutar FROM vLastTotalOrder "
                + "WHERE Sira IS NOT NULL";
                SqlDataAdapter TitlesAdpt2 = new SqlDataAdapter(SQL2, ConnStr);
                DataSet Titles2 = new DataSet();

                //2.Tablo
                TitlesAdpt2.Fill(Titles2);
                //Header
                body = "<h3 align='center'>Simple Terzi - Haftanın Sipariş Raporu</h3>";
                //Content
                body += "<table width='70%' bgcolor='#f6f8f1' border='0' cellpadding='0' cellspacing='0' class='table table-striped text-center' align='center'>";
                body += "<tr bgcolor='#c7d8a7'>";
                body += "<td><strong>Sipariş Miktarı<strong></td>";
                body += "<td><strong>Sipariş Tutarı<strong></td>";
                body += "</tr>";
                foreach (DataRow Title in Titles2.Tables[0].Rows)
                {
                    body += "<tr>";
                    body += "<td>" + Title[1] + " Adet </td>";
                    body += "<td>" + String.Format("{0:c}", Title[2]) + "</td>";
                    body += "</tr>";
                }
                body += "</table>";

                body += "<br>";

                String SQL3 = "SELECT [Year],[Ocak],[Subat],[Mart],[Nisan],[Mayis],[Haziran],[Temmuz],[Agustos],[Eylul],[Ekim],[Kasim],[Aralik] FROM vOrderWYear "
                + "WHERE Year IS NOT NULL";
                SqlDataAdapter TitlesAdpt3 = new SqlDataAdapter(SQL3, ConnStr);
                DataSet Titles3 = new DataSet();

                //3.Tablo
                TitlesAdpt3.Fill(Titles3);
                //Header
                body += "<h3 align='center'>Simple Terzi - Aylık Sipariş Tutar Raporu</h3>";
                //Content
                body += "<table width='70%' bgcolor='#f6f8f1' border='0' cellpadding='0' cellspacing='0' class='table table-striped text-center' align='center'>";
                body += "<tr bgcolor='#c7d8a7'>";
                body += "<td><strong>Yıl<strong></td>";
                body += "<td><strong>Ocak<strong></td>";
                body += "<td><strong>Şubat<strong></td>";
                body += "<td><strong>Mart<strong></td>";
                body += "<td><strong>Nisan<strong></td>";
                body += "<td><strong>Mayıs<strong></td>";
                body += "<td><strong>Haziran<strong></td>";
                body += "<td><strong>Temmuz<strong></td>";
                body += "<td><strong>Ağustos<strong></td>";
                body += "<td><strong>Eylül<strong></td>";
                body += "<td><strong>Ekim<strong></td>";
                body += "<td><strong>Kasım<strong></td>";
                body += "<td><strong>Aralık<strong></td>";
                body += "</tr>";
                foreach (DataRow Title in Titles3.Tables[0].Rows)
                {
                    body += "<tr>";
                    body += "<td>" + Title[0] + "</td>";
                    body += "<td>" + String.Format("{0:c}", Title[1]) + "</td>";
                    body += "<td>" + String.Format("{0:c}", Title[2]) + "</td>";
                    body += "<td>" + String.Format("{0:c}", Title[3]) + "</td>";
                    body += "<td>" + String.Format("{0:c}", Title[4]) + "</td>";
                    body += "<td>" + String.Format("{0:c}", Title[5]) + "</td>";
                    body += "<td>" + String.Format("{0:c}", Title[6]) + "</td>";
                    body += "<td>" + String.Format("{0:c}", Title[7]) + "</td>";
                    body += "<td>" + String.Format("{0:c}", Title[8]) + "</td>";
                    body += "<td>" + String.Format("{0:c}", Title[9]) + "</td>";
                    body += "<td>" + String.Format("{0:c}", Title[10]) + "</td>";
                    body += "<td>" + String.Format("{0:c}", Title[11]) + "</td>";
                    body += "<td>" + String.Format("{0:c}", Title[12]) + "</td>";
                    body += "</tr>";
                }
                body += "</table>";

                body += "<br>";

                String SQL4 = "SELECT [Year],[Ocak],[Subat],[Mart],[Nisan],[Mayis],[Haziran],[Temmuz],[Agustos],[Eylul],[Ekim],[Kasim],[Aralik] FROM vOrderQYear "
                + "WHERE Year IS NOT NULL";
                SqlDataAdapter TitlesAdpt4 = new SqlDataAdapter(SQL4, ConnStr);
                DataSet Titles4 = new DataSet();

                //4.Tablo
                TitlesAdpt4.Fill(Titles4);
                //Header
                body += "<h3 align='center'>Simple Terzi - Aylık Sipariş Miktar Raporu</h3>";
                //Content
                body += "<table width='70%' bgcolor='#f6f8f1' border='0' cellpadding='0' cellspacing='0' class='table table-striped text-center' align='center'>";
                body += "<tr bgcolor='#c7d8a7'>";
                body += "<td><strong>Yıl<strong></td>";
                body += "<td><strong>Ocak<strong></td>";
                body += "<td><strong>Şubat<strong></td>";
                body += "<td><strong>Mart<strong></td>";
                body += "<td><strong>Nisan<strong></td>";
                body += "<td><strong>Mayıs<strong></td>";
                body += "<td><strong>Haziran<strong></td>";
                body += "<td><strong>Temmuz<strong></td>";
                body += "<td><strong>Ağustos<strong></td>";
                body += "<td><strong>Eylül<strong></td>";
                body += "<td><strong>Ekim<strong></td>";
                body += "<td><strong>Kasım<strong></td>";
                body += "<td><strong>Aralık<strong></td>";
                body += "</tr>";
                foreach (DataRow Title in Titles4.Tables[0].Rows)
                {
                    body += "<tr>";
                    body += "<td>" + Title[0] + "</td>";
                    body += "<td>" + Title[1] + "</td>";
                    body += "<td>" + Title[2] + "</td>";
                    body += "<td>" + Title[3] + "</td>";
                    body += "<td>" + Title[4] + "</td>";
                    body += "<td>" + Title[5] + "</td>";
                    body += "<td>" + Title[6] + "</td>";
                    body += "<td>" + Title[7] + "</td>";
                    body += "<td>" + Title[8] + "</td>";
                    body += "<td>" + Title[9] + "</td>";
                    body += "<td>" + Title[10] + "</td>";
                    body += "<td>" + Title[11] + "</td>";
                    body += "<td>" + Title[12] + "</td>";
                    body += "</tr>";
                }
                body += "</table>";

                //Footer
                body += "</hr>";
                body += "<h5 align='center'>";
                body += "zuTTers, İstanbul 2018 &copy; ";
                body += "<a href='mailto:zubeyir.kocalioglu@gmail.com'><font color='red'> E-Posta </font></a> | Zübeyir Koçalioğlu tarafından gönderildi.";
                body += "</h5>";

                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("simpleterzi3428@outlook.com", "Simple Terzi - Axis");
                mail.To.Add(new MailAddress("simpleterzi3428@outlook.com"));
                mail.Bcc.Add(new MailAddress("zubeyir.kocalioglu@gmail.com", "Zübeyir KOÇALİOĞLU"));
                mail.Subject = "Simple Terzi Rapor - Genel Rapor";
                mail.Body = body;
                mail.IsBodyHtml = true;

                ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };

                Shared.AddToDBMail(mail.Body, mail.Subject, mail.From.ToString(), mail.To.ToString());

                smtp.Send(mail);


            }
            catch (Exception ex)
            {
                ex.AddToDBLog("SendLogMail", ex.Message);
            }

        }

        public static void SendOrderMail()
        {
            SmtpClient smtp = new SmtpClient("smtp-mail.outlook.com", 587);
            smtp.UseDefaultCredentials = false;

            smtp.Credentials = new System.Net.NetworkCredential("simpleterzi3428@outlook.com", "3428simple");
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.EnableSsl = true;

            smtp.Timeout = 600000;

            smtp.Credentials = new System.Net.NetworkCredential("simpleterzi3428@outlook.com", "pass");



            string body = "";
            SqlConnection ConnStr = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLKTOtomasyon"].ConnectionString);
            String SQL = "SELECT Sira, SipMiktar, SipTutar FROM vLastTotalOrder "
                       + "WHERE Sira IS NOT NULL";
            SqlDataAdapter TitlesAdpt = new SqlDataAdapter(SQL, ConnStr);
            DataSet Titles = new DataSet();
            try
            {
                TitlesAdpt.Fill(Titles);

                //Header
                body = "<h3 align='center'>Simple Terzi - Haftanın Sipariş Raporu</h3>";

                //Content
                body += "<table width='70%' bgcolor='#f6f8f1' border='0' cellpadding='0' cellspacing='0' class='table table-striped text-center' align='center'>";
                body += "<tr bgcolor='#c7d8a7'>";
                body += "<td><strong>Sipariş Miktarı<strong></td>";
                body += "<td><strong>Sipariş Tutarı<strong></td>";
                body += "</tr>";
                foreach (DataRow Title in Titles.Tables[0].Rows)
                {
                    body += "<tr>";
                    body += "<td>" + Title[1] + " Adet </td>";
                    body += "<td>" + String.Format("{0:c}", Title[2]) + "</td>";
                    body += "</tr>";
                }
                body += "</table>";

                String SQL1 = "SELECT TOP 10 Log_Id, MethodName, Message, CreatedDate FROM Logs WHERE Log_Id IS NOT NULL Order by Log_Id desc";
                SqlDataAdapter TitlesAdpt1 = new SqlDataAdapter(SQL1, ConnStr);
                DataSet Titles1 = new DataSet();

                //1.Tablo
                TitlesAdpt1.Fill(Titles1);
                //Header1
                body += "<h3 align='center'>Simple Terzi - Son 10 Hata Raporu</h3>";
                //Content1
                body += "<table width='70%' bgcolor='#f6f8f1' border='0' cellpadding='0' cellspacing='0' class='table table-striped text-center' align='center'>";
                body += "<tr bgcolor='#c7d8a7'>";
                body += "<td><strong>Log ID<strong></td>";
                body += "<td><strong>Metod Adı<strong></td>";
                body += "<td><strong>Hata Mesajı<strong></td>";
                body += "<td><strong>Oluşma Tarihi<strong></td>";
                body += "</tr>";
                foreach (DataRow Title in Titles1.Tables[0].Rows)
                {
                    body += "<tr>";
                    body += "<td>" + Title[0] + "</td>";
                    body += "<td>" + Title[1] + "</td>";
                    body += "<td>" + Title[2] + "</td>";
                    body += "<td>" + String.Format("{0:d/M/yyyy HH:mm:ss}", Title[3]) + "</td>";
                    body += "</tr>";
                }
                body += "</table>";


                //Footer
                body += "</hr>";
                body += "<h5 align='center'>";
                body += "zuTTers, İstanbul 2018 ";
                body += "<a href='twitter.com/zkocalioglu'><font color='red'> İletişim </font></a> | Zübeyir Koçalioğlu tarafından gönderildi.";
                body += "</h5>";




                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("simpleterzi3428@outlook.com", "Simple Terzi - Axis");
                mail.To.Add(new MailAddress("zubeyir.kocalioglu@gmail.com"));
                mail.Bcc.Add(new MailAddress("zubeyir.kocalioglu@gmail.com", "Zübeyir KOÇALİOĞLU"));
                mail.Subject = "Simple Terzi Rapor - Sipariş Raporu";
                mail.Body = body;
                mail.IsBodyHtml = true;

                ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };

                Shared.AddToDBMail(mail.Body, mail.Subject, mail.From.ToString(), mail.To.ToString());

                smtp.Send(mail);


            }
            catch (Exception ex)
            {
                ex.AddToDBLog("SendOrderMail", ex.Message);
            }

        }

        public static void SendOrderWMail()
        {
            SmtpClient smtp = new SmtpClient("smtp-mail.outlook.com", 587);
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new System.Net.NetworkCredential("simpleterzi3428@outlook.com", "3428simple");
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.EnableSsl = true;

            smtp.Timeout = 600000;


            string body = "";
            SqlConnection ConnStr = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLKTOtomasyon"].ConnectionString);
            String SQL = "SELECT [Year],[Ocak],[Subat],[Mart],[Nisan],[Mayis],[Haziran],[Temmuz],[Agustos],[Eylul],[Ekim],[Kasim],[Aralik] FROM vOrderWYear "
                       + "WHERE Year IS NOT NULL";
            SqlDataAdapter TitlesAdpt = new SqlDataAdapter(SQL, ConnStr);
            DataSet Titles = new DataSet();
            try
            {
                TitlesAdpt.Fill(Titles);

                //Header
                body = "<h3 align='center'>Simple Terzi - Aylık Sipariş Tutar Raporu</h3>";

                //Content
                body += "<table width='70%' bgcolor='#f6f8f1' border='0' cellpadding='0' cellspacing='0' class='table table-striped text-center' align='center'>";
                body += "<tr bgcolor='#c7d8a7'>";
                body += "<td><strong>Yıl<strong></td>";
                body += "<td><strong>Ocak<strong></td>";
                body += "<td><strong>Şubat<strong></td>";
                body += "<td><strong>Mart<strong></td>";
                body += "<td><strong>Nisan<strong></td>";
                body += "<td><strong>Mayıs<strong></td>";
                body += "<td><strong>Haziran<strong></td>";
                body += "<td><strong>Temmuz<strong></td>";
                body += "<td><strong>Ağustos<strong></td>";
                body += "<td><strong>Eylül<strong></td>";
                body += "<td><strong>Ekim<strong></td>";
                body += "<td><strong>Kasım<strong></td>";
                body += "<td><strong>Aralık<strong></td>";
                body += "</tr>";
                foreach (DataRow Title in Titles.Tables[0].Rows)
                {
                    body += "<tr>";
                    body += "<td>" + Title[0] + "</td>";
                    body += "<td>" + String.Format("{0:c}", Title[1]) + "</td>";
                    body += "<td>" + String.Format("{0:c}", Title[2]) + "</td>";
                    body += "<td>" + String.Format("{0:c}", Title[3]) + "</td>";
                    body += "<td>" + String.Format("{0:c}", Title[4]) + "</td>";
                    body += "<td>" + String.Format("{0:c}", Title[5]) + "</td>";
                    body += "<td>" + String.Format("{0:c}", Title[6]) + "</td>";
                    body += "<td>" + String.Format("{0:c}", Title[7]) + "</td>";
                    body += "<td>" + String.Format("{0:c}", Title[8]) + "</td>";
                    body += "<td>" + String.Format("{0:c}", Title[9]) + "</td>";
                    body += "<td>" + String.Format("{0:c}", Title[10]) + "</td>";
                    body += "<td>" + String.Format("{0:c}", Title[11]) + "</td>";
                    body += "<td>" + String.Format("{0:c}", Title[12]) + "</td>";
                    body += "</tr>";
                }
                body += "</table>";

                //Footer
                body += "</hr>";
                body += "<h5 align='center'>";
                body += "zuTTers, İstanbul 2018 ";
                body += "<a href='twitter.com/zkocalioglu'><font color='red'> İletişim </font></a> | Zübeyir Koçalioğlu tarafından gönderildi.";
                body += "</h5>";

                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("simpleterzi3428@outlook.com", "Simple Terzi - Axis");
                mail.To.Add(new MailAddress("simpleterzi3428@outlook.com"));
                mail.Bcc.Add(new MailAddress("zubeyir.kocalioglu@gmail.com", "Zübeyir KOÇALİOĞLU"));
                mail.Subject = "Simple Terzi Rapor - Ay Bazında Tutar Raporu";
                mail.Body = body;
                mail.IsBodyHtml = true;

                ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };

                Shared.AddToDBMail(mail.Body, mail.Subject, mail.From.ToString(), mail.To.ToString());

                smtp.Send(mail);


            }
            catch (Exception ex)
            {
                ex.AddToDBLog("SendOrderWMail", ex.Message);
            }

        }

        public static void SendOrderQMail()
        {
            SmtpClient smtp = new SmtpClient("smtp-mail.outlook.com", 587);
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new System.Net.NetworkCredential("simpleterzi3428@outlook.com", "3428simple");
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.EnableSsl = true;

            smtp.Timeout = 600000;


            string body = "";
            SqlConnection ConnStr = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLKTOtomasyon"].ConnectionString);
            String SQL = "SELECT [Year],[Ocak],[Subat],[Mart],[Nisan],[Mayis],[Haziran],[Temmuz],[Agustos],[Eylul],[Ekim],[Kasim],[Aralik] FROM vOrderQYear "
                       + "WHERE Year IS NOT NULL";
            SqlDataAdapter TitlesAdpt = new SqlDataAdapter(SQL, ConnStr);
            DataSet Titles = new DataSet();
            try
            {
                TitlesAdpt.Fill(Titles);

                //Header
                body = "<h3 align='center'>Simple Terzi - Aylık Sipariş Miktar Raporu</h3>";

                //Content
                body += "<table width='70%' bgcolor='#f6f8f1' border='0' cellpadding='0' cellspacing='0' class='table table-striped text-center' align='center'>";
                body += "<tr bgcolor='#c7d8a7'>";
                body += "<td><strong>Yıl<strong></td>";
                body += "<td><strong>Ocak<strong></td>";
                body += "<td><strong>Şubat<strong></td>";
                body += "<td><strong>Mart<strong></td>";
                body += "<td><strong>Nisan<strong></td>";
                body += "<td><strong>Mayıs<strong></td>";
                body += "<td><strong>Haziran<strong></td>";
                body += "<td><strong>Temmuz<strong></td>";
                body += "<td><strong>Ağustos<strong></td>";
                body += "<td><strong>Eylül<strong></td>";
                body += "<td><strong>Ekim<strong></td>";
                body += "<td><strong>Kasım<strong></td>";
                body += "<td><strong>Aralık<strong></td>";
                body += "</tr>";
                foreach (DataRow Title in Titles.Tables[0].Rows)
                {
                    body += "<tr>";
                    body += "<td>" + Title[0] + "</td>";
                    body += "<td>" + Title[1] + "</td>";
                    body += "<td>" + Title[2] + "</td>";
                    body += "<td>" + Title[3] + "</td>";
                    body += "<td>" + Title[4] + "</td>";
                    body += "<td>" + Title[5] + "</td>";
                    body += "<td>" + Title[6] + "</td>";
                    body += "<td>" + Title[7] + "</td>";
                    body += "<td>" + Title[8] + "</td>";
                    body += "<td>" + Title[9] + "</td>";
                    body += "<td>" + Title[10] + "</td>";
                    body += "<td>" + Title[11] + "</td>";
                    body += "<td>" + Title[12] + "</td>";
                    body += "</tr>";
                }
                body += "</table>";

                //Footer
                body += "</hr>";
                body += "<h5 align='center'>";
                body += "zuTTers, İstanbul 2018 ";
                body += "<a href='twitter.com/zkocalioglu'><font color='red'> İletişim </font></a> | Zübeyir Koçalioğlu tarafından gönderildi.";
                body += "</h5>";




                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("simpleterzi3428@outlook.com", "Simple Terzi - Axis");
                mail.To.Add(new MailAddress("simpleterzi3428@outlook.com"));
                mail.Bcc.Add(new MailAddress("zubeyir.kocalioglu@gmail.com", "Zübeyir KOÇALİOĞLU"));
                mail.Subject = "Simple Terzi Rapor - Ay Bazında Miktar Raporu";
                mail.Body = body;
                mail.IsBodyHtml = true;

                ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };

                Shared.AddToDBMail(mail.Body, mail.Subject, mail.From.ToString(), mail.To.ToString());

                smtp.Send(mail);


            }
            catch (Exception ex)
            {
                ex.AddToDBLog("SendOrderWMail", ex.Message);
            }

        }


    }


}
