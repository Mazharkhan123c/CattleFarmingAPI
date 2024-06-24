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
    public class VaccinationController : ApiController
    {
        Cattle_Farming_ManagementEntities db = new Cattle_Farming_ManagementEntities();
        [HttpGet]
        public HttpResponseMessage GetAllVaccinationsStock()
        {
            List<Vaccination> vaccinations = new List<Vaccination>();
            return Request.CreateResponse(HttpStatusCode.OK, db.Vaccination);
        }


        [HttpPost]
        public HttpResponseMessage SaveVaccineStock(Vaccination c)
        {
            try
            {
                // Assuming "db" is your database context
                db.Vaccination.Add(c);
                db.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.OK, $"Vaccine {c.Type} Save successfully");
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, $"Error in Saving Vaccination: {ex.Message}");
            }
        }

        [HttpPost]
        public HttpResponseMessage SaveInjectedVaccine(InjectedVaccination c)
        {
            try
            {
                // Assuming "db" is your database context
                db.InjectedVaccination.Add(c);
                db.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.OK, $"Cattle {c.CattleTag} Vaccinated successfully");
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, $"Error in Saving inject Vaccination: {ex.Message}");
            }
        }



        [HttpGet]
        public IHttpActionResult GetLastInjectedVaccineForTag(string tag)
        {
            // Find the last injected vaccination record for the given tag
            var lastInjected = db.InjectedVaccination
                .Where(iv => iv.CattleTag == tag)
                .OrderByDescending(iv => iv.Date)
                .FirstOrDefault();

            if (lastInjected == null)
            {
                return NotFound(); // Return 404 if not found
            }

            return Ok(lastInjected); // Return the last injected vaccination record
        }


        //[HttpPost]
        //public HttpResponseMessage AddVaccination()
        //{
        //    HttpRequest form = HttpContext.Current.Request;
        //    Vaccination vaccination = new Vaccination();
        //    vaccination.Type = form["Type"];
        //  //  vaccination.ValidityPeriod = form["ValidityPeriod"];
        //   // vaccination.Description = form["Description"];


        //    db.Vaccination.Add(vaccination);
        //    db.SaveChanges();
        //    return Request.CreateResponse(HttpStatusCode.OK, $"Vaccination Added {vaccination.Type}");
        //}

        [HttpPost]
        public HttpResponseMessage UpdateVaccination()
        {
            HttpRequest form = HttpContext.Current.Request;
            int id = int.Parse(form["id"]);
            Vaccination vaccination = db.Vaccination.Where(v => v.ID == id).FirstOrDefault();
            vaccination.Type = form["Type"];
         //   vaccination.ValidityPeriod = form["ValidityPeriod"];
         //   vaccination.Description = form["Description"];
           

            db.SaveChanges();
            return Request.CreateResponse(HttpStatusCode.OK, "Vaccination Updated");
        }

        [HttpPost]
        public HttpResponseMessage DeleteVaccination()
        {
            HttpRequest form = HttpContext.Current.Request;
            int id = int.Parse(form["id"]);
            Vaccination vaccination = db.Vaccination.Where(v => v.ID == id).FirstOrDefault();
            db.Vaccination.Remove(vaccination);
            db.SaveChanges();
            return Request.CreateResponse(HttpStatusCode.OK, "Vaccination Deleted");
        }
    }
}

