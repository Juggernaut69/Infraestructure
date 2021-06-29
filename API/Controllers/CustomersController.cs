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
using Service;

namespace API.Controllers
{
    public class CustomersController : ApiController
    {
        CustomerService Service;

        public CustomersController()
        {
            Service = new CustomerService();
        }

        // GET: api/Customers/GetCustomers
        public JsonResult<List<CustomerModel>> GetCustomers()
        {
            EntityMapper<Customer, CustomerModel> mapObj = new EntityMapper<Customer, CustomerModel>();
            List<Customer> customersData = Service.Get();
            List<CustomerModel> customers = new List<CustomerModel>();
            foreach (var item in customersData)
            {
                customers.Add(mapObj.Translate(item));
            }
            return Json(customers);
        }

        // GET: api/Customers/GetCustomer/5
        [ResponseType(typeof(CustomerModel))]
        public JsonResult<CustomerModel> GetCustomer(int id)
        {
            try
            {
                EntityMapper<Customer, CustomerModel> mapObj = new EntityMapper<Customer, CustomerModel>();
                Customer customerData = Service.GetById(id);
                CustomerModel customer = new CustomerModel();
                customer = mapObj.Translate(customerData);
                return Json(customer);
            }
            catch (Exception)
            {
                return null;
            }
            
        }

        // PUT: api/Customers/PutCustomer/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCustomer(int id, CustomerModel customerData)
        {
            EntityMapper<CustomerModel, Customer> mapObj = new EntityMapper<CustomerModel, Customer>();
            Customer customer = new Customer();
            try
            {
                customer = mapObj.Translate(customerData);
                Service.Update(customer, id);
            }
            catch (Exception)
            {
                return null;
            }

            return StatusCode(HttpStatusCode.OK);
        }

        // POST: api/Customers/PostCustomer
        [ResponseType(typeof(CustomerModel))]
        public IHttpActionResult PostCustomer( CustomerModel customerData)
        {

            EntityMapper<CustomerModel, Customer> mapObj = new EntityMapper<CustomerModel, Customer>();
            Customer customer = new Customer();

            try
            {
                customer = mapObj.Translate(customerData);
                Service.Insert(customer);
            }
            catch (Exception)
            {
                return null;
            }

            return CreatedAtRoute("DefaultApi", new { id = customer.CustomerID }, customer);
        }

        // DELETE: api/Customers/DeleteCustomer/5
        [ResponseType(typeof(Customer))]
        public IHttpActionResult DeleteCustomer(int id)
        {

            Customer customer = Service.GetById(id);
            if (customer == null)
            {
                return NotFound();
            }

            Service.Delete(id);

            return Ok(customer);
        }

    }
}