using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class SpecificOrderDomain
    {
        public int OrderDetailID { get; set; }
        public int OrderID { get; set; }
        public ProductDomain Product { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }

    }
}
