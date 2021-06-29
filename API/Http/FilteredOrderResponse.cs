using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using API.Models;

namespace API.Http
{
    public class FilteredOrderResponse
    {
        public int OrderID { get; set; }
        public CustomerModel Customer { get; set; }
        public DateTime OrderDate { get; set; }
        public bool Active { get; set; }
        public List<OrderDetailModel> OrdersDetail { get; set; }

    }
}