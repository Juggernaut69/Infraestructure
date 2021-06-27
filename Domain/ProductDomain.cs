using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class ProductDomain
    {
        [Key]
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public double Price { get; set; }

    }
}
