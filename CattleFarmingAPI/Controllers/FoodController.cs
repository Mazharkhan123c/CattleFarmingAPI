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
    public class FoodController : ApiController
    {
        Cattle_Farming_ManagementEntities db = new Cattle_Farming_ManagementEntities();

        //Funtion to get Record of All Food

        [HttpGet]
        public HttpResponseMessage GetAllFood()
        {
            List<Food> food = new List<Food>();
            return Request.CreateResponse(HttpStatusCode.OK, db.Food);
        }

        //Function to Insert Record of Food

        [HttpPost]
        public HttpResponseMessage AddFood()
        {
            HttpRequest form = HttpContext.Current.Request;
            Food fd = new Food();
            fd.Quantity = form["Quantity"];
           
            fd.Item = form["Item"];
            fd.Price = int.Parse(form["Price"]);
            fd.ExpiryDate = DateTime.Parse(form["ExpiryDate"]);
            fd.CattleTag = form["CattleTag"];           
            db.Food.Add(fd);
            db.SaveChanges();
            return Request.CreateResponse(HttpStatusCode.OK, $"Food Added {fd.Item}");
        }

        [HttpPost]
        public HttpResponseMessage UpdateFood()
        {
            HttpRequest form = HttpContext.Current.Request;
            int id = int.Parse(form["id"]);
            Food fd = db.Food.Where(pd => pd.ID == id).FirstOrDefault();
            fd.Quantity = form["Quantity"];
            fd.Date = form["Date"] ;
            fd.Item = form["Item"];
            fd.Price = int.Parse(form["Price"]);
            fd.ExpiryDate = DateTime.Parse(form["ExpiryDate"]);
            fd.CattleTag = form["CattleTag"];

            db.SaveChanges();
            return Request.CreateResponse(HttpStatusCode.OK, "Food Updated");
        }

        [HttpPost]
        public HttpResponseMessage DeleteProduct()
        {
            HttpRequest form = HttpContext.Current.Request;
            int id = int.Parse(form["id"]);
            Food fd = db.Food.Where(pd => pd.ID == id).FirstOrDefault();
            db.Food.Remove(fd);
            db.SaveChanges();
            return Request.CreateResponse(HttpStatusCode.OK, "Food Deleted");
        }

    }
}
