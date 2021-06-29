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
using System.Web.Http.Description;
using API;
using API.Models;
using System.Web.Http.Results;
using API.Repository;
using Domain;
using Service;
using API.Http;

namespace API.Controllers
{
    public class OrderDetailsController : ApiController
    {
        OrderDetailService ServiceOrderDetail;
        ProductService ServiceProduct;

        public OrderDetailsController()
        {
            ServiceOrderDetail = new OrderDetailService();
            ServiceProduct = new ProductService();
        }

        // GET: api/OrderDetails/GetOrderDetails
        public JsonResult<List<OrderDetailModel>> GetOrderDetails()
        {
            EntityMapper<OrderDetail, OrderDetailModel> mapObj = new EntityMapper<OrderDetail, OrderDetailModel>();
            List<OrderDetail> customersData = ServiceOrderDetail.Get();
            List<OrderDetailModel> customers = new List<OrderDetailModel>();
            foreach (var item in customersData)
            {
                customers.Add(mapObj.Translate(item));
            }
            return Json(customers);
        }

        // GET: api/OrderDetails/GetSpecificOrder/1
        [HttpGet]
        [Route("api/OrderDetails/GetSpecificOrder/{idorder}")]
        public JsonResult<List<SpecificOrderResponse>> GetSpecificOrder(int idorder)
        {
            EntityMapper<Product, ProductModel> mapObjProduct = new EntityMapper<Product, ProductModel>();

            List<OrderDetail> orderDetailsData = ServiceOrderDetail.Get().FindAll((x) => x.OrderID == idorder);
            List<SpecificOrderResponse> specificOrders = new List<SpecificOrderResponse>();
            foreach (var order in orderDetailsData)
            {
                SpecificOrderResponse specificOrder = new SpecificOrderResponse();
                specificOrder.OrderID = order.OrderID;
                specificOrder.Price = order.Price;
                specificOrder.Quantity = order.Quantity;
                specificOrder.OrderDetailID = order.OrderDetailID;

                Product productData = ServiceProduct.GetById(order.ProductID);
                ProductModel product = new ProductModel();
                product = mapObjProduct.Translate(productData);
                specificOrder.Product = product;
                specificOrders.Add(specificOrder);
            }
            return Json<List<SpecificOrderResponse>>(specificOrders);
        }

        // GET: api/OrderDetails/GetOrderDetail/5
        [ResponseType(typeof(OrderDetail))]
        public JsonResult<OrderDetailModel> GetOrderDetail(int id)
        {
            try
            {
                EntityMapper<OrderDetail, OrderDetailModel> mapObj = new EntityMapper<OrderDetail, OrderDetailModel>();
                OrderDetail customerData = ServiceOrderDetail.GetById(id);
                OrderDetailModel customer = new OrderDetailModel();
                customer = mapObj.Translate(customerData);
                return Json(customer);
            }
            catch (Exception)
            {
                return null;
            }
        }

        // PUT: api/OrderDetails/PutOrderDetail/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutOrderDetail(int id, OrderDetailModel orderDetailData)
        {
            EntityMapper<OrderDetailModel, OrderDetail> mapObj = new EntityMapper<OrderDetailModel, OrderDetail>();
            OrderDetail orderDetail = new OrderDetail();
            try
            {
                orderDetail = mapObj.Translate(orderDetailData);
                ServiceOrderDetail.Update(orderDetail, id);
            }
            catch (Exception)
            {
                return StatusCode(HttpStatusCode.BadRequest);
            }

            return StatusCode(HttpStatusCode.OK);
        }

        // POST: api/OrderDetails/PostOrderDetail
        [ResponseType(typeof(OrderDetailModel))]
        public IHttpActionResult PostOrderDetail(OrderDetailModel orderDetailData)
        {
            EntityMapper<OrderDetailModel, OrderDetail> mapObj = new EntityMapper<OrderDetailModel, OrderDetail>();
            OrderDetail orderDetail = new OrderDetail();
            try
            {
                orderDetail = mapObj.Translate(orderDetailData);
                ServiceOrderDetail.Insert(orderDetail);
            }
            catch (Exception)
            {
                return null;
            }
            
            return CreatedAtRoute("DefaultApi", new { id = orderDetail.OrderDetailID }, orderDetail);
        }

        // DELETE: api/OrderDetails/DeleteOrderDetail/5
        [ResponseType(typeof(OrderDetailModel))]
        public IHttpActionResult DeleteOrderDetail(int id)
        {
            OrderDetail orderDetail = ServiceOrderDetail.GetById(id);
            if (orderDetail == null)
            {
                return NotFound();
            }
            ServiceOrderDetail.Delete(id);
            return Ok(orderDetail);
        }

    }
}