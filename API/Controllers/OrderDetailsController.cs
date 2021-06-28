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

namespace API.Controllers
{
    public class OrderDetailsController : ApiController
    {
        private OrderContext db = new OrderContext();

        // GET: api/OrderDetails/GetOrderDetails
        public JsonResult<List<OrderDetailDomain>> GetOrderDetails()
        {
            EntityMapper<OrderDetail, OrderDetailDomain> mapObj = new EntityMapper<OrderDetail, OrderDetailDomain>();
            List<OrderDetail> orderDetailsData = db.OrderDetails.ToList();
            List<OrderDetailDomain> ordersDetail = new List<OrderDetailDomain>();
            foreach (var item in orderDetailsData)
            {
                ordersDetail.Add(mapObj.Translate(item));
            }
            return Json<List<OrderDetailDomain>>(ordersDetail);
        }

        // GET: api/OrderDetails/GetSpecificOrder/1
        [HttpGet]
        [Route("api/OrderDetails/GetSpecificOrder/{idorder}")]
        public async Task<JsonResult<List<SpecificOrderDomain>>> GetSpecificOrder(int idorder)
        {
            EntityMapper<Product, ProductDomain> mapObjProduct = new EntityMapper<Product, ProductDomain>();

            List<OrderDetail> orderDetailsData = db.OrderDetails.ToList().FindAll(x => x.OrderID == idorder);
            List<SpecificOrderDomain> specificOrders = new List<SpecificOrderDomain>();
            foreach (var order in orderDetailsData)
            {
                SpecificOrderDomain specificOrder = new SpecificOrderDomain();
                specificOrder.OrderID = order.OrderID;
                specificOrder.Price = order.Price;
                specificOrder.Quantity = order.Quantity;
                specificOrder.OrderDetailID = order.OrderDetailID;

                Product productData = await db.Products.FindAsync(order.ProductID);
                ProductDomain product = new ProductDomain();
                product = mapObjProduct.Translate(productData);
                specificOrder.Product = product;
                specificOrders.Add(specificOrder);
            }
            return Json<List<SpecificOrderDomain>>(specificOrders);
        }

        // GET: api/OrderDetails/GetOrderDetail/5
        [ResponseType(typeof(OrderDetail))]
        public async Task<JsonResult<OrderDetailDomain>> GetOrderDetail(int id)
        {
            try
            {
                EntityMapper<OrderDetail, OrderDetailDomain> mapObj = new EntityMapper<OrderDetail, OrderDetailDomain>();
                OrderDetail customerData = await db.OrderDetails.FindAsync(id);
                OrderDetailDomain customer = new OrderDetailDomain();
                customer = mapObj.Translate(customerData);
                return Json<OrderDetailDomain>(customer);
            }
            catch (Exception)
            {
                return null;
            }
        }

        // PUT: api/OrderDetails/PutOrderDetail/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutOrderDetail(int id, OrderDetailDomain orderDetailData)
        {
            EntityMapper<OrderDetailDomain, OrderDetail> mapObj = new EntityMapper<OrderDetailDomain, OrderDetail>();
            OrderDetail orderDetail = new OrderDetail();
            try
            {
                orderDetail = mapObj.Translate(orderDetailData);
            }
            catch (Exception)
            {
                return null;
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != orderDetail.OrderDetailID)
            {
                return BadRequest();
            }

            db.Entry(orderDetail).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderDetailExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/OrderDetails/PostOrderDetail
        [ResponseType(typeof(OrderDetail))]
        public async Task<IHttpActionResult> PostOrderDetail(OrderDetailDomain orderDetailData)
        {
            EntityMapper<OrderDetailDomain, OrderDetail> mapObj = new EntityMapper<OrderDetailDomain, OrderDetail>();
            OrderDetail orderDetail = new OrderDetail();
            try
            {
                orderDetail = mapObj.Translate(orderDetailData);
            }
            catch (Exception)
            {
                return null;
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.OrderDetails.Add(orderDetail);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = orderDetail.OrderDetailID }, orderDetail);
        }

        // DELETE: api/OrderDetails/DeleteOrderDetail/5
        [ResponseType(typeof(OrderDetail))]
        public async Task<IHttpActionResult> DeleteOrderDetail(int id)
        {
            OrderDetail orderDetail = await db.OrderDetails.FindAsync(id);
            if (orderDetail == null)
            {
                return NotFound();
            }

            db.OrderDetails.Remove(orderDetail);
            await db.SaveChangesAsync();

            return Ok(orderDetail);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool OrderDetailExists(int id)
        {
            return db.OrderDetails.Count(e => e.OrderDetailID == id) > 0;
        }
    }
}