using CattleFarmingAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using CattleFarmingAPI.Models;
using System.Web.Http.Results;
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

        [HttpGet]
      //  [Route("api/Milk/GetAliveCattleWithMilk/{tag}")]
        public HttpResponseMessage GetAliveCattleWithMilk(string tag)
        {
            var query = db.Cattle
                .Where(c => c.Status == "Alive" && c.Gender == "Female" && c.Tag == tag)
                .GroupJoin(
                    db.MilkCollection,
                    cattle => cattle.Tag,
                    milk => milk.CattleTag,
                    (cattle, milks) => new { Cattle = cattle, Milks = milks }
                )
                .Select(g => new
                {
                    Tag = g.Cattle.Tag,
                    MilkRecord = g.Milks
                        .OrderByDescending(milk => milk.Date)
                        .FirstOrDefault(),
                    g.Cattle.CattleType,
                    g.Cattle.DOB
                })
                .FirstOrDefault(); // Ensure we get only the first (and only) matching record

            if (query == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "Cattle not found.");
            }

            // Process the result to handle null values and calculate availability
            var result = new CattleMilkInfo
            {
                Tag = query.Tag,
                TotalMilk = query.MilkRecord?.TotalMilk ?? 0, // Handle null values here
                IsAvailableToGiveMilk = IsCattleAvailableToGiveMilk(query.CattleType, DateTime.Parse(query.DOB))
            };

            // Check if the cattle is available to give milk
            if (!result.IsAvailableToGiveMilk)
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, "Cattle is not available to give milk.");
            }

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        //private bool IsCattleAvailableToGiveMilk(string cattleType, DateTime dob)
        //{
        //    int ageYears = CalculateAge(dob);

        //    if ((cattleType == "Cow" || cattleType == "Buffalo") && ageYears >= 2)
        //    {
        //        return true;
        //    }
        //    else if (cattleType == "Goat" && ageYears >= 1)
        //    {
        //        return true;
        //    }

        //    return false;
        //}

        //private int CalculateAge(DateTime dob)
        //{
        //    var today = DateTime.Today;
        //    var ageYears = today.Year - dob.Year;
        //    var ageMonths = today.Month - dob.Month;
        //    var ageDays = today.Day - dob.Day;

        //    if (ageDays < 0)
        //    {
        //        ageMonths--;
        //        ageDays += DateTime.DaysInMonth(today.Year, (today.Month - 1) < 1 ? 12 : today.Month - 1);
        //    }

        //    if (ageMonths < 0)
        //    {
        //        ageYears--;
        //        ageMonths += 12;
        //    }

        //    return ageYears;
        //}



        [HttpPost]
[Route("api/Milk/PostUpdatedSingleCattleMilkData")]
public HttpResponseMessage PostUpdatedSingleCattleMilkData([FromBody] MilkCollection updatedMilk)
{
    var existingMilk = db.MilkCollection.FirstOrDefault(m => m.CattleTag == updatedMilk.CattleTag && m.Date == updatedMilk.Date && m.Time == updatedMilk.Time);
    if (existingMilk != null)
    {
        existingMilk.TotalMilk = updatedMilk.TotalMilk;
    }
    else
    {
        db.MilkCollection.Add(updatedMilk);
    }

    db.SaveChanges();
    return Request.CreateResponse(HttpStatusCode.OK);

}
        //   [HttpGet]
        //   [Route("api/Milk/GetAliveCattleWithMilk")]
        //   public HttpResponseMessage GetAliveCattleWithMilk()
        //   {
        //       // Assuming db is your DbContext
        //       var query = db.Cattle
        //   .Where(c => c.Status == "Alive" && c.Gender == "Female")
        //   .GroupJoin(
        //       db.MilkCollection,
        //       cattle => cattle.Tag,
        //       milk => milk.CattleTag,
        //       (cattle, milks) => new { Cattle = cattle, Milks = milks }
        //   )
        //   .Select(g => new
        //   {
        //       Tag = g.Cattle.Tag,
        //       MilkRecords = g.Milks
        //           .OrderByDescending(milk => milk.Date)
        //           .FirstOrDefault()
        //   })
        //   .ToList(); // Execute the query here


        //       var result = query
        //.Select(g => new CattleMilkInfo
        //{
        //    Tag = g.Tag,
        //    TotalMilk = g.MilkRecords?.TotalMilk ?? 0 // Handle null values here
        //})
        //.ToList();

        //       return Request.CreateResponse(HttpStatusCode.OK, result);
        //   }

        //  ----------------------------This below method get cattlesmilk with age greater the 2 and 1 year
        [HttpGet]
      //  [Route("api/Milk/GetAliveCattleWithMilk")]
        public HttpResponseMessage GetAliveCattleWithMilk()
        {
            var query = db.Cattle
       .Where(c => c.Status == "Alive" && c.Gender == "Female")
       .GroupJoin(
           db.MilkCollection,
           cattle => cattle.Tag,
           milk => milk.CattleTag,
           (cattle, milks) => new { Cattle = cattle, Milks = milks }
       )
       .ToList() // Execute the query here to work with in-memory data
       .Select(g => new
       {
           Tag = g.Cattle.Tag,
           MilkRecords = g.Milks
               .OrderByDescending(milk => milk.Date)
               .FirstOrDefault(),
           g.Cattle.CattleType,
           g.Cattle.DOB
       })
       .ToList(); // Ensure we are working with in-memory data for subsequent calculations

            // Process the results to handle null values and calculate availability
            var result = query
                .Select(g => new CattleMilkInfo
                {
                    Tag = g.Tag,
                    TotalMilk = g.MilkRecords?.TotalMilk ?? 0, // Handle null values here
                    IsAvailableToGiveMilk = IsCattleAvailableToGiveMilk(g.CattleType, DateTime.Parse(g.DOB))
                })
                        .Where(info => info.IsAvailableToGiveMilk)
                        .ToList();

            // Return or use the result as needed


            return Request.CreateResponse(HttpStatusCode.OK, result);
        }


        private bool IsCattleAvailableToGiveMilk(string cattleType, DateTime dob)
        {

            int ageYears = CalculateAge(dob);

            //int age= ageYears.ToString().Length;

            if ((cattleType == "Cow" || cattleType == "Buffalo") && ageYears >= 2)
            {
                return true;
            }
            else if (cattleType == "Goat" && ageYears >= 1)
            {
                return true;
            }

            return false;
        }

        private int CalculateAge(DateTime dob)
        {
            var today = DateTime.Today;
            var ageYears = today.Year - dob.Year;
            var ageMonths = today.Month - dob.Month;
            var ageDays = today.Day - dob.Day;

            if (ageDays < 0)
            {
                ageMonths--;
                ageDays += DateTime.DaysInMonth(today.Year, (today.Month - 1) < 1 ? 12 : today.Month - 1);
            }

            if (ageMonths < 0)
            {
                ageYears--;
                ageMonths += 12;
            }

            return ageYears;
        }




        [HttpPost]
        public HttpResponseMessage PostUpdatedMilkData([FromBody] List<MilkCollection> updatedMilks)
        {
            foreach (var milk in updatedMilks)
            {
                var existingMilk = db.MilkCollection.FirstOrDefault(m => m.CattleTag == milk.CattleTag && m.Date == milk.Date && m.Time == milk.Time);
                if (existingMilk != null)
                {
                    existingMilk.TotalMilk = milk.TotalMilk;
                }
                else
                {
                    db.MilkCollection.Add(milk);
                }
            }
            db.SaveChanges();
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        //======================This below post method is correct but now requirements are changed now we post data in list
      
        [HttpPost]
        //[Route("SaveMilkCollection")]
        [Route("api/Milk/SaveMilkCollection")]
        public HttpResponseMessage SaveMilkCollection([FromBody]  MilkCollection c)
        {
            try
            {
                // Find the cattle by tag
                var cattle = db.Cattle.Where(s => s.Tag == c.CattleTag).FirstOrDefault();

                // Check if the cattle exists
                if (cattle == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Cattle not exist");
                }

                // Check if the cattle is female
                if (cattle.Gender != "Female")
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Male Cattle not give Milk");
                }

                // Check if the milk collection entry already exists
                var milkCollect = db.MilkCollection
                    .Where(s => s.CattleTag == c.CattleTag && s.Time == c.Time && s.Date == c.Date)
                    .FirstOrDefault();

                if (milkCollect != null)
                {
                    return Request.CreateResponse(HttpStatusCode.Conflict, "Milk entry already exists");
                }

                // Add the milk collection entry
                db.MilkCollection.Add(c);
                db.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.OK, "Milk added successfully");
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, $"Error adding milk collection: {ex.Message}");
            }
        }


        //[HttpPost]

        // public HttpResponseMessage SaveMilkCollection(MilkCollection c)

        //{
        //    try
        //    {
        //      //  var mcatle = db.Cattle.Where(s => s.Gender == "Female").ToList();
        //        var milkCollect = db.MilkCollection.Where(s => s.CattleTag == c.CattleTag && s.Time == c.Time && s.Date == c.Date).FirstOrDefault();
        //        var catle = db.Cattle.Where(s => s.Tag == c.CattleTag).FirstOrDefault();

        //        //if (mcatle != null)
        //        //{
        //            if (catle != null)
        //            {

        //                if (milkCollect != null)
        //                {

        //                    return Request.CreateResponse(HttpStatusCode.OK, "Milk Already exsist");
        //                }
        //                else
        //                {
        //                    db.MilkCollection.Add(c);
        //                    db.SaveChanges();

        //                    return Request.CreateResponse(HttpStatusCode.OK, $"Milk added successfully");
        //                }
        //            }
        //            else
        //            {
        //                return Request.CreateResponse(HttpStatusCode.OK, "Cattle not exsist");

        //            }


        //        //}
        //        //else
        //        //{
        //        //    return Request.CreateResponse(HttpStatusCode.OK, "Male dont give this type of Milk");
        //        //}

        //    }
        //    catch (Exception ex)
        //    {

        //        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, $"Error adding Milk Collection: {ex.Message}");
        //    }
        //}



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
               oldRecord.MilkAvailability = mc.MilkAvailability;

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


        // in thsi below function milk sale table not include
        //[HttpGet]
        //public HttpResponseMessage CattleTypeTotalMilk(int farmId, string type)
        //{
        //    // Query to calculate total milk for a given farmId and cattle type
        //    if (type == "TotalMilk")
        //    {
        //        var result = (from c in db.Cattle
        //                      join m in db.MilkCollection on c.Tag equals m.CattleTag
        //                      where c.FarmID == farmId
        //                      select m.TotalMilk
        //                     ).Sum();

        //        return Request.CreateResponse(HttpStatusCode.OK, new { TotalMilk = result });
        //    }
        //    else
        //    {
        //        var result = (from c in db.Cattle
        //                      join m in db.MilkCollection on c.Tag equals m.CattleTag
        //                      where c.FarmID == farmId && c.CattleType == type
        //                      group m by c.CattleType into g
        //                      select new { TotalMilk = g.Sum(x => x.TotalMilk) }
        //                    ).SingleOrDefault();

        //        return Request.CreateResponse(HttpStatusCode.OK, result);
        //    }

        //}

        [HttpGet]
        public HttpResponseMessage CattleTypeTotalMilk(int farmId, string type)
        {
            if (type == "TotalMilk")
            {
                var totalMilkCollected = (from m in db.MilkCollection
                                          join c in db.Cattle on m.CattleTag equals c.Tag
                                          where c.FarmID == farmId
                                          select m.TotalMilk).DefaultIfEmpty(0).Sum();

                var totalMilkSold = db.MilkSale
                                     .Where(s => s.FarmId == farmId)
                                     .Select(s => (decimal?)s.Quantity) // Cast to nullable decimal to handle no records case
                                     .DefaultIfEmpty(0).Sum();

                var remainingMilk = totalMilkCollected - totalMilkSold.GetValueOrDefault();

                return Request.CreateResponse(HttpStatusCode.OK, new { TotalMilk = remainingMilk });
            }
            else
            {
                var totalMilkCollected = (from m in db.MilkCollection
                                          join c in db.Cattle on m.CattleTag equals c.Tag
                                          where c.FarmID == farmId && c.CattleType == type
                                          select m.TotalMilk).DefaultIfEmpty(0).Sum();

                var totalMilkSold = db.MilkSale
                                     .Where(s => s.FarmId == farmId && s.CattleType == type)
                                     .Select(s => (decimal?)s.Quantity) // Cast to nullable decimal to handle no records case
                                     .DefaultIfEmpty(0).Sum();

                var remainingMilk = totalMilkCollected - totalMilkSold.GetValueOrDefault();

                return Request.CreateResponse(HttpStatusCode.OK, new { TotalMilk = remainingMilk });
            }
        }


        //[HttpGet]
        //public HttpResponseMessage CattleTypeTotalMilk(int farmId, string type)
        //{
        //    if (type == "TotalMilk")
        //    {
        //        var totalMilkCollected = (from m in db.MilkCollection
        //                                  join c in db.Cattle on m.CattleTag equals c.Tag
        //                                  where c.FarmID == farmId
        //                                  select m.TotalMilk).Sum();

        //        var totalMilkSold = db.MilkSale
        //                             .Where(s => s.FarmId == farmId)
        //                             .Sum(s => s.Quantity);

        //        var remainingMilk = totalMilkCollected - totalMilkSold;

        //        return Request.CreateResponse(HttpStatusCode.OK, new { TotalMilk = remainingMilk });
        //    }
        //    else
        //    {
        //        var totalMilkCollected = (from m in db.MilkCollection
        //                                  join c in db.Cattle on m.CattleTag equals c.Tag
        //                                  where c.FarmID == farmId && c.CattleType == type
        //                                  select m.TotalMilk).Sum();

        //        var totalMilkSold = db.MilkSale
        //                             .Where(s => s.FarmId == farmId && s.CattleType == type)
        //                             .Sum(s => s.Quantity);

        //        var remainingMilk = totalMilkCollected - totalMilkSold;

        //        return Request.CreateResponse(HttpStatusCode.OK, new { TotalMilk = remainingMilk });
        //    }
        //}


        ////--------------------------------get Avg Milk of last 5 days
        //[HttpGet]
        //public HttpResponseMessage SearchAvgCattleMilkByTagAndFarm(string tag, int farmid)
        //{
        //    if (string.IsNullOrEmpty(tag))
        //    {
        //        return Request.CreateResponse(HttpStatusCode.BadRequest, "Cattle tag is required.");
        //    }

        //    // Find the cattle with the specified tag and farm ID
        //    var cattle = db.Cattle.FirstOrDefault(c => c.Tag == tag && c.FarmID == farmid);
        //    if (cattle == null)
        //    {
        //        return Request.CreateResponse(HttpStatusCode.NotFound, "Cattle not found.");
        //    }

        //    // Get the current date
        //    var currentDate = DateTime.Now;

        //    // Calculate the date 5 days ago
        //    var startDate = currentDate.AddDays(-5);

        //    // Retrieve milk collection records for the last 5 days for the given cattle tag and farm ID
        //    var milkCollections = db.MilkCollection
        //        .Where(m => m.CattleTag == tag && m.FarmId == farmid && DateTime.Parse(m.Date) >= startDate)
        //        .ToList();

        //    if (!milkCollections.Any())
        //    {
        //        return Request.CreateResponse(HttpStatusCode.NotFound, "No milk collection records found for the last 5 days.");
        //    }

        //    // Calculate the average milk
        //    var avgMilk = milkCollections.Average(m => m.TotalMilk) ?? 0;

        //    // Create the view model
        //    var viewAvgMilk = new AverageMilkOfCattle
        //    {
        //        Tag = tag,
        //        CattleType = cattle.CattleType,
        //        AvgMilk = avgMilk
        //    };

        //    return Request.CreateResponse(HttpStatusCode.OK, viewAvgMilk);
        //}


        //[HttpGet]
        //public HttpResponseMessage GetAverageMilkForAllCattle(int farmid)
        //{
        //    // Find all cattle for the specified farm ID
        //    var cattleList = db.Cattle.Where(c => c.FarmID == farmid).ToList();

        //    if (!cattleList.Any())
        //    {
        //        return Request.CreateResponse(HttpStatusCode.NotFound, "No cattle found for the specified farm ID.");
        //    }

        //    // Get the current date
        //    var currentDate = DateTime.Now;

        //    // Calculate the date 5 days ago
        //    var startDate = currentDate.AddDays(-5);

        //    // List to hold average milk data for each cattle
        //    var averageMilkList = new List<AverageMilkOfCattle>();

        //    foreach (var cattle in cattleList)
        //    {
        //        // Retrieve milk collection records for the last 5 days for the current cattle
        //        var milkCollections = db.MilkCollection
        //            .Where(m => m.CattleTag == cattle.Tag && m.FarmId == farmid && DateTime.Parse(m.Date) >= startDate)
        //            .ToList();

        //        // Calculate the average milk
        //        var avgMilk = milkCollections.Any() ? milkCollections.Average(m => m.TotalMilk) ?? 0 : 0;

        //        // Add the average milk data to the list
        //        averageMilkList.Add(new AverageMilkOfCattle
        //        {
        //            Tag = cattle.Tag,
        //            CattleType = cattle.CattleType,
        //            AvgMilk = avgMilk
        //        });
        //    }

        //    return Request.CreateResponse(HttpStatusCode.OK, averageMilkList);
        //}

        //--------------below is the code of avg milk of cattle for all time
        [HttpGet]
        public HttpResponseMessage GetAverageMilkForSpecificCattleTag(string tag, int farmid)
        {
            if (string.IsNullOrEmpty(tag))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Cattle tag is required.");
            }

            // Find the cattle with the specified tag and farm ID
            var cattle = db.Cattle.FirstOrDefault(c => c.Tag == tag && c.FarmID == farmid);
            if (cattle == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "Cattle not found.");
            }

            // Retrieve all milk collection records for the specified cattle tag
            var milkCollections = db.MilkCollection
                .Where(m => m.CattleTag == tag && m.FarmId == farmid)
                .ToList();

            if (!milkCollections.Any())
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "No milk records found for the specified cattle tag.");
            }

            // Calculate the average milk
            var avgMilk = milkCollections.Average(m => m.TotalMilk) ?? 0;
            var avgMilkFormatted = Math.Round(avgMilk, 2);

            // Create the view model
            var viewAvgMilk = new AverageMilkOfCattle
            {
                Tag = tag,
                CattleType = cattle.CattleType,
                AvgMilk = avgMilkFormatted
            };

            return Request.CreateResponse(HttpStatusCode.OK, viewAvgMilk);
        }





        //[HttpGet]
        //public HttpResponseMessage GetAverageMilkForAllCattleOfAllTime(int farmid)
        //{
        //    // Find all cattle for the specified farm ID
        //    var cattleList = db.Cattle.Where(c => c.FarmID == farmid).ToList();

        //    if (!cattleList.Any())
        //    {
        //        return Request.CreateResponse(HttpStatusCode.NotFound, "No cattle found for the specified farm ID.");
        //    }

        //    // List to hold average milk data for each cattle
        //    var averageMilkList = new List<AverageMilkOfCattle>();

        //    foreach (var cattle in cattleList)
        //    {
        //        // Retrieve all milk collection records for the current cattle
        //        var milkCollections = db.MilkCollection
        //            .Where(m => m.CattleTag == cattle.Tag && m.FarmId == farmid)
        //            .ToList();

        //        // Calculate the average milk
        //        var avgMilk = milkCollections.Any() ? milkCollections.Average(m => m.TotalMilk) ?? 0 : 0;
        //        var avgMilkFormatted = Math.Round(avgMilk, 2);

        //        // Add the average milk data to the list
        //        averageMilkList.Add(new AverageMilkOfCattle
        //        {
        //            Tag = cattle.Tag,
        //            CattleType = cattle.CattleType,
        //            AvgMilk = avgMilkFormatted
        //        });
        //    }

        //    return Request.CreateResponse(HttpStatusCode.OK, averageMilkList);
        //}

        [HttpGet]
        public HttpResponseMessage GetAverageMilkForAllCattleOfAllTime(int farmid)
        {
            // Find all cattle for the specified farm ID
            var cattleList = db.Cattle.Where(c => c.FarmID == farmid && c.Status == "Alive").ToList();

            if (!cattleList.Any())
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "No cattle found for the specified farm ID.");
            }

            // List to hold average milk data for each cattle
            var averageMilkList = new List<AverageMilkOfCattle>();

            foreach (var cattle in cattleList)
            {
                // Retrieve all milk collection records for the current cattle
                var milkCollections = db.MilkCollection
                    .Where(m => m.CattleTag == cattle.Tag && m.FarmId == farmid)
                    .ToList();

                // Calculate the average milk
                var avgMilk = milkCollections.Any() ? milkCollections.Average(m => m.TotalMilk) ?? 0 : 0;
                var avgMilkFormatted = Math.Round(avgMilk, 2);

                // Add the average milk data to the list
                averageMilkList.Add(new AverageMilkOfCattle
                {
                    Tag = cattle.Tag,
                    CattleType = cattle.CattleType,
                    AvgMilk = avgMilkFormatted
                });
            }

            return Request.CreateResponse(HttpStatusCode.OK, averageMilkList);
        }

        [HttpPost]
        public HttpResponseMessage MilkSale(List<MilkSale> milksale)
        {
            try
            {
                // Assuming "db" is your database context
                db.MilkSale.AddRange(milksale);
                db.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.OK, $"{milksale.Count} milk added successfully");
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, $"Error adding milk Sale: {ex.Message}");
            }
        }





        [HttpGet]
        public HttpResponseMessage GetAllSaledMilk()
        {
            List<MilkSale> food = new List<MilkSale>();
            return Request.CreateResponse(HttpStatusCode.OK, db.MilkSale);
        }

        //this below function only calculate cattle type milk not total milk
        //[HttpGet]
        //public HttpResponseMessage CattleTypeTotalMilk(int farmId, string type)
        //{
        //    // Query to calculate total milk for a given farmId and cattle type
        //    var result = (from c in db.Cattle
        //                  join m in db.MilkCollection on c.Tag equals m.CattleTag
        //                  where c.FarmID == farmId && c.CattleType == type
        //                  group m by c.CattleType into g
        //                  select new { TotalMilk = g.Sum(x => x.TotalMilk) }
        //      ).SingleOrDefault();

        //    return Request.CreateResponse(HttpStatusCode.OK, result);

        //}


        //[HttpGet]
        //public HttpResponseMessage CattleTypeTotalMilk(int farmId, string type)
        //{
        //    // Query to calculate total milk for a given farmId and cattle type

        //    var result = (from c in db.Cattle
        //                  join m in db.MilkCollection on c.Tag equals m.CattleTag
        //                  where c.FarmID == farmId && c.CattleType == type
        //                  group m by c.CattleType into g
        //                  select new { TotalMilk = g.Sum(x => x.TotalMilk) }
        //                 ).SingleOrDefault();

        //    return Request.CreateResponse(HttpStatusCode.OK, result);
        //}

        //[HttpGet]
        //public HttpResponseMessage CattleTotalMilk()
        //{
        //    var request = HttpContext.Current.Request;
        //    string type = request["type"];
        //    //var result = (from c in db.Cattle
        //    //              join m in db.MilkCollection
        //    //              on c.ID equals m.CattleID
        //    //              select new { TotalMilk = m.TotalMilk, Type = c.CattleCategory }
        //    //           ).Where(n => n.Type == type
        //    //           ).Sum(n=>n.TotalMilk);

        //    //OR

        //    var result = (from c in db.Cattle
        //                  join m in db.MilkCollection on c.Tag equals m.CattleTag
        //                  where c.CattleType == type
        //                  group m by c.CattleType into g
        //                  select new { TotalMilk = g.Sum(x => x.TotalMilk) }
        //           ).SingleOrDefault();

        //    return Request.CreateResponse(HttpStatusCode.OK, result);
        //}
    }
}
