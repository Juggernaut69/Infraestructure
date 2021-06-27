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
    public class ProductsController : ApiController
    {
        private OrderContext db = new OrderContext();

        // GET: api/Products/GetProducts
        public JsonResult<List<ProductDomain>> GetProducts()
        {
            EntityMapper<Product, ProductDomain> mapObj = new EntityMapper<Product, ProductDomain>();
            List<Product> productsData = db.Products.ToList();
            List<ProductDomain> product = new List<ProductDomain>();
            foreach (var item in productsData)
            {
                product.Add(mapObj.Translate(item));
            }
            return Json<List<ProductDomain>>(product);
        }

        // GET: api/Products/GetProduct/5
        [ResponseType(typeof(Product))]
        public async Task<JsonResult<ProductDomain>> GetProduct(int id)
        {
            try
            {
                EntityMapper<Product, ProductDomain> mapObj = new EntityMapper<Product, ProductDomain>();
                Product productData = await db.Products.FindAsync(id);
                ProductDomain product = new ProductDomain();
                product = mapObj.Translate(productData);
                return Json<ProductDomain>(product);
            }
            catch (Exception)
            {
                return null;
            }
        }

        // PUT: api/Products/PutProduct/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutProduct(int id, ProductDomain productData)
        {

            EntityMapper<ProductDomain, Product> mapObj = new EntityMapper<ProductDomain, Product>();
            Product product = new Product();
            try
            {
                product = mapObj.Translate(productData);
            }
            catch (Exception)
            {
                return null;
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != product.ProductID)
            {
                return BadRequest();
            }

            db.Entry(product).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
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

        // POST: api/Products/PostProduct
        [ResponseType(typeof(Product))]
        public async Task<IHttpActionResult> PostProduct(ProductDomain productData)
        {
            EntityMapper<ProductDomain, Product> mapObj = new EntityMapper<ProductDomain, Product>();
            Product product = new Product();
            try
            {
                product = mapObj.Translate(productData);
            }
            catch (Exception)
            {
                return null;
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Products.Add(product);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = product.ProductID }, product);
        }

        // DELETE: api/Products/DeleteProduct/5
        [ResponseType(typeof(Product))]
        public async Task<IHttpActionResult> DeleteProduct(int id)
        {
            Product product = await db.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            db.Products.Remove(product);
            await db.SaveChangesAsync();

            return Ok(product);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProductExists(int id)
        {
            return db.Products.Count(e => e.ProductID == id) > 0;
        }
    }
}