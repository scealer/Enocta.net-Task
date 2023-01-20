using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KTOtomasyon.Models
{
    public class DisplayReceipt
    {
        public int Order_Id { get; set; }
        public string CustomerName { get; set; }      
        public string PhoneNumber { get; set; }
        public string Debt { get; set; }
        public string Addition { get; set; }
        public Nullable<System.DateTime> OrderDate { get; set; }
        public Nullable<int> CreatedUser { get; set; }
        public string CreatedUserText { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string ProductName { get; set; }
        public decimal TTotalPrice { get; set; }
        public decimal NTotalPrice { get; set; }
        public int TQuantity { get; set; }
        public bool IsPaid { get; set; }
        public List<OrderDetails> DetailList { get; set; }
        public Nullable<int> Discount { get; set; }

    }
}