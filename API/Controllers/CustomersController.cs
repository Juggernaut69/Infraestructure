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
using System.Web.Http.Results;
using API;
using API.Models;
using API.Repository;
using Domain;

namespace API.Controllers
{
    public class CustomersController : ApiController
    {
        private OrderContext db = new OrderContext();

        // GET: api/Customers/GetCustomers
        public JsonResult<List<CustomerDomain>> GetCustomers()
        {
            EntityMapper<Customer, CustomerDomain> mapObj = new EntityMapper<Customer, CustomerDomain>();
            List<Customer> customersData = db.Customers.ToList();
            List<CustomerDomain> customers = new List<CustomerDomain>();
            foreach (var item in customersData)
            {
                customers.Add(mapObj.Translate(item));
            }
            return Json<List<CustomerDomain>>(customers);
        }

        // GET: api/Customers/GetCustomer/5
        [ResponseType(typeof(CustomerDomain))]
        public async Task<JsonResult<CustomerDomain>> GetCustomer(int id)
        {
            try
            {
                EntityMapper<Customer, CustomerDomain> mapObj = new EntityMapper<Customer, CustomerDomain>();
                Customer customerData = await db.Customers.FindAsync(id);
                CustomerDomain customer = new CustomerDomain();
                customer = mapObj.Translate(customerData);
                return Json<CustomerDomain>(customer);
            }
            catch (Exception)
            {
                return null;
            }
            
        }

        // PUT: api/Customers/PutCustomer/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutCustomer(int id, CustomerDomain customerData)
        {
            EntityMapper<CustomerDomain,Customer> mapObj = new EntityMapper<CustomerDomain,Customer>();
            Customer customer = new Customer();
            try
            {
                customer = mapObj.Translate(customerData);
            }
            catch (Exception)
            {
                return null;
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != customer.CustomerID)
            {
                return BadRequest();
            }

            db.Entry(customer).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(id))
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

        // POST: api/Customers/PostCustomer
        [ResponseType(typeof(CustomerDomain))]
        public async Task<IHttpActionResult> PostCustomer(CustomerDomain customerData)
        {

            EntityMapper<CustomerDomain, Customer> mapObj = new EntityMapper<CustomerDomain, Customer>();
            Customer customer = new Customer();

            try
            {
                customer = mapObj.Translate(customerData);
            }
            catch (Exception)
            {
                return null;
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Customers.Add(customer);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = customer.CustomerID }, customer);
        }

        // DELETE: api/Customers/DeleteCustomer/5
        [ResponseType(typeof(Customer))]
        public async Task<IHttpActionResult> DeleteCustomer(int id)
        {

            Customer customer = await db.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            db.Customers.Remove(customer);
            await db.SaveChangesAsync();

            return Ok(customer);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CustomerExists(int id)
        {
            return db.Customers.Count(e => e.CustomerID == id) > 0;
        }
    }
}