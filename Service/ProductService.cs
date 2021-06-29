using Infraestructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;

namespace Service
{
    public class ProductService
    {
        private OrdersContext db = new OrdersContext();
        public List<Product> Get()
        {
            List<Product> products = null;
            products = db.Products.ToList();
            return products;
        }

        public Product GetById(int ID)
        {
            Product product = null;
            product = db.Products.Find(ID);
            return product;
        }

        public void Insert(Product product)
        {
            db.Products.Add(product);
            db.SaveChanges();
        }

        public void Update(Product product, int ID)
        {
            var newProduct = db.Products.Find(ID);
            newProduct.OrderDetails = product.OrderDetails;
            newProduct.Price = product.Price;
            newProduct.ProductID = product.ProductID;
            newProduct.ProductName = product.ProductName;
            db.SaveChanges();
        }

        public void Delete(int ID)
        {
            var product = db.Products.Find(ID);
            db.Products.Remove(product);
            db.SaveChanges();
        }

    }
}
