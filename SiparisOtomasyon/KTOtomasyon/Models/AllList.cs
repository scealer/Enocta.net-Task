using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KTOtomasyon.Models
{
    public class AllList
    {
        public IList<Users> Users { get; set; }
        public IList<Orders> Orders { get; set; }
        public IList<OrderDetail> OrderDetails { get; set; }
        public IList<Products> Products { get; set; }
        public IList<Operations> Operations { get; set; }

        public int TotalCount { get; set; }
        public int TotalPage { get; set; }
        public int CurrentPage { get; set; }
    }
}