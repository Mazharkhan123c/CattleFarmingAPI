using CattleFarmingAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using CattleFarmingAPI.Models;
using System.Collections;
using System.Data.Entity.Migrations;
using System.Diagnostics;
using System.Xml.Linq;
namespace CattleFarmingAPI.Controllers
{
    public class FoodController : ApiController
    {
        Cattle_Farming_ManagementEntities db = new Cattle_Farming_ManagementEntities();

        //Funtion to get Record of All Food
        [HttpGet]
        public HttpResponseMessage GetAllFood()
        {
          //  var food = db.FoodStock.Where(s => s.FarmId == farmid).ToList();

             List<FoodStock> food = new List<FoodStock>();
            return Request.CreateResponse(HttpStatusCode.OK, db.FoodStock);
        }

        //[HttpGet]
        //public HttpResponseMessage GetAllFood(int farmid)
        //{
        //    var food=db.FoodStock.Where(s=> s.FarmId==farmid).ToList();

        //   // List<FoodStock> food = new List<FoodStock>();
        //    return Request.CreateResponse(HttpStatusCode.OK, food);
        //}

        [HttpGet]
        public HttpResponseMessage GetRemainingFood()
        {
            List<FoodCalculate> food = new List<FoodCalculate>();
            return Request.CreateResponse(HttpStatusCode.OK, db.FoodCalculate);
        }


        [HttpPost]
        public HttpResponseMessage SaveFoodStock(FoodStock c)
        {
            try
            {
                // Assuming "db" is your database context
                db.FoodStock.Add(c);
                db.SaveChanges();

                // Find the existing food item in FoodCalculate table
                var existingFoodItem = db.FoodCalculate.SingleOrDefault(f => f.Item == c.Item && f.FarmId == c.FarmId);

                if (existingFoodItem != null)
                {
                    // Update the existing food item
                    existingFoodItem.Price = c.Price;
                    existingFoodItem.Quantity += c.Quantity;
                    existingFoodItem.Date = c.Date;
                }
                else
                {
                    // Add a new food item if it doesn't exist
                    FoodCalculate fc = new FoodCalculate
                    {
                        Item = c.Item,
                        Price = c.Price,
                        Quantity = c.Quantity,
                        Date = c.Date,
                        FarmId = c.FarmId
                    };
                    db.FoodCalculate.Add(fc);
                }

                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, $"Food {c.Item} saved successfully");
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, $"Error in saving food: {ex.Message}");
            }
        }


        //[HttpPost]
        //public HttpResponseMessage SaveFoodStock(FoodStock c)
        //{
        //    try
        //    {
        //        // Assuming "db" is your database context
        //        db.FoodStock.Add(c);
        //        db.SaveChanges();
        //       var foodItem = db.FoodCalculate.Where(f => f.Item == c.Item).SingleOrDefault();
        //        if (foodItem != null) 
        //        {
        //            var foodQuantities = db.FoodCalculate
        // .Where(f => f.Quantity > 0)
        // .Select(f => f.Quantity)
        // .SingleOrDefault();

        //            FoodCalculate fc = new FoodCalculate();
        //           fc.Item = c.Item;
        //            fc.Price = c.Price;
        //            fc.Quantity = c.Quantity+ foodQuantities;
        //            fc.Date = c.Date;
        //            fc.FarmId = c.FarmId;

        //            db.FoodCalculate.AddOrUpdate(fc);
        //        }


        //        db.SaveChanges();
        //        return Request.CreateResponse(HttpStatusCode.OK, $"Food {c.Item} Save successfully");
        //    }
        //    catch (Exception ex)
        //    {
        //        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, $"Error in Saving Food: {ex.Message}");
        //    }
        //}



        //---------------------------------theses below two functions run on postman but not in flutter

        //[HttpPost]
        //[Route("api/Food/SaveConsumeFood")]

        //public HttpResponseMessage SaveConsumeFood(string type, string food, string date, int farmId)
        //{
        //    try
        //    {
        //        if (type == "Cow And Buffalo")
        //        {
        //            var fdata = db.FoodCalculate.Where(f => f.Item == food).First();
        //            var clist = db.Cattle.Where(n => n.CattleType == "Cow" || n.CattleType == "Buffalo" && n.Status == "Alive").ToList();
        //            int foodQty = int.Parse(fdata.Quantity.ToString());
        //            int totalCattles = clist.Count();
        //            if (totalCattles > 0)
        //            {
        //                int reqFood = totalCattles * 15;
        //                if (foodQty >= reqFood)
        //                {
        //                    fdata.Quantity = fdata.Quantity - reqFood;
        //                    db.SaveChanges();
        //                    foreach (var c in clist)
        //                    {
        //                        FoodConsume fc = new FoodConsume();
        //                        {

        //                            fc.FoodItem = food;
        //                            fc.CattleTyp = c.CattleType;
        //                            fc.CattleTag = c.Tag;
        //                            fc.Date = date;
        //                            fc.Quantity = 15;
        //                            fc.FarmId = 1;

        //                        }
        //                        db.FoodConsume.Add(fc);
        //                        db.SaveChanges();
        //                    }

        //                }
        //                else
        //                {
        //                    return Request.CreateResponse(HttpStatusCode.OK, $"Food Quantity is Low ");
        //                }
        //            }

        //        }

        //        else if (type == "Goat")
        //        {
        //            var fdata = db.FoodCalculate.Where(f => f.Item == food).First();
        //            var clist = db.Cattle.Where(n => n.CattleType == "Goat" && n.Status == "Alive").ToList();
        //            int foodQty = int.Parse(fdata.Quantity.ToString());
        //            int totalCattles = clist.Count();
        //            if (totalCattles > 0)
        //            {
        //                int reqFood = totalCattles * 3;
        //                if (foodQty >= reqFood)
        //                {
        //                    fdata.Quantity = fdata.Quantity - reqFood;
        //                    db.SaveChanges();
        //                    foreach (var c in clist)
        //                    {
        //                        FoodConsume fc = new FoodConsume();
        //                        {

        //                            fc.FoodItem = food;
        //                            fc.CattleTyp = c.CattleType;
        //                            fc.CattleTag = c.Tag;
        //                            fc.Date = date;
        //                            fc.Quantity = 3;
        //                            fc.FarmId = farmId;

        //                        }
        //                        db.FoodConsume.Add(fc);
        //                        db.SaveChanges();
        //                    }
        //                }
        //                else
        //                {
        //                    return Request.CreateResponse(HttpStatusCode.OK, $"Food Quantity is Low ");
        //                }
        //            }
        //        }
        //        return Request.CreateResponse(HttpStatusCode.OK, $"Consume Food added successfully");
        //    }
        //    catch (Exception ex)
        //    {
        //        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, $"Error in Saving Food: {ex.Message}");
        //    }


        //}



        //[HttpPost]
        //[Route("api/Food/ConsumeFoodSpecialCase")]
        //public HttpResponseMessage ConsumeFoodSpecialCase(string tag, string food, string date,double quantity, int farmId)
        //{
        //    try
        //    {
        //        var fdata = db.FoodCalculate.Where(f => f.Item == food).First();
        //        var catle = db.Cattle.Where(n => n.Tag == tag && n.Status == "Alive").SingleOrDefault();
        //        int foodQty = int.Parse(fdata.Quantity.ToString());
        //       // int totalCattles = clist.Count();


        //            if (foodQty >= quantity)
        //            {
        //                fdata.Quantity = fdata.Quantity -decimal.Parse( quantity.ToString());
        //                db.SaveChanges();

        //                    FoodConsume fc = new FoodConsume();
        //                    {

        //                        fc.FoodItem = food;
        //                        fc.CattleTyp = catle.CattleType;
        //                        fc.CattleTag = tag;
        //                        fc.Date = date;
        //                        fc.Quantity = decimal.Parse( quantity.ToString());
        //                        fc.FarmId = farmId;

        //                    }
        //                    db.FoodConsume.Add(fc);
        //                    db.SaveChanges();

        //            }
        //            else
        //            {
        //                return Request.CreateResponse(HttpStatusCode.OK, $"Food Quantity is Low ");
        //            }

        //        return Request.CreateResponse(HttpStatusCode.OK, "Food Consume in Special Case Save Succesfully");
        //    }
        //    catch (Exception ex)
        //    {
        //        return Request.CreateResponse(HttpStatusCode.InternalServerError, $"Error in Saving Special Food Consume: {ex.Message}");
        //    }
        //}



        [HttpPost]
        [Route("api/Food/SaveConsumeFood")]
        public HttpResponseMessage SaveConsumeFood([FromBody] dynamic data)
        {
            try
            {
                string type = data.cattleType;
                string food = data.fodderType;
                string date = data.date;
                int farmId = data.farmId;

                if (type == "Cow And Buffalo")
                {
                    var fdata = db.FoodCalculate.Where(f => f.Item == food).First();
                    var clist = db.Cattle.Where(n => (n.CattleType == "Cow" || n.CattleType == "Buffalo") && n.Status == "Alive").ToList();
                    int foodQty = int.Parse(fdata.Quantity.ToString());
                    int totalCattles = clist.Count();
                    if (totalCattles > 0)
                    {
                        int reqFood = totalCattles * 15;
                        if (foodQty >= reqFood)
                        {
                            fdata.Quantity = fdata.Quantity - reqFood;
                            db.SaveChanges();
                            foreach (var c in clist)
                            {
                                FoodConsume fc = new FoodConsume();
                                {
                                    fc.FoodItem = food;
                                    fc.CattleTyp = c.CattleType;
                                    fc.CattleTag = c.Tag;
                                    fc.Date = date;
                                    fc.Quantity = 15;
                                    fc.FarmId = 1;
                                }
                                db.FoodConsume.Add(fc);
                                db.SaveChanges();
                            }
                        }
                        else
                        {
                            return Request.CreateResponse(HttpStatusCode.OK, $"Food Quantity is Low ");
                        }
                    }
                }
                else if (type == "Goat")
                {
                    var fdata = db.FoodCalculate.Where(f => f.Item == food).First();
                    var clist = db.Cattle.Where(n => n.CattleType == "Goat" && n.Status == "Alive").ToList();
                    int foodQty = int.Parse(fdata.Quantity.ToString());
                    int totalCattles = clist.Count();
                    if (totalCattles > 0)
                    {
                        int reqFood = totalCattles * 3;
                        if (foodQty >= reqFood)
                        {
                            fdata.Quantity = fdata.Quantity - reqFood;
                            db.SaveChanges();
                            foreach (var c in clist)
                            {
                                FoodConsume fc = new FoodConsume();
                                {
                                    fc.FoodItem = food;
                                    fc.CattleTyp = c.CattleType;
                                    fc.CattleTag = c.Tag;
                                    fc.Date = date;
                                    fc.Quantity = 3;
                                    fc.FarmId = farmId;
                                }
                                db.FoodConsume.Add(fc);
                                db.SaveChanges();
                            }
                        }
                        else
                        {
                            return Request.CreateResponse(HttpStatusCode.OK, $"Food Quantity is Low ");
                        }
                    }
                }
                return Request.CreateResponse(HttpStatusCode.OK, $"Consume Food added successfully");
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, $"Error in Saving Food: {ex.Message}");
            }
        }

        [HttpPost]
        [Route("api/Food/ConsumeFoodSpecialCase")]
        public HttpResponseMessage ConsumeFoodSpecialCase([FromBody] dynamic data)
        {
            try
            {
                string tag = data.tag;
                string food = data.foodType;
                string date = data.date;
                double quantity = data.quantity;
                int farmId = data.farmId;

                var fdata = db.FoodCalculate.Where(f => f.Item == food).First();
                var catle = db.Cattle.Where(n => n.Tag == tag && n.Status == "Alive").SingleOrDefault();
                int foodQty = int.Parse(fdata.Quantity.ToString());

                if (foodQty >= quantity)
                {
                    fdata.Quantity = fdata.Quantity - decimal.Parse(quantity.ToString());
                    db.SaveChanges();

                    FoodConsume fc = new FoodConsume();
                    {
                        fc.FoodItem = food;
                        fc.CattleTyp = catle.CattleType;
                        fc.CattleTag = tag;
                        fc.Date = date;
                        fc.Quantity = decimal.Parse(quantity.ToString());
                        fc.FarmId = farmId;
                    }
                    db.FoodConsume.Add(fc);
                    db.SaveChanges();
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, $"Food Quantity is Low ");
                }
                return Request.CreateResponse(HttpStatusCode.OK, "Food Consume in Special Case Save Successfully");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, $"Error in Saving Special Food Consume: {ex.Message}");
            }
        }




        //Function to Insert Record of Food

        //[HttpPost]
        //public HttpResponseMessage AddFood()
        //{
        //    HttpRequest form = HttpContext.Current.Request;
        //    Food fd = new Food();
        //    fd.Quantity = form["Quantity"];

        //    fd.Item = form["Item"];
        //    fd.Price = int.Parse(form["Price"]);
        //    fd.ExpiryDate = DateTime.Parse(form["ExpiryDate"]);
        //    fd.CattleTag = form["CattleTag"];           
        //    db.Food.Add(fd);
        //    db.SaveChanges();
        //    return Request.CreateResponse(HttpStatusCode.OK, $"Food Added {fd.Item}");
        //}

        //[HttpPost]
        //public HttpResponseMessage UpdateFood()
        //{
        //    HttpRequest form = HttpContext.Current.Request;
        //    int id = int.Parse(form["id"]);
        //    Food fd = db.Food.Where(pd => pd.ID == id).FirstOrDefault();
        //    fd.Quantity = form["Quantity"];
        //    fd.Date = form["Date"] ;
        //    fd.Item = form["Item"];
        //    fd.Price = int.Parse(form["Price"]);
        //    fd.ExpiryDate = DateTime.Parse(form["ExpiryDate"]);
        //    fd.CattleTag = form["CattleTag"];

        //    db.SaveChanges();
        //    return Request.CreateResponse(HttpStatusCode.OK, "Food Updated");
        //}

        //[HttpPost]
        //public HttpResponseMessage DeleteProduct()
        //{
        //    HttpRequest form = HttpContext.Current.Request;
        //    int id = int.Parse(form["id"]);
        //    Food fd = db.Food.Where(pd => pd.ID == id).FirstOrDefault();
        //    db.Food.Remove(fd);
        //    db.SaveChanges();
        //    return Request.CreateResponse(HttpStatusCode.OK, "Food Deleted");
        //}

    }
}
