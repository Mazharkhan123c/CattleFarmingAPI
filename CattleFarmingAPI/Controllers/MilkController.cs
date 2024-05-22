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
    public class MilkController : ApiController
    {
        Cattle_Farming_ManagementEntities db = new Cattle_Farming_ManagementEntities();

        [HttpGet]
        public HttpResponseMessage GetAllMilkDetails()
        {
            List<MilkCollection> milks = new List<MilkCollection>();
            return Request.CreateResponse(HttpStatusCode.OK, db.MilkCollection);
        }


        [HttpPost]
       
         public HttpResponseMessage SaveMilkCollection(MilkCollection c)

        {
            try
            {
                var milkCollect = db.MilkCollection.Where(s => s.CattleTag == c.CattleTag && s.Time == c.Time && s.Date == c.Date).FirstOrDefault();
                var catle = db.Cattle.Where(s => s.Tag == c.CattleTag).FirstOrDefault();
                if (catle != null)
                {

                    if (milkCollect != null)
                    {

                        return Request.CreateResponse(HttpStatusCode.OK, "Milk Already exsist");
                    }
                    else
                    {
                        db.MilkCollection.Add(c);
                        db.SaveChanges();

                        return Request.CreateResponse(HttpStatusCode.OK, $"Milk added successfully");
                    } }
                else {
                    return Request.CreateResponse(HttpStatusCode.OK, "Cattle not exsist");

                }
                
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, $"Error adding Milk Collection: {ex.Message}");
            }
        }



        [HttpGet]
        public HttpResponseMessage GetMilkCollectionByDate(String date)
        {
            var temp = db.MilkCollection.Where(f => f.Date == date).ToList();

            return Request.CreateResponse(HttpStatusCode.OK, temp);
        }
        [HttpPost]
       // [Route("api/MilkCollection/Delete")]
        public HttpResponseMessage DeleteMilkCollection( int id)
        {
            try
            {
                var milkCollection = db.MilkCollection.Find(id);
                if (milkCollection == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Milk collection record not found.");
                }

                db.MilkCollection.Remove(milkCollection);
                db.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.OK, "Milk collection record deleted successfully.");
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, $"Error deleting record: {ex.Message}");
            }
        }

        // POST: api/MilkCollection/Update
        [HttpPost]
      //  [Route("api/MilkCollection/Update")]
        public HttpResponseMessage UpdateMilkCollection(MilkCollection mc)
        {
            try
            {
                var oldRecord = db.MilkCollection.Find(mc.ID);
                if (oldRecord == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Milk collection record not found.");
                }

                // Update the fields
                oldRecord.Date = mc.Date;
                oldRecord.TotalMilk = mc.TotalMilk;
                oldRecord.Time = mc.Time;
                oldRecord.CattleTag = mc.CattleTag;
                oldRecord.UsedMilk = mc.UsedMilk;
                oldRecord.Note = mc.Note;

            //    db.Entry(oldRecord).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.OK, "Milk collection record updated successfully.");
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, $"Error updating record: {ex.Message}");
            }
        }

        [HttpPost]
        public HttpResponseMessage UpdateMilk()
        {
            HttpRequest form = HttpContext.Current.Request;
            int id = int.Parse(form["id"]);
            MilkCollection milk = db.MilkCollection.Where(m => m.ID == id).FirstOrDefault();
          
            milk.TotalMilk = decimal.Parse(form["TotalMilk"]);
            milk.Time = form["Time"];
       

            db.SaveChanges();
            return Request.CreateResponse(HttpStatusCode.OK, "Milk Updated");
        }

        [HttpPost]
        public HttpResponseMessage DeleteMilk()
        {
            HttpRequest form = HttpContext.Current.Request;
            int id = int.Parse(form["id"]);
            MilkCollection milk = db.MilkCollection.Where(m => m.ID == id).FirstOrDefault();
            db.MilkCollection.Remove(milk);
            db.SaveChanges();
            return Request.CreateResponse(HttpStatusCode.OK, "Milk Deleted");
        }
        [HttpGet]
        public HttpResponseMessage CattleTotalMilk()
        {
            var request = HttpContext.Current.Request;
            string type = request["type"];
            //var result = (from c in db.Cattle
            //              join m in db.MilkCollection
            //              on c.ID equals m.CattleID
            //              select new { TotalMilk = m.TotalMilk, Type = c.CattleCategory }
            //           ).Where(n => n.Type == type
            //           ).Sum(n=>n.TotalMilk);

            //OR

            var result = (from c in db.Cattle
                          join m in db.MilkCollection on c.Tag equals m.CattleTag
                          where c.CattleType == type
                          group m by c.CattleType into g
                          select new { TotalMilk = g.Sum(x => x.TotalMilk) }
                   ).SingleOrDefault();

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}
