using CattleFarmingAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using CattleFarmingAPI.Models;
namespace CattleFarmingAPI.Controllers
{
    public class CustomerController : ApiController
    {

        Cattle_Farming_ManagementEntities db = new Cattle_Farming_ManagementEntities();

        [HttpGet]
        public HttpResponseMessage GetAllCustomers()
        {
            try
            {
                var customer = db.Customer.
              ToList();
                if (customer != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, customer);
                }
                else
                {

                    return Request.CreateResponse(HttpStatusCode.NoContent);
                }
            }
            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }

        }

        [HttpGet]
        public HttpResponseMessage GetCustomer()
        {
            try
            {
                List<Customer> customers = db.Customer.ToList();
                return Request.CreateResponse(HttpStatusCode.OK, customers);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "An error occurred while retrieving the Customer list.");
            }
        }


        [HttpGet]
        public HttpResponseMessage GetCustomerByName(String name)
        {
            var customer = db.Customer.Where(f => f.Name == name).ToList();

            return Request.CreateResponse(HttpStatusCode.OK, customer);
        }



        [HttpPost]

        public HttpResponseMessage saveCustomer(Customer c)

        {
            try
            {



                db.Customer.Add(c);
                db.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.OK, $"Customer {c.Name} added successfully");




            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, $"Error adding Customer: {ex.Message}");
            }
        }


      

        //[HttpGet]
        //public HttpResponseMessage GetAllCustomers()
        //{
        //    List<Customer> customers = new List<Customer>();
        //    return Request.CreateResponse(HttpStatusCode.OK, db.Customer);
        //}

        //[HttpPost]
        //public HttpResponseMessage AddCustomer()
        //{
        //    HttpRequest form = HttpContext.Current.Request;
        //    Customer customer = new Customer();
        //    customer.Name = form["Name"];
        //    customer.Address = form["Address"];
           

        //    db.Customer.Add(customer);
        //    db.SaveChanges();
        //    return Request.CreateResponse(HttpStatusCode.OK, $"Customer Added {customer.Name}");
        //}

        [HttpPost]
        public HttpResponseMessage UpdateCustomer()
        {
            HttpRequest form = HttpContext.Current.Request;
            int id = int.Parse(form["id"]);
            Customer customer = db.Customer.Where(c => c.ID == id).FirstOrDefault();
            customer.Name = form["Name"];
            customer.Address = form["Address"];
            customer.Contact = form["Contact"];
         

            db.SaveChanges();
            return Request.CreateResponse(HttpStatusCode.OK, "Customer Updated");
        }

        [HttpPost]
        public HttpResponseMessage DeleteCustomer()
        {
            HttpRequest form = HttpContext.Current.Request;
            int id = int.Parse(form["id"]);
            Customer customer = db.Customer.Where(c => c.ID == id).FirstOrDefault();
            db.Customer.Remove(customer);
            db.SaveChanges();
            return Request.CreateResponse(HttpStatusCode.OK, "Customer Deleted");
        }
    }
}
