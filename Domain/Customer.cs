using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class Customer
    {
        public int CustomerID { get; set; }
        public string FullName { get; set; }
        public int DNI { get; set; }
        public virtual ICollection<Order> Orders { get; set; }

    }

}
