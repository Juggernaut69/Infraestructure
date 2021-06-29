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
using Service;
using API.Http;

namespace API.Controllers
{
    public class OrdersController : ApiController
    {
        OrderDetailService ServiceOrderDetail;
        ProductService ServiceProduct;
        OrderService ServiceOrder;
        CustomerService ServiceCustomer;

        public OrdersController()
        {
            ServiceOrderDetail = new OrderDetailService();
            ServiceProduct = new ProductService();
            ServiceOrder = new OrderService();
            ServiceCustomer = new CustomerService();
        }

        // GET: api/Orders/GetOrders
        public JsonResult<List<OrderModel>> GetOrders()
        {
            EntityMapper<Order, OrderModel> mapObj = new EntityMapper<Order, OrderModel>();
            List<Order> ordersData = ServiceOrder.Get();
            List<OrderModel> orders = new List<OrderModel>();
            foreach (var item in ordersData)
            {
                orders.Add(mapObj.Translate(item));
            }
            return Json(orders);
        }

        // GET: api/Orders/GetFilteredOrders
        public JsonResult<List<FilteredOrderResponse>> GetFilteredOrders()
        {
            EntityMapper<Order, OrderModel> mapObjOrder = new EntityMapper<Order, OrderModel>();
            EntityMapper<Customer, CustomerModel> mapObjCustomer = new EntityMapper<Customer, CustomerModel>();
            EntityMapper<OrderDetail, OrderDetailModel> mapObjOrderDetail = new EntityMapper<OrderDetail, OrderDetailModel>();

            List<Order> ordersData = ServiceOrder.Get();
            List<FilteredOrderResponse> filteredOrders = new List<FilteredOrderResponse>();

            foreach (var order in ordersData)
            {
                FilteredOrderResponse filteredOrder = new FilteredOrderResponse();
                filteredOrder.Active = order.Active;
                filteredOrder.OrderID = order.OrderID;
                filteredOrder.OrderDate = order.OrderDate;

                Customer customerData = ServiceCustomer.GetById(order.CustomerID);
                CustomerModel customer = new CustomerModel();
                customer = mapObjCustomer.Translate(customerData);
                filteredOrder.Customer = customer;

                List<OrderDetail> orderDetailsData = ServiceOrderDetail.Get().FindAll((x) => x.OrderID == order.OrderID);
                List<OrderDetailModel> ordersDetail = new List<OrderDetailModel>();
                foreach (var item in orderDetailsData)
                {
                    ordersDetail.Add(mapObjOrderDetail.Translate(item));
                    filteredOrder.OrdersDetail = ordersDetail;
                }
                filteredOrders.Add(filteredOrder);

            }
            return Json<List<FilteredOrderResponse>>(filteredOrders);
        }

        // POST: api/Orders/PostFilteredOrders
        [ResponseType(typeof(OrderModel))]
        public JsonResult<List<FilteredOrderResponse>> PostFilteredOrders(FilterParams param )
        {

            EntityMapper<Order, OrderModel> mapObjOrder = new EntityMapper<Order, OrderModel>();
            EntityMapper<Customer, CustomerModel> mapObjCustomer = new EntityMapper<Customer, CustomerModel>();
            EntityMapper<OrderDetail, OrderDetailModel> mapObjOrderDetail = new EntityMapper<OrderDetail, OrderDetailModel>();

            List<Order> ordersData = ServiceOrder.Get();
            List<FilteredOrderResponse> filteredOrders = new List<FilteredOrderResponse>();

            foreach (var order in ordersData)
            {
                FilteredOrderResponse filteredOrder = new FilteredOrderResponse();
                filteredOrder.Active = order.Active;
                filteredOrder.OrderID = order.OrderID;
                filteredOrder.OrderDate = order.OrderDate;

                Customer customerData = ServiceCustomer.GetById(order.CustomerID);
                CustomerModel customer = new CustomerModel();
                customer = mapObjCustomer.Translate(customerData);
                filteredOrder.Customer = customer;

                List<OrderDetail> orderDetailsData = ServiceOrderDetail.Get().FindAll((x) => x.OrderID == order.OrderID);
                List<OrderDetailModel> ordersDetail = new List<OrderDetailModel>();
                foreach (var item in orderDetailsData)
                {
                    ordersDetail.Add(mapObjOrderDetail.Translate(item));
                    filteredOrder.OrdersDetail = ordersDetail;
                }
                filteredOrders.Add(filteredOrder);
            }
            List<FilteredOrderResponse> fltOrder = filteredOrders.Where(x => (x.OrderID == param.OrderID || param.OrderID == 0) && (x.Customer.CustomerID == param.ClientID || param.ClientID == 0) && (x.OrderDate > param.StartDate && x.OrderDate < param.EndDate || param.StartDate == null || param.EndDate == null)).ToList();
            return Json<List<FilteredOrderResponse>>(fltOrder);
        }

        // GET: api/Orders/GetOrder/5
        [ResponseType(typeof(OrderModel))]
        public JsonResult<OrderModel> GetOrder(int id)
        {
            try
            {
                EntityMapper<Order, OrderModel> mapObj = new EntityMapper<Order, OrderModel>();
                Order orderData = ServiceOrder.GetById(id);
                OrderModel order = new OrderModel();
                order = mapObj.Translate(orderData);
                return Json(order);
            }
            catch (Exception)
            {
                return null;
            }
        }

        // PUT: api/Orders/PutOrder/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutOrder(int id, OrderModel orderData)
        {

            EntityMapper<OrderModel, Order> mapObj = new EntityMapper<OrderModel, Order>();
            Order order = new Order();
            try
            {
                order = mapObj.Translate(orderData);
                ServiceOrder.Update(order, id);
            }
            catch (Exception)
            {
                return null;
            }

            return StatusCode(HttpStatusCode.OK);
        }

        // POST: api/Orders/PostOrder
        [ResponseType(typeof(OrderModel))]
        public IHttpActionResult PostOrder(OrderModel orderData)
        {
            EntityMapper<OrderModel, Order> mapObj = new EntityMapper<OrderModel, Order>();
            Order order = new Order();

            try
            {
                order = mapObj.Translate(orderData);
                ServiceOrder.Insert(order);
            }
            catch (Exception)
            {
                return null;
            }
      
            return CreatedAtRoute("DefaultApi", new { id = order.OrderID }, order);
        }

        // DELETE: api/Orders/5
        [ResponseType(typeof(OrderModel))]
        public async Task<IHttpActionResult> DeleteOrder(int id)
        {
            Order order = ServiceOrder.GetById(id);
            if (order == null)
            {
                return NotFound();
            }
            order.Active = false;
            ServiceOrder.Update(order, id);

            return Ok(order);
        }

    }
}