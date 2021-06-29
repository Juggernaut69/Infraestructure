using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Infraestructure;

namespace Service
{
    public class OrderService
    {

        private OrdersContext db = new OrdersContext();
        public List<Order> Get()
        {
            List<Order> orders = null;
            orders = db.Orders.ToList();
            return orders;
        }

        public Order GetById(int ID)
        {
            Order order = null;
            order = db.Orders.Find(ID);
            return order;
        }

        public void Insert(Order order)
        {
            db.Orders.Add(order);
            db.SaveChanges();
        }

        public void Update(Order order, int ID)
        {
            var newOrder = db.Orders.Find(ID);
            newOrder.Active = order.Active;
            newOrder.Customer = order.Customer;
            newOrder.CustomerID = order.CustomerID;
            newOrder.OrderDate = order.OrderDate;
            newOrder.OrderDetails = order.OrderDetails;
            newOrder.OrderID = order.OrderID;
            db.SaveChanges();
        }

        public void Delete(int ID)
        {
            var order = db.Orders.Find(ID);
            db.Orders.Remove(order);
            db.SaveChanges();
        }

    }
}
