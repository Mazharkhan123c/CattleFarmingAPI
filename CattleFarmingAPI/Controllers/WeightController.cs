﻿using CattleFarmingAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace CattleFarmingAPI.Controllers
{
    public class WeightController : ApiController
    {

        Cattle_Farming_ManagementEntities db = new Cattle_Farming_ManagementEntities();
        [HttpGet]
        public HttpResponseMessage GetWeightByTag(String tag)
        {
            var weight = db.Weight.Where(f => f.CattleTag == tag).ToList();

            return Request.CreateResponse(HttpStatusCode.OK, weight);
        }


        [HttpGet]
        public HttpResponseMessage
            GetLastMatricsByTag(string cattleTag)
        {
            var matrics =  db.Weight
                .Where(w => w.CattleTag == cattleTag)
                .OrderByDescending(w => w.Date)
                .FirstOrDefault();

            if (matrics == null)
            {
               // return NotFound("No weight records found for the given cattle tag.");
                return Request.CreateResponse(HttpStatusCode.NoContent, "No weight records found for the given cattle tag.");
            }
            return Request.CreateResponse(HttpStatusCode.OK, matrics);
          //  return Ok(weight);
        }
    
    [HttpGet]
        public HttpResponseMessage GetAllWeight()
        {
            try
            {
                var weight = db.Weight.
              ToList();
                if (weight != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, weight);
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
        public HttpResponseMessage GetWeightByDate(String date)
        {
            var weigt = db.Weight.Where(f => f.Date == date).ToList();

            return Request.CreateResponse(HttpStatusCode.OK, weigt);
        }



        [HttpPost]

        public HttpResponseMessage SaveWeight(Weight w)

        {
            try
            {
                var weight = db.Weight.Where(s => s.CattleTag == w.CattleTag && s.Date == w.Date).FirstOrDefault();
                var cattle=db.Cattle.Where(c =>c.Tag==w.CattleTag).FirstOrDefault();
                if (cattle != null)
                {
                    if (weight != null)
                    {

                        return Request.CreateResponse(HttpStatusCode.OK, "Already exsist");
                    }
                    else
                    {
                        db.Weight.Add(w);
                        db.SaveChanges();

                        return Request.CreateResponse(HttpStatusCode.OK, $"Weight {w.Weight1} added successfully");
                    }
                }
                else {

                    return Request.CreateResponse(HttpStatusCode.OK, "Weight Not exsist");
                }
               

            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, $"Error adding Weight: {ex.Message}");
            }
        }
    }
}
