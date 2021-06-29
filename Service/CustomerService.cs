using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Infraestructure;

namespace Service
{
    public class CustomerService
    {
        private OrdersContext db = new OrdersContext();
        public List<Customer> Get()
        {
            List<Customer> customers = null;
            customers = db.Customers.ToList();
            return customers;
        }

        public Customer GetById(int ID)
        {
            Customer customer = null;
            customer = db.Customers.Find(ID);
            return customer;
        }

        public void Insert(Customer customer)
        {
            db.Customers.Add(customer);
            db.SaveChanges();
        }

        public void Update(Customer customer, int ID)
        {
            var newCustomer = db.Customers.Find(ID);
            newCustomer.CustomerID = customer.CustomerID;
            newCustomer.DNI = customer.DNI;
            newCustomer.FullName = customer.FullName;
            newCustomer.Orders = customer.Orders;
            db.SaveChanges();
        }

        public void Delete(int ID)
        {
            var customer = db.Customers.Find(ID);
            db.Customers.Remove(customer);
            db.SaveChanges();
        }

    }
}
