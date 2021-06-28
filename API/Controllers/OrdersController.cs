using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Http.Description;
using API;
using API.Models;
using API.Repository;
using Domain;

namespace API.Controllers
{
    public class OrdersController : ApiController
    {
        private OrderContext db = new OrderContext();

        // GET: api/Orders/GetOrders
        public JsonResult<List<OrderDomain>> GetOrders()
        {
            EntityMapper<Order, OrderDomain> mapObj = new EntityMapper<Order, OrderDomain>();
            List<Order> ordersData = db.Orders.ToList();
            List<OrderDomain> orders = new List<OrderDomain>();
            foreach (var item in ordersData)
            {
                orders.Add(mapObj.Translate(item));
            } 
            return Json<List<OrderDomain>>(orders);
        }

        // GET: api/Orders/GetFilteredOrders
        public async Task<JsonResult<List<FilteredOrderDomain>>> GetFilteredOrders()
        {
            EntityMapper<Order, OrderDomain> mapObjOrder = new EntityMapper<Order, OrderDomain>();
            EntityMapper<Customer, CustomerDomain> mapObjCustomer = new EntityMapper<Customer, CustomerDomain>();
            EntityMapper<OrderDetail, OrderDetailDomain> mapObjOrderDetail = new EntityMapper<OrderDetail, OrderDetailDomain>();

            List<Order> ordersData = db.Orders.ToList();
            List<FilteredOrderDomain> filteredOrders = new List<FilteredOrderDomain>();

            foreach (var order in ordersData)
            {
                FilteredOrderDomain filteredOrder = new FilteredOrderDomain();
                filteredOrder.Active = order.Active;
                filteredOrder.OrderID = order.OrderID;
                filteredOrder.OrderDate = order.OrderDate;

                Customer customerData = await db.Customers.FindAsync(order.CustomerID);
                CustomerDomain customer = new CustomerDomain();
                customer = mapObjCustomer.Translate(customerData);
                filteredOrder.Customer = customer;

                List<OrderDetail> orderDetailsData = db.OrderDetails.ToList().FindAll(x => x.OrderID == order.OrderID);
                List<OrderDetailDomain> ordersDetail = new List<OrderDetailDomain>();
                foreach (var item in orderDetailsData)
                {
                    ordersDetail.Add(mapObjOrderDetail.Translate(item));
                    filteredOrder.OrdersDetail = ordersDetail;
                }
                filteredOrders.Add(filteredOrder);

            }
            return Json<List<FilteredOrderDomain>>(filteredOrders);
        }

        // POST: api/Orders/PostFilteredOrders
        [ResponseType(typeof(Order))]
        public async Task<JsonResult<List<FilteredOrderDomain>>> PostFilteredOrders( FilterParams param )
        {

            EntityMapper<Order, OrderDomain> mapObjOrder = new EntityMapper<Order, OrderDomain>();
            EntityMapper<Customer, CustomerDomain> mapObjCustomer = new EntityMapper<Customer, CustomerDomain>();
            EntityMapper<OrderDetail, OrderDetailDomain> mapObjOrderDetail = new EntityMapper<OrderDetail, OrderDetailDomain>();

            List<Order> ordersData = db.Orders.ToList();
            List<FilteredOrderDomain> filteredOrders = new List<FilteredOrderDomain>();

            foreach (var order in ordersData)
            {
                FilteredOrderDomain filteredOrder = new FilteredOrderDomain();
                filteredOrder.Active = order.Active;
                filteredOrder.OrderID = order.OrderID;
                filteredOrder.OrderDate = order.OrderDate;

                Customer customerData = await db.Customers.FindAsync(order.CustomerID);
                CustomerDomain customer = new CustomerDomain();
                customer = mapObjCustomer.Translate(customerData);
                filteredOrder.Customer = customer;

                List<OrderDetail> orderDetailsData = db.OrderDetails.ToList().FindAll(x => x.OrderID == order.OrderID);
                List<OrderDetailDomain> ordersDetail = new List<OrderDetailDomain>();
                foreach (var item in orderDetailsData)
                {
                    ordersDetail.Add(mapObjOrderDetail.Translate(item));
                    filteredOrder.OrdersDetail = ordersDetail;
                }
                filteredOrders.Add(filteredOrder);
            }
            List<FilteredOrderDomain> fltOrder = (List<FilteredOrderDomain>)filteredOrders.FindAll(x => (x.OrderID == param.OrderID || param.OrderID == 0) && (x.Customer.CustomerID == param.ClientID || param.ClientID == 0));
            return Json<List<FilteredOrderDomain>>(fltOrder);
        }






        // GET: api/Orders/GetOrder/5
        [ResponseType(typeof(Order))]
        public async Task<JsonResult<OrderDomain>> GetOrder(int id)
        {
            try
            {
                EntityMapper<Order, OrderDomain> mapObj = new EntityMapper<Order, OrderDomain>();
                Order orderData = await db.Orders.FindAsync(id);
                OrderDomain order = new OrderDomain();
                order = mapObj.Translate(orderData);
                return Json<OrderDomain>(order);
            }
            catch (Exception)
            {
                return null;
            }
        }

        // PUT: api/Orders/PutOrder/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutOrder(int id, OrderDomain orderData)
        {

            EntityMapper<OrderDomain, Order> mapObj = new EntityMapper<OrderDomain, Order>();
            Order order = new Order();
            try
            {
                order = mapObj.Translate(orderData);
            }
            catch (Exception)
            {
                return null;
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != order.OrderID)
            {
                return BadRequest();
            }

            db.Entry(order).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.OK);
        }

        // POST: api/Orders/PostOrder
        [ResponseType(typeof(Order))]
        public async Task<IHttpActionResult> PostOrder(OrderDomain orderData)
        {
            EntityMapper<OrderDomain, Order> mapObj = new EntityMapper<OrderDomain, Order>();
            Order order = new Order();

            try
            {
                order = mapObj.Translate(orderData);
            }
            catch (Exception)
            {
                return null;
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Orders.Add(order);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = order.OrderID }, order);
        }

        // DELETE: api/Orders/5
        [ResponseType(typeof(Order))]
        public async Task<IHttpActionResult> DeleteOrder(int id)
        {
            Order order = await db.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            db.Orders.Remove(order);
            await db.SaveChangesAsync();

            return Ok(order);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool OrderExists(int id)
        {
            return db.Orders.Count(e => e.OrderID == id) > 0;
        }
    }
}