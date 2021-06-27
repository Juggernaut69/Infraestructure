using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using API.Models;
using Domain;

namespace API
{
    public class OrderContext : DbContext
    {   
        public OrderContext() : base("name = MyContextDB")
        {

        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Product> Products { get; set; }

    }

}