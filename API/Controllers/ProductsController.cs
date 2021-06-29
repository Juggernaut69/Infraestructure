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


namespace API.Controllers
{
    public class ProductsController : ApiController
    {

        ProductService Service;

        public ProductsController()
        {
            Service = new ProductService();
        }

        // GET: api/Products/GetProducts
        public JsonResult<List<ProductModel>> GetProducts()
        {
            EntityMapper<Product, ProductModel> mapObj = new EntityMapper<Product, ProductModel>();
            List<Product> productsData = Service.Get();
            List<ProductModel> products = new List<ProductModel>();
            foreach (var item in productsData)
            {
                products.Add(mapObj.Translate(item));
            }
            return Json(products);
        }

        // GET: api/Products/GetProduct/5
        [ResponseType(typeof(ProductModel))]
        public JsonResult<ProductModel> GetProduct(int id)
        {
            try
            {
                EntityMapper<Product, ProductModel> mapObj = new EntityMapper<Product, ProductModel>();
                Product productData = Service.GetById(id);
                ProductModel product = new ProductModel();
                product = mapObj.Translate(productData);
                return Json(product);
            }
            catch (Exception)
            {
                return null;
            }
        }

        // PUT: api/Products/PutProduct/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutProduct(int id, ProductModel productData)
        {
            EntityMapper<ProductModel, Product> mapObj = new EntityMapper<ProductModel, Product>();
            Product product = new Product();
            try
            {
                product = mapObj.Translate(productData);
                Service.Update(product, id);
            }
            catch (Exception)
            {
                return null;
            }

            return StatusCode(HttpStatusCode.OK);
        }

        // POST: api/Products/PostProduct
        [ResponseType(typeof(ProductModel))]
        public IHttpActionResult PostProduct(ProductModel productData)
        {

            EntityMapper<ProductModel, Product> mapObj = new EntityMapper<ProductModel, Product>();
            Product product = new Product();

            try
            {
                product = mapObj.Translate(productData);
                Service.Insert(product);
            }
            catch (Exception)
            {
                return null;
            }

            return CreatedAtRoute("DefaultApi", new { id = product.ProductID }, product);
        }

        // DELETE: api/Products/DeleteProduct/5
        [ResponseType(typeof(ProductModel))]
        public IHttpActionResult DeleteProduct(int id)
        {
            
            Product product = Service.GetById(id);

            if (product == null)
            {
                return NotFound();
            }

            Service.Delete(id);

            return Ok(product);
        }

    }
}