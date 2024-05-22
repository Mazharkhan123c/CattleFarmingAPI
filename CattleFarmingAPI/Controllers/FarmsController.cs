using CattleFarmingAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CattleFarmingAPI.Models;
using System.Web;

namespace CattleFarmingAPI.Controllers
{
    public class FarmsController : ApiController
    {
        Cattle_Farming_ManagementEntities db =new Cattle_Farming_ManagementEntities();
        [HttpPost]

        public HttpResponseMessage saveFarm(Farm c)

        {
            try
            {



                db.Farm.Add(c);
                db.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.OK, $"Farm {c.Name} added successfully");




            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, $"Error adding Customer: {ex.Message}");
            }
        }


        [HttpGet]

        public HttpResponseMessage GetFarm(int id)
        {
            Farm farm = db.Farm.Where(f => f.ID == id).FirstOrDefault();
            return Request.CreateResponse(HttpStatusCode.OK, farm);
        }

        [HttpPost]
        public HttpResponseMessage UpdateFarm()
        {
            HttpRequest form = HttpContext.Current.Request;
            int id = int.Parse(form["ID"]);
            Farm f = db.Farm.Where(fm => fm.ID == id).FirstOrDefault();
            f.Name = form["Name"];

            db.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.OK, "Farm Updated");
        }
        [HttpPost]
        public HttpResponseMessage DeleteFarm()
        {
            HttpRequest form = HttpContext.Current.Request;
            int id = int.Parse(form["ID"]);
            Farm p = db.Farm.Where(fm => fm.ID == id).FirstOrDefault();
            db.Farm.Remove(p);

            db.SaveChanges();
            return Request.CreateResponse(HttpStatusCode.OK, "Farm Deleted");
        }

        [HttpGet]
        public HttpResponseMessage GetAllFarms()
        {
            List<Farm> farm = new List<Farm>();
            return Request.CreateResponse(HttpStatusCode.OK, db.Farm);
        }
    }
}
