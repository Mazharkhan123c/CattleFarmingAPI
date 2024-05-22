using CattleFarmingAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CattleFarmingAPI.Controllers
{
    public class TemperatureController : ApiController
    {

        Cattle_Farming_ManagementEntities db = new Cattle_Farming_ManagementEntities();

        [HttpGet]
        public HttpResponseMessage GetAllTemperature()
        {
            try
            {
                var temp = db.Temperature.
              ToList();
                if (temp != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, temp);
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
        public HttpResponseMessage GetTemperatureByDate(String date)
        {
            var temp = db.Temperature.Where(f => f.Date == date).ToList();

            return Request.CreateResponse(HttpStatusCode.OK, temp);
        }



        [HttpPost]

        public HttpResponseMessage SaveTemperature(Temperature t)

        {
            try
            {
               

              
                        db.Temperature.Add(t);
                        db.SaveChanges();

                        return Request.CreateResponse(HttpStatusCode.OK, $"Temperature {t.Temperature1} added successfully");
                    
                              


            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, $"Error adding Temperature: {ex.Message}");
            }
        }
    }
}
