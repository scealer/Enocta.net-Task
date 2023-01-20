using KTOtomasyon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KTOtomasyon.Controllers
{
    public class CustomerController : Controller
    {
        public int defaultPageSize = 15;

        // GET: Customer
        public ActionResult Index(int? p, string filter)
        {
            DisplayCustomers customer = new DisplayCustomers();

            if (p == null)
                p = 1;

            if (Session["UserId"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                try
                {
                    //Ürünleri alıyoruz
                    using (var db = new KTOtomasyonEntities())
                    {
                        //Filter
                        IQueryable<vCustomers> query = null;
                        if (string.IsNullOrEmpty(filter))
                        {
                            query = db.vCustomers.Where(x => 1 == 1);
                        }
                        else
                        {
                            query = db.vCustomers.Where(x => x.CustomerName.Contains(filter) && (x.PhoneNumber.Contains(filter)));
                        }



                        customer.CustomerList = query.OrderByDescending(x => x.PhoneNumber).Skip(defaultPageSize * (p.Value - 1)).Take(defaultPageSize).ToList();
                        customer.CurrentPage = p.Value;
                        customer.TotalCount = query.Count();
                        if ((customer.TotalCount % defaultPageSize) == 0)
                        {
                            customer.TotalPage = customer.TotalCount / defaultPageSize;
                        }
                        else
                        {
                            customer.TotalPage = (customer.TotalCount / defaultPageSize) + 1;
                        }

                    }
                }
                catch (Exception)
                {

                    RedirectToAction("ErrorPage", "Home");
                }

                return View(customer);
            }

        }
    }
}