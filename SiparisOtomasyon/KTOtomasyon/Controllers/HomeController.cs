using System;
using System.Transactions;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using KTOtomasyon.Models;


namespace KTOtomasyon.Controllers
{


    public class HomeController : Controller
    {
        //--Anasayfa--
        //Bir sayfadaki kayıt sayısıdır.İptal!!!
        public int defaultPageSize = 15;

        //Tüm siparişlerin listelendiği ekrandır.
        public ActionResult Index(int? p, string filter, string otype, string oname)
        {
            DisplayOrderDetail orders = new DisplayOrderDetail();


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
                    //Siparişleri databeseden alıyoruz
                    using (var db = new KTOtomasyonEntities())
                    {


                        //Filter
                        IQueryable<vOrders> query = null;
                        if (string.IsNullOrEmpty(filter))
                        {
                            query = db.vOrders.Where(x => 1 == 1);
                        }
                        else
                        {
                            query = db.vOrders.Where(x => x.CustomerName.Contains(filter) || x.Order_Id.ToString().Equals(filter) || x.PhoneNumber.ToString().Equals(filter));

                        }
                        //Skip ve Take parametreleri silindi!
                        if (string.IsNullOrEmpty(oname))
                        {
                            orders.OrdersList = query.OrderByDescending(x => x.Order_Id).ToList();
                        }

                        else if (oname == "durum")
                        {
                            if (string.IsNullOrEmpty(otype) || otype == "asc")
                            {
                                orders.OrdersList = query.OrderByDescending(x => x.Order_Id).Where(x => x.IsDelivered == false).ToList();
                            }
                            else if (otype == "desc")
                            {
                                orders.OrdersList = query.OrderByDescending(x => x.Order_Id).Where(x => x.IsDelivered == true).ToList();
                            }
                        }

                    }
                }
                catch (Exception e)
                {
                    e.AddToDBLog("HomeController.Index");
                    RedirectToAction("ErrorPage", "Home");
                }

                ViewBag.oname = oname;
                ViewBag.otype = otype;

                return View(orders);

            }


        }

        //Sipariş kaydet işlemini gerçekleştirir.
        public JsonResult OrderSave(OrderWithDetail orderWithDetail)
        {
            Orders orderadd = new Orders();
            ReturnValue ret = new ReturnValue();

            if (Shared.CheckSession() == false)
            {
                ret.requiredLogin = true;
                ret.message = "Lütfen giriş yapınız.";
                ret.success = false;
                return Json(ret);
            }
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    ret.success = false;

                    //Sipariş id'sine göre kayıtları databaseden çeker
                    using (var db = new KTOtomasyonEntities())
                    {
                        if (orderWithDetail.OrderDetails == null || orderWithDetail.OrderDetails.Count == 0)
                        {
                            ret.requiredLogin = false;
                            ret.message = "Lütfen işlem giriniz.";
                            ret.success = false;
                            return Json(ret);
                        }

                        if (orderWithDetail.Order_Id != 0)
                        {
                            orderadd = db.Orders.Where(x => x.Order_Id == orderWithDetail.Order_Id).FirstOrDefault();
                        }

                        orderadd.CustomerName = orderWithDetail.CustomerName;
                        orderadd.PhoneNumber = orderWithDetail.PhoneNumber;
                        orderadd.Debt = orderWithDetail.Debt;
                        orderadd.Addition = orderWithDetail.Addition;
                        orderadd.Discount = orderWithDetail.Discount;
                        orderadd.OrderDate = orderWithDetail.OrderDate;
                        orderadd.CreatedDate = DateTime.Now;
                        orderadd.CreatedUser = Convert.ToInt32(Session["UserId"]);
                        orderadd.IsPaid = orderWithDetail.IsPaid;
                        orderadd.IsDelivered = orderWithDetail.IsDelivered;
                        orderadd.IsDeleted = orderWithDetail.IsDeleted;

                        if (orderWithDetail.Order_Id == 0)
                        {
                            db.Orders.Add(orderadd);
                        }
                        db.SaveChanges();

                        int id = orderadd.Order_Id;
                        foreach (var item in orderadd.OrderDetail.ToList())
                        {
                            db.OrderDetail.Remove(item);
                        }
                        db.SaveChanges();

                        foreach (var item in orderWithDetail.OrderDetails)
                        {
                            OrderDetail odetail = new OrderDetail();

                            odetail.Order_Id = id;
                            odetail.Operation_Id = item.Operation_Id;
                            odetail.Quantity = item.Quantity;
                            odetail.Price = item.Price;
                            odetail.TotalPrice = item.TotalPrice;

                            db.OrderDetail.Add(odetail);
                        }
                        db.SaveChanges();
                        scope.Complete();
                        ret.retObject = orderWithDetail;
                    }
                    ret.message = "Başarıyla kaydedildi.";
                    ret.success = true;
                }
                catch (Exception ex)
                {
                    ex.AddToDBLog("HomeController.OrderSave");
                    ret.success = false;
                    ret.message = ex.Message;
                    scope.Dispose();
                }
            }
            return Json(ret);
        }

        public JsonResult OrderSave2(OrderWithDetail orderWithDetail)
        {
            Orders orderadd = new Orders();
            ReturnValue ret = new ReturnValue();

            if (Shared.CheckSession() == false)
            {
                ret.requiredLogin = true;
                ret.message = "Lütfen giriş yapınız.";
                ret.success = false;
                return Json(ret);
            }
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    ret.success = false;

                    //Sipariş id'sine göre kayıtları databaseden çeker
                    using (var db = new KTOtomasyonEntities())
                    {
                        if (orderWithDetail.OrderDetails == null || orderWithDetail.OrderDetails.Count == 0)
                        {
                            ret.requiredLogin = false;
                            ret.message = "Lütfen işlem giriniz.";
                            ret.success = false;
                            return Json(ret);
                        }

                        if (orderWithDetail.Order_Id != 0)
                        {
                            orderadd = db.Orders.Where(x => x.Order_Id == orderWithDetail.Order_Id).FirstOrDefault();
                        }

                        orderadd.CustomerName = orderWithDetail.CustomerName;
                        orderadd.PhoneNumber = orderWithDetail.PhoneNumber;
                        orderadd.Debt = orderWithDetail.Debt;
                        orderadd.Addition = orderWithDetail.Addition;
                        orderadd.Discount = orderWithDetail.Discount;
                        orderadd.OrderDate = orderWithDetail.OrderDate;
                        orderadd.CreatedDate = DateTime.Now;
                        orderadd.CreatedUser = Convert.ToInt32(Session["UserId"]);
                        orderadd.IsPaid = orderWithDetail.IsPaid;
                        orderadd.IsDelivered = orderWithDetail.IsDelivered;
                        orderadd.IsDeleted = orderWithDetail.IsDeleted;

                        if (orderWithDetail.Order_Id == 0)
                        {
                            db.Orders.Add(orderadd);
                        }
                        db.SaveChanges();

                        int id = orderadd.Order_Id;
                        foreach (var item in orderadd.OrderDetail.ToList())
                        {
                            db.OrderDetail.Remove(item);
                        }
                        db.SaveChanges();


                        OrderDetail odetail = new OrderDetail();

                        odetail.Order_Id = id;
                        odetail.Operation_Id = orderWithDetail.OrderDetails[0].Operation_Id;
                        odetail.Quantity = orderWithDetail.OrderDetails[0].Quantity;
                        odetail.Price = orderWithDetail.OrderDetails[0].Price;
                        odetail.TotalPrice = orderWithDetail.OrderDetails[0].TotalPrice;

                        db.OrderDetail.Add(odetail);
                        
                        db.SaveChanges();
                        scope.Complete();
                        ret.retObject = orderWithDetail;
                    }
                    ret.message = "Başarıyla kaydedildi.";
                    ret.success = true;
                }
                catch (Exception ex)
                {
                    ex.AddToDBLog("HomeController.OrderSave2");
                    ret.success = false;
                    ret.message = ex.Message;
                    scope.Dispose();
                }
            }
            return Json(ret);
        }


        //Her ürünün tadilatlarının listesini çeker.
        public JsonResult GetOperations(int Product_Id)
        {
            ReturnValue ret = new ReturnValue();

            if (Shared.CheckSession() == false)
            {
                ret.requiredLogin = true;
                ret.message = "Lütfen giriş yapınız.";
                ret.success = false;
                return Json(ret);
            }
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    using (var db = new KTOtomasyonEntities())
                    {

                        var operationdata = db.Operations.Where(x => x.Product_Id == Product_Id).Select(x => new { Name = x.Name, Price = x.Price, Operation_Id = x.Operation_Id }).ToList();

                        if (operationdata == null)
                        {
                            ret.requiredLogin = false;
                            ret.message = "Lütfen işlem giriniz.";
                            ret.success = false;
                            return Json(ret);
                        }

                        ret.retObject = operationdata;
                        ret.message = "Başarıyla kaydedildi.";
                        ret.success = true;
                        scope.Complete();

                        return Json(operationdata);

                    }

                }
                catch (Exception ex)
                {

                    ret.success = false;
                    ret.message = ex.Message;
                    ex.AddToDBLog("HomeController.GetOperations");
                    scope.Dispose();
                }
                return Json(ret);
            }


        }

        //Tıklanılan siparişin detaylarını getirir.
        public JsonResult GetOrderData(int Order_Id)
        {
            OrderWithDetail orderWithDetail = new OrderWithDetail();
            ReturnValue ret = new ReturnValue();

            if (Shared.CheckSession() == false)
            {
                ret.requiredLogin = true;
                ret.message = "Lütfen giriş yapınız.";
                ret.success = false;
                return Json(ret);
            }
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    using (var db = new KTOtomasyonEntities())
                    {
                        var orderdata = db.Orders.Where(x => x.Order_Id == Order_Id).FirstOrDefault();

                        orderWithDetail.Order_Id = orderdata.Order_Id;
                        orderWithDetail.CreatedUser = orderdata.CreatedUser;
                        orderWithDetail.CreatedUserText = orderdata.Users.Name;
                        orderWithDetail.CustomerName = orderdata.CustomerName;
                        orderWithDetail.PhoneNumber = orderdata.PhoneNumber;
                        orderWithDetail.Debt = orderdata.Debt;
                        orderWithDetail.Discount = orderdata.Discount;
                        orderWithDetail.Addition = orderdata.Addition;
                        orderWithDetail.OrderDate = orderdata.OrderDate;
                        orderWithDetail.CreatedDate = orderdata.CreatedDate;
                        orderWithDetail.IsPaid = orderdata.IsPaid;
                        orderWithDetail.IsDelivered = orderdata.IsDelivered;
                        orderWithDetail.IsDeleted = orderdata.IsDeleted;

                        orderWithDetail.OrderDetails = new List<OrderDetails>();

                        foreach (var item in orderdata.OrderDetail)
                        {
                            orderWithDetail.OrderDetails.Add(new OrderDetails
                            {
                                Order_Id = item.Order_Id,
                                Operation_Id = item.Operation_Id,
                                Quantity = item.Quantity,
                                Price = item.Price,
                                TotalPrice = item.TotalPrice,
                                OrderDetail_Id = item.OrderDetail_Id,
                                OperationText = item.Operations.Name

                            });
                        }
                        scope.Complete();
                        return Json(orderWithDetail);

                    }
                }
                catch (Exception ex)
                {
                    ex.AddToDBLog("HomeController.GetOrderData");
                    ret.success = false;
                    ret.message = ex.Message;
                    scope.Dispose();
                }
            }

            return Json(ret);
        }

        //Telefon noya göre müşteri bilgisi çeker.
        public JsonResult GetPhoneData(string PNumber)
        {
            OrderWithDetail orderWithDetail = new OrderWithDetail();
            ReturnValue ret = new ReturnValue();

            if (Shared.CheckSession() == false)
            {
                ret.requiredLogin = true;
                ret.message = "Lütfen giriş yapınız.";
                ret.success = false;
                return Json(ret);
            }
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    using (var db = new KTOtomasyonEntities())
                    {
                        var phonedata = db.Orders.Where(x => x.PhoneNumber == PNumber).FirstOrDefault();

                        if (phonedata != null)
                        {
                            orderWithDetail.Order_Id = phonedata.Order_Id;
                            orderWithDetail.CustomerName = phonedata.CustomerName;
                            orderWithDetail.PhoneNumber = phonedata.PhoneNumber;
                            orderWithDetail.Debt = phonedata.Debt;
                            orderWithDetail.Discount = phonedata.Discount;
                            orderWithDetail.OrderDate = DateTime.Now;

                            ret.message = "Müşteri Bulundu.";
                            ret.success = true;

                            ret.retObject = orderWithDetail;
                            scope.Complete();
                        }
                        else
                        {
                            ret.message = "Müşteri Bulunamadı.";
                            ret.success = false;
                        }
                    }

                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    ret.success = false;
                    ret.message = ex.Message;
                    ex.AddToDBLog("HomeController.GetPhoneData");
                }
            }
            return Json(ret);
        }
      
        public ActionResult OrderDelete(int Order_Id)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    using (var db = new KTOtomasyonEntities())
                    {
                        Orders orderdelete = null;
                        var orderadd = db.Orders.Where(x => x.Order_Id == Order_Id).FirstOrDefault();

                        orderdelete = db.Orders.Where(x => x.Order_Id == Order_Id).FirstOrDefault();
                        orderdelete.CreatedUser = orderadd.CreatedUser;
                        orderdelete.CustomerName = orderadd.CustomerName;
                        orderdelete.PhoneNumber = orderadd.PhoneNumber;
                        orderdelete.Debt = orderadd.Debt;
                        orderdelete.Discount = orderadd.Discount;
                        orderdelete.Addition = orderadd.Addition;
                        orderdelete.OrderDate = orderadd.OrderDate;
                        orderdelete.CreatedDate = orderadd.CreatedDate;
                        orderdelete.IsPaid = orderadd.IsPaid;
                        orderdelete.IsDelivered = orderadd.IsDelivered;
                        orderdelete.IsDeleted = true;

                        db.SaveChanges();
                        scope.Complete();
                    }

                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    ex.AddToDBLog("HomeController.OrderDelete",ex.Message);
                }
            }

            return RedirectToAction("Index", "Home");
        }
    

        //Makbuz görüntülenme ekranıdır.
        public ActionResult Receipt(int Order_Id)
        {
            DisplayReceipt orderreceipt = new DisplayReceipt();



            if (Session["UserId"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                try
                {
                    using (var db = new KTOtomasyonEntities())
                    {
                        var orderdata = db.Orders.Where(x => x.Order_Id == Order_Id).FirstOrDefault();

                        orderreceipt.Order_Id = orderdata.Order_Id;
                        orderreceipt.CreatedUser = orderdata.CreatedUser;
                        orderreceipt.CreatedUserText = orderdata.Users.Name;
                        orderreceipt.CustomerName = orderdata.CustomerName;
                        orderreceipt.PhoneNumber = orderdata.PhoneNumber;
                        orderreceipt.Debt = orderdata.Debt;
                        orderreceipt.Addition = orderdata.Addition;
                        //orderreceipt.Discount = orderdata.Discount;
                        orderreceipt.OrderDate = orderdata.OrderDate;
                        orderreceipt.CreatedDate = orderdata.CreatedDate;
                        orderreceipt.TTotalPrice = Convert.ToInt32(orderdata.OrderDetail.Sum(x => x.TotalPrice).Value);
                        orderreceipt.NTotalPrice = Convert.ToInt32(orderdata.OrderDetail.Sum(x => x.TotalPrice).Value) - ((Convert.ToInt32(orderdata.OrderDetail.Sum(x => x.TotalPrice).Value) * Convert.ToInt32(orderdata.Discount)/100));
                        orderreceipt.TQuantity = orderdata.OrderDetail.Sum(x => x.Quantity).Value;
                        orderreceipt.DetailList = new List<OrderDetails>();
                        orderreceipt.IsPaid = orderdata.IsPaid.Value;

                        foreach (var item in orderdata.OrderDetail.ToList())
                        {
                            orderreceipt.DetailList.Add(new OrderDetails
                            {
                                Operation_Id = item.Operation_Id,
                                Quantity = item.Quantity,
                                Price = Convert.ToInt32(item.Price),
                                TotalPrice = Convert.ToInt32(item.TotalPrice),
                                OrderDetail_Id = item.OrderDetail_Id,
                                OperationText = item.Operations.Name,
                                ProductName = item.Operations.Products.Name

                            });
                        }

                    }
                }
                catch (Exception ex)
                {
                    ex.AddToDBLog("HomeController.Receipt", ex.Message);
                    RedirectToAction("ErrorPage", "Home");
                }
            }

            return PartialView(orderreceipt);
        }

        //--Kullanıcılar--
        public ActionResult Users(int? p, string filter)
        {
            DisplayUsers user = new DisplayUsers();


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
                    //Kullanıcıları alıyoruz
                    using (var db = new KTOtomasyonEntities())
                    {
                        //Filter
                        IQueryable<Users> query = null;

                        if (string.IsNullOrEmpty(filter))
                        {
                            query = db.Users.Where(x => 1 == 1 && x.IsDeleted == false);
                        }
                        else
                        {
                            query = db.Users.Where(x => x.IsDeleted == false && (x.Name.Contains(filter)));
                        }

                        user.UsersList = query.OrderByDescending(x => x.User_Id).Skip(defaultPageSize * (p.Value - 1)).Take(defaultPageSize).ToList();

                        user.CurrentPage = p.Value;
                        user.TotalCount = query.Count();
                        if ((user.TotalCount % defaultPageSize) == 0)
                        {
                            user.TotalPage = user.TotalCount / defaultPageSize;
                        }
                        else
                        {
                            user.TotalPage = (user.TotalCount / defaultPageSize) + 1;
                        }

                    }
                }
                catch (Exception ex)
                {
                    ex.AddToDBLog("HomeController.Users", ex.Message);
                    RedirectToAction("ErrorPage", "Home");
                }

                return View(user);
            }

        }

        public ActionResult UserAdder(Users user)
        {
            Users useradd = new Users();
            using (var db = new KTOtomasyonEntities())
            {

                useradd.Name = user.Name;
                useradd.Gender = user.Gender;
                useradd.Mail = user.Mail;
                useradd.Password = user.Name;
                useradd.IsDeleted = false;
                useradd.UserType = 1;

                db.Users.Add(useradd);
                db.SaveChanges();
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult UserDetail(int id)
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

        public ActionResult UserEdit(int id)
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                Users useredit = null;

                using (var db = new KTOtomasyonEntities())
                {
                    useredit = db.Users.Where(d => d.User_Id == id).First();
                }
                return View(useredit);
            }
        }

        public ActionResult UserUpdate(Users user)
        {
            Users userupdate = null;
            try
            {
                using (var db = new KTOtomasyonEntities())
                {
                    userupdate = db.Users.Where(d => d.User_Id == user.User_Id).First();
                    userupdate.Name = user.Name;
                    userupdate.Gender = user.Gender;
                    userupdate.Birthday = user.Birthday;
                    userupdate.Mail = user.Mail;
                    userupdate.Password = user.Password;
                    userupdate.UserType = 1;
                    userupdate.IsDeleted = false;
                    userupdate.IsActive = true;

                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                ex.AddToDBLog("HomeController.UserUpdate", "Sıkıntı Büyük");
            }

            return RedirectToAction("UserDetail", new { id = user.User_Id });

        }

        public ActionResult UserDelete(Users user)
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                Users userdelete = null;

                using (var db = new KTOtomasyonEntities())
                {
                    userdelete = db.Users.Where(d => d.User_Id == user.User_Id).First();
                    userdelete.IsDeleted = true;

                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
        }


        //--Tadilatlar--
        public ActionResult Operations(int? p, string filter, string pfilter)
        {
            DisplayOperations operation = new DisplayOperations();

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
                    //Tadilatları alıyoruz
                    using (var db = new KTOtomasyonEntities())
                    {
                        //Filter
                        IQueryable<Operations> query = null;
                        if (string.IsNullOrEmpty(filter))
                        {
                            query = db.Operations.Where(x => x.Product_Id.ToString() == filter && x.IsActive == true);
                        }
                        else
                        {
                            query = db.Operations.Where(x => x.IsActive == true && (x.Product_Id.ToString().Equals(filter)));
                        }

                        operation.OperationList = query.OrderByDescending(x => x.Operation_Id).Skip(defaultPageSize * (p.Value - 1)).Take(defaultPageSize).ToList();
                        operation.ProductName = db.Products.Where(x => x.Product_Id.ToString() == filter).FirstOrDefault().Name;

                        operation.CurrentPage = p.Value;
                        operation.TotalCount = query.Count();
                        if ((operation.TotalCount % defaultPageSize) == 0)
                        {
                            operation.TotalPage = operation.TotalCount / defaultPageSize;
                        }
                        else
                        {
                            operation.TotalPage = (operation.TotalCount / defaultPageSize) + 1;
                        }

                    }
                }
                catch (Exception)
                {

                    RedirectToAction("ErrorPage", "Home");
                }

                return View(operation);
            }

        }

        public ActionResult NewOperation()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                try
                {
                    using (var db = new KTOtomasyonEntities())
                    {
                        ViewBag.VBProducts = db.Products.ToList();
                    }
                }
                catch (Exception e)
                {
                    e.AddToDBLog("Newoperation");
                }

            }
            return View();
        }

        public ActionResult OperationAdder(Operations operation)
        {
            Operations op = new Operations();

            using (var db = new KTOtomasyonEntities())
            {
                op.Product_Id = operation.Product_Id;
                op.Name = operation.Name;
                op.Price = operation.Price;
                op.IsActive = true;

                db.Operations.Add(op);
                db.SaveChanges();
            }

            return RedirectToAction("Operations", "Home");
        }

        public ActionResult OperationEdit(int id)
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                Operations op = null;

                using (var db = new KTOtomasyonEntities())
                {
                    ViewBag.VBProducts = db.Products.ToList();
                    op = db.Operations.Where(d => d.Operation_Id == id).First();
                }
                return View(op);
            }
        }

        public ActionResult OperationUpdate(Operations operation)
        {
            Operations op = null;

            using (var db = new KTOtomasyonEntities())
            {
                op = db.Operations.Where(x => x.Operation_Id == operation.Operation_Id).First();
                op.Product_Id = operation.Product_Id;
                op.Name = operation.Name;
                op.Price = operation.Price;
                op.IsActive = true;

                db.SaveChanges();
            }

            return RedirectToAction("NewOperation", "Home");
        }

        //--Ürünler--
        public ActionResult Products(int? p, string filter)
        {
            DisplayProducts product = new DisplayProducts();

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
                        IQueryable<Products> query = null;
                        if (string.IsNullOrEmpty(filter))
                        {
                            query = db.Products.Where(x => 1 == 1 && x.IsActive == true);
                        }
                        else
                        {
                            query = db.Products.Where(x => x.IsActive == true && (x.Name.Contains(filter)));
                        }



                        product.ProductList = query.OrderByDescending(x => x.Product_Id).Skip(defaultPageSize * (p.Value - 1)).Take(defaultPageSize).ToList();
                        product.CurrentPage = p.Value;
                        product.TotalCount = query.Count();
                        if ((product.TotalCount % defaultPageSize) == 0)
                        {
                            product.TotalPage = product.TotalCount / defaultPageSize;
                        }
                        else
                        {
                            product.TotalPage = (product.TotalCount / defaultPageSize) + 1;
                        }

                    }
                }
                catch (Exception)
                {

                    RedirectToAction("ErrorPage", "Home");
                }

                return View(product);
            }

        }

        public ActionResult NewProduct()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ProductAdder(Products product)
        {
            Products newproduct = new Products();

            using (var db = new KTOtomasyonEntities())
            {



                if (Request.Files.Count > 0)
                {
                    var file = Request.Files[0];

                    if (file != null && file.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        var path = Path.Combine(Server.MapPath("~/img/producticons/"), fileName);
                        file.SaveAs(path);
                    }
                }

                newproduct.Name = product.Name;
                newproduct.PhotoUrl = product.PhotoUrl;
                newproduct.IsActive = true;


                db.Products.Add(newproduct);
                db.SaveChanges();
            }
            return RedirectToAction("Index", "Home");
        }

        //--Harici Sayfalar
        public ActionResult BackUp()
        {

            Shared.BackupDB();
            string path = "C:\\backupdb";
            var getfilesvm = GetFilesFromDirectory(path);

            return View(getfilesvm);
        }

        public ActionResult AboutMe()
        {
            return View();
        }

        public ActionResult ErrorPage()
        {
            return View();
        }

        //--Harici Methodlar--
        private List<FilesViewModel> GetFilesFromDirectory(string DirPath)
        {

            List<FilesViewModel> dosyavm = new List<FilesViewModel>();

            try
            {
                DirectoryInfo Dir = new DirectoryInfo(DirPath);
                FileInfo[] FileList = Dir.GetFiles("*.*", SearchOption.AllDirectories);
                foreach (FileInfo FI in FileList)
                {
                    dosyavm.Add(new FilesViewModel { FileName = FI.FullName });

                }
            }
            catch (Exception e)
            {
                e.AddToDBLog("HomeController.GetFilesFromDirectory");
            }
            return dosyavm;
        }       

        public ActionResult Reports()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                try
                {
                    //Shared.DefaultSendMail();
                    //DefaultSendMail();
                }
                catch (Exception ex)
                {

                    ex.AddToDBLog("Reports");
                }
            }

            return View();
        }

        public JsonResult GetDateReport(DateTime? FirstOrderDate, DateTime? LastOrderDate)
        {
            using (var db = new KTOtomasyonEntities())
            {
                var toplam = db.ToplaTutar(FirstOrderDate, LastOrderDate).First();
                return Json(toplam);
            }
        }

        public JsonResult GetMountlyReport()
        {

            using (var db = new KTOtomasyonEntities())
            {
                var result = db.AylikSiparisTutar().ToList();
                return Json(result);
            }
        }

        public JsonResult DefaultSendMail()
        {
            ReturnValue retVal = new ReturnValue();

            try
            {
                retVal.success = false;

                SmtpClient smtp = new SmtpClient("smtp-mail.outlook.com", 587); //587
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential("simpleterzi3428@outlook.com", "3428simple");

                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("simpleterzi3428@outlook.com", "Simple Terzi - Axis");
                mail.To.Add(new MailAddress("simpleterzi3428@outlook.com"));
                //mail.CC.Add(new MailAddress("zubeyir_kocalioglu@outlook.com"));
                mail.Bcc.Add(new MailAddress("zubeyir.kocalioglu@gmail.com", "Zübeyir KOÇALİOĞLU"));

                Mails addmail = new Mails();

                using (var db = new KTOtomasyonEntities())
                {

                    var Data = db.vTodayTotalOrder.OrderByDescending(x => x.Sira).ToList();
                    var ThisMessageBody = Data.First();


                    mail.Subject = "Simple Terzi Sipariş Rapor";
                    mail.Body = "Bugün : Toplam sipariş miktarı '";
                    mail.Body += ThisMessageBody.SipMiktar + "' ve sipariş tutarı '" + ThisMessageBody.SipTutar + "'₺ dir.";

                    addmail.CreatedDate = DateTime.Now;
                    addmail.MailTo = "simpleterzi3428@outlook.com";
                    addmail.MailBCC = "zubeyir.kocalioglu@gmail.com";
                    addmail.MailBody = "Bugün : Toplam sipariş miktarı '";
                    addmail.MailBody += ThisMessageBody.SipMiktar + "' ve sipariş tutarı '" + ThisMessageBody.SipTutar + "'₺ dir.";
                    addmail.MailSubject = "Simple Terzi Sipariş Rapor";
                    addmail.SendDate = DateTime.Now;
                    addmail.IsSend = true;

                    db.Mails.Add(addmail);
                    db.SaveChanges();
                }



                smtp.Send(mail);

                retVal.success = true;
                retVal.message = "mail gönderildi";

            }
            catch (Exception ex)
            {
                ex.AddToDBLog("SendMail", ex.Message);
                retVal.message = "mail gönderilemedi";
                retVal.error = ex.Message;
                retVal.success = true;
            }

            return Json(retVal);
        }


    }
}
