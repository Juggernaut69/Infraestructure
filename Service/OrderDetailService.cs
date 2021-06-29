using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Infraestructure;

namespace Service
{
    public class OrderDetailService
    {

        private OrdersContext db = new OrdersContext();
        public List<OrderDetail> Get()
        {
            List<OrderDetail> orderdetails = null;
            orderdetails = db.OrderDetails.ToList();
            return orderdetails;
        }

        public OrderDetail GetById(int ID)
        {
            OrderDetail orderdetail = null;
            orderdetail = db.OrderDetails.Find(ID);
            return orderdetail;
        }

        public void Insert(OrderDetail orderdetail)
        {
            db.OrderDetails.Add(orderdetail);
            db.SaveChanges();
        }

        public void Update(OrderDetail orderdetail, int ID)
        {
            var newOrderDetail = db.OrderDetails.Find(ID);
            newOrderDetail.Order = orderdetail.Order;
            newOrderDetail.OrderDetailID = orderdetail.OrderDetailID;
            newOrderDetail.OrderID = orderdetail.OrderID;
            newOrderDetail.Price = orderdetail.Price;
            newOrderDetail.Product = orderdetail.Product;
            newOrderDetail.ProductID = orderdetail.ProductID;
            newOrderDetail.Quantity = orderdetail.Quantity;
            db.SaveChanges();
        }

        public void Delete(int ID)
        {
            var orderdetail = db.OrderDetails.Find(ID);
            db.OrderDetails.Remove(orderdetail);
            db.SaveChanges();
        }

    }
}
