using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KTOtomasyon.Models
{
    public class OrderWithDetail
    {
        public int Order_Id { get; set; }
        public string CustomerName { get; set; }
        public string PhoneNumber { get; set; }
        public string Debt { get; set; }
        public string Addition { get; set; }
        public Nullable<int> Discount { get; set; }
        public Nullable<System.DateTime> OrderDate { get; set; }
        public Nullable<int> CreatedUser { get; set; }
        public string CreatedUserText { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<bool> IsPaid { get; set; }
        public Nullable<bool> IsDelivered { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public IList<OrderDetails> OrderDetails { get; set; }


    }

    public class OrderDetails
    {
        public int OrderDetail_Id { get; set; }
        public Nullable<int> Order_Id { get; set; }
        public Nullable<int> Operation_Id { get; set; }
        public string OperationText { get; set; }
        public Nullable<int> Quantity { get; set; }
        public Nullable<decimal> Price { get; set; }
        public Nullable<decimal> TotalPrice { get; set; }
        public Nullable<decimal> TTotalPrice { get; set; }
        public int TQuantity { get; set; }
        public string ProductName { get; set; }

    }

    public class DisplayOrderDetail
    {

        public IList<vOrders> OrdersList { get; set; }
        public int TotalPrice { get; set; }
        public string CreatedUserName { get; set; }

        public int TotalCount { get; set; }
        public int TotalPage { get; set; }
        public int CurrentPage { get; set; }
    }

    public class MailDetail
    {
        public long Sira { get; set; }
        public int? SipMiktar { get; set; }
        public decimal? SipTutar { get; set; }
    }




}