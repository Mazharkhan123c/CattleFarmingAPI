using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Helpers;
using System.Web.Http;
using System.Web.UI;
using System.Xml.Linq;
using CattleFarmingAPI.Models;
namespace CattleFarmingAPI.Controllers
{
    public class UserController : ApiController
    {
        Cattle_Farming_ManagementEntities db =new Cattle_Farming_ManagementEntities();



        /*  [HttpGet]
          public HttpResponseMessage GetUserByID(int id)
          {
              User user = db.User.Where(item => item.ID == id).FirstOrDefault();
              return Request.CreateResponse(HttpStatusCode.OK, user);
          }*/
        [HttpGet]
        public HttpResponseMessage Login(String gmailOrPhone, String pass)
        {
            try
            {
                var account = db.User.
                       Where(s => (s.Email == gmailOrPhone || s.Contact == gmailOrPhone) && s.Password == pass).FirstOrDefault();
                if (account != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, account);
                }
                else
                {

                    return Request.CreateResponse(HttpStatusCode.NoContent, "login Successfully");
                }
            }
            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        



        [HttpPost]
       
        public HttpResponseMessage SignUp(string name, string email, string pass, string contact, int farmid, string role)

        {
            try
            {

                if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(pass))
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid input parameters");
                }

               User user = new User
               {
                   Name = name,
                   Email = email,
                   Password = pass,
                   Contact = contact,
                   FarmID = farmid,
                   Role=role

               };


                db.User.Add(user);
                db.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.OK, $"User {name} added successfully");
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, $"Error adding user: {ex.Message}");
            }
        }

        //private string HashPassword(string password)
        //{

        //    return password;
        //}

        //[HttpPost]
        //public HttpResponseMessage SignUp(string name, string email, string pass,
        //    string contact, string image,      int age, string gender, int farmid)
        //{
        //    User user = new User();
        //    user.Name = name;
        //    user.Email = email;
        //    user.Password = pass;
        //    user.Contact = contact;
            
           
           
        //    user.FarmID = farmid;

        //    db.User.Add(user);
        //    db.SaveChanges();
        //    return Request.CreateResponse(HttpStatusCode.OK, $"Item Added {name}");
        //}

      /*  [HttpPost]
        public HttpResponseMessage UpdateUser()
        {
            HttpRequest form = HttpContext.Current.Request;
            int id = int.Parse(form["ID"]);
            var u = db.User.Where(us => us.ID == id).FirstOrDefault();

            u.Name = form["Name"];
            u.Email = form["Email"];
            u.Password = form["Password"];
            u.Contact = form["Contact"];
           
           
  
            db.SaveChanges();
            return Request.CreateResponse(HttpStatusCode.OK, "User Updated");
        }
        [HttpPost]
        public HttpResponseMessage DeleteUser()
        {
            HttpRequest form = HttpContext.Current.Request;
            int id = int.Parse(form["ID"]);
            User p = db.User.Where(us => us.ID == id).FirstOrDefault();
            db.User.Remove(p);
            db.SaveChanges();
            return Request.CreateResponse(HttpStatusCode.OK, "User Deleted");
        }
*/
    /*      [HttpGet]
        public HttpResponseMessage GetAllUser()
        {
            List<User> user = new List<User>();
            return Request.CreateResponse(HttpStatusCode.OK, db.User);
        }*/
        /*
                [HttpPost]
                public HttpResponseMessage AddProductByObject(Product newProduct)
                {
                    db.Products.Add(newProduct);
                    db.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK, $"Item Added {newProduct.title}");
                }
                [HttpPost]
                public HttpResponseMessage AddProductByMultipartRequest()
                {
                    HttpRequest form = HttpContext.Current.Request;
                    Product newProduct = new Product();
                    newProduct.title = form["title"];
                    newProduct.price = int.Parse(form["price"]);
                    newProduct.stock = int.Parse(form["stock"]);
                    db.Products.Add(newProduct);
                    db.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK, $"Item Added {newProduct.title}");
                }
               */


        //[HttpGet]
        //public HttpResponseMessage SearchCattle(String date)
        //{
        //    List<User> user = new List<User>();
        //    user = db.Temperature.Select(p=>p.Temperature1).Where(p=> p.dat)
        //    return Request.CreateResponse(HttpStatusCode.OK, db.User);
        //}
        [HttpGet]
        public HttpResponseMessage GetFarm()
        {
            try
            {
                List<Farm> farms = db.Farm.ToList();
                return Request.CreateResponse(HttpStatusCode.OK, farms);
            }
            catch (Exception ex)
            { 
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "An error occurred while retrieving the farm list.");
            }
        }
       
    }

   


}
