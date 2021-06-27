using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class FilteredOrderDomain
    {

        [Key]
        public int OrderID { get; set; }
        public CustomerDomain Customer { get; set; }
        public DateTime OrderDate { get; set; }
        public bool Active { get; set; }
        public List<OrderDetailDomain> OrdersDetail { get; set; }

    }
}
