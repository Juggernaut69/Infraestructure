using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using API.Models;

namespace API.Http
{
    public class SpecificOrderResponse
    {
        public int OrderDetailID { get; set; }
        public int OrderID { get; set; }
        public ProductModel Product { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }

    }
}