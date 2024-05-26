using CattleFarmingAPI.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
//using CattleFarmingAPI.Models;

namespace CattleFarmingAPI.Controllers
{
    public class CattleController : ApiController
    {

        Cattle_Farming_ManagementEntities db = new Cattle_Farming_ManagementEntities();



        //[HttpGet]
        //public HttpResponseMessage GetCattleById(int id)
        //{
        //    Cattle cattle = db.Cattle.Where(f => f.ID == id).FirstOrDefault();

        //    return Request.CreateResponse(HttpStatusCode.OK, cattle);
        //}




        //[HttpPost]
        //public IHttpActionResult uploadImage()
        //{
        //    var form = HttpContext.Current.Request.Form;
        //    string DeptName = form["name"];

        //    var files = HttpContext.Current.Request.Files;
        //    //string dateTimeStamp = DateTime.Now.ToString();
        //    string path = HttpContext.Current.Server.MapPath("~/Images");
        //    if (!Directory.Exists(path))
        //    {
        //        Directory.CreateDirectory(path);
        //    }
        //    int rowEffectd = 0;
        //    string fileName = null;
        //    for (int i = 0; i < files.Count; i++)
        //    {
        //        fileName = / dateTimeStamp +/ files[i].FileName;
        //        files[i].SaveAs(path + "/" + fileName);
        //        rowEffectd = new Department().insert(new Department { name = DeptName, imageData = fileName });
        //    }



        //    return Ok("Image uploaded" + " Name:" + DeptName + "Row Effected:" + rowEffectd);
        //}
      

        [HttpPost]

        public HttpResponseMessage SaveCattle(Cattle c)

        {
            try
            {
                var catle = db.Cattle.Where(s => s.Tag == c.Tag).FirstOrDefault();
                if (catle != null)
                {

                    return Request.CreateResponse(HttpStatusCode.OK, "Already exsist");
                }
                else {
                    db.Cattle.Add(c);
                    db.SaveChanges();

                    return Request.CreateResponse(HttpStatusCode.OK, $"Cattle {c.Tag} added successfully");
                }
                   
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, $"Error adding Cattle: {ex.Message}");
            }
        }
        //   [HttpPost]
        //public HttpResponseMessage SaveCattle()
        //{
        //    HttpRequest form = HttpContext.Current.Request;

        //    // Create a new Cattle instance
        //    Cattle c = new Cattle();
        //    c.Weight = decimal.Parse(form["Weight"]);
        //    c.Gender = form["Gender"];


        //    c.Status = form["Status"];
        //    c.FarmID = int.Parse(form["FarmID"]);

        // Handle image upload
        /*   HttpPostedFile imageFile = form.Files["Image"];
           if (imageFile != null && imageFile.ContentLength > 0)
           {
               // Specify the directory to save the images
               string uploadDirectory = HttpContext.Current.Server.MapPath("~/Images/");
               // Ensure the directory exists
               if (!Directory.Exists(uploadDirectory))
               {
                   Directory.CreateDirectory(uploadDirectory);
               }

               // Generate a unique filename for the image
               string fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
               // Combine the directory and filename to get the full path
               string filePath = Path.Combine(uploadDirectory, fileName);

               // Save the image file to the server
               imageFile.SaveAs(filePath);

               // Store the image path in the Cattle entity
               c.Image = "~/Images/" + fileName;
           }*/

        // Add the Cattle to the database
        //       db.Cattle.Add(c);
        //      db.SaveChanges();

        //            return Request.CreateResponse(HttpStatusCode.OK, $"Cattle Added {c.ID}");
        //      }
        //[HttpPost]
        //public HttpResponseMessage SaveCattle()
        //{
        //    HttpRequest form = HttpContext.Current.Request;
        //    Cattle c = new Cattle();
        //    c.Weight = int.Parse( form["Weight"]);
        //    c.Gender = form["Gender"]; ;
        //    c.CattleCategory = form["CattleCategory"];
        //    c.Age = int.Parse(form["Age"]);
        //    c.Status = form["Status"];
        //    c.Image = form["Image"];
        //    db.Cattle.Add(c);
        //    db.SaveChanges();
        //    return Request.CreateResponse(HttpStatusCode.OK, $"Cattle Added {c.ID}");
        //}


        [HttpPost]
        public HttpResponseMessage UpdateCattle()
        {
            HttpRequest form = HttpContext.Current.Request;
            String id = form["id"];
            Cattle c = db.Cattle.Where(pd => pd.Tag == id).FirstOrDefault();
            c.Weight = int.Parse(form["Weight"]);
            c.Gender = form["Gender"];

            c.Status = form["Status"];


            db.SaveChanges();
            return Request.CreateResponse(HttpStatusCode.OK, "Cattle Updated");
        }

        [HttpPost]
        public HttpResponseMessage DeleteCattle()
        {
            HttpRequest form = HttpContext.Current.Request;
            String id = form["id"];
            Cattle c = db.Cattle.Where(ct => ct.Tag == id).FirstOrDefault();
            db.Cattle.Remove(c);
            db.SaveChanges();
            return Request.CreateResponse(HttpStatusCode.OK, "Cattle Deleted");
        }
        //[HttpGet]
        //public HttpResponseMessage ViewAllCatel()
        //{
        //    //List<ViewCattle> vc=(from c in db.Cattle
        //    //                    join v in db.InjectedVaccination
        //    //                    on c.ID equals v.CattleID 
        //    //                    select new ViewCattle {ID =c.ID, Weight= c.Weight, isVaccinated=v.CattleID ==0 ? "Not Vaccinated ":"Vaccinated" }).ToList();

        //    var cattles = db.Cattle.ToList();
        //    var vcattle = db.InjectedVaccination.ToList();
        //    List<ViewCattle> clist = new List<ViewCattle>();
        //    foreach (var a in cattles)
        //    {
        //        var isVaccinated = vcattle.Any(v => v.CattleTag == a.Tag);

        //        if (isVaccinated)
        //        {
        //            clist.Add(new ViewCattle { Tag = a.Tag, Weight = a.Weight, CattleType = a.CattleType, isVaccinated = "Vaccinated" });
        //        }
        //        else
        //        {
        //            clist.Add(new ViewCattle { Tag = a.Tag, Weight = a.Weight, CattleType = a.CattleType, isVaccinated = "Not Vaccinated" });
        //        }
        //    }
        //    return Request.CreateResponse(HttpStatusCode.OK, clist);
        //}






        //[HttpGet]
        //public HttpResponseMessage FilterAllCattles(string status, string type, string isVaccinate)
        //{
        //    List<Cattle> clist = db.Cattle.ToList();
        //    List<ViewCattle> filterCattleList = new List<ViewCattle>();
        //    List<InjectedVaccination> filterVaccList = db.InjectedVaccination.ToList();

        //    // Apply status filter if provided
        //    if (!string.IsNullOrEmpty(status))
        //    {
        //        clist = clist.Where(n => n.Status == status).ToList();
        //    }

        //    // Apply type filter if provided
        //    if (!string.IsNullOrEmpty(type))
        //    {
        //        clist = clist.Where(n => n.CattleType == type).ToList();
        //    }

        //    // Process vaccination status filter
        //    if (!string.IsNullOrEmpty(isVaccinate))
        //    {
        //        foreach (var cattle in clist)
        //        {
        //            bool isVaccinated = filterVaccList.Any(v => v.CattleTag == cattle.Tag);
        //            if (isVaccinate == "Vaccinated" && isVaccinated)
        //            {
        //                filterCattleList.Add(new ViewCattle { Tag = cattle.Tag, Weight = cattle.Weight, CattleType = cattle.CattleType, isVaccinated = "Vaccinated" });
        //            }
        //            else if (isVaccinate == "Not Vaccinated" && !isVaccinated)
        //            {
        //                filterCattleList.Add(new ViewCattle { Tag = cattle.Tag, Weight = cattle.Weight, CattleType = cattle.CattleType, isVaccinated = "Not Vaccinated" });
        //            }
        //        }
        //    }
        //    else
        //    {
        //        // If no vaccination status filter is provided, include all based on the previous filters
        //        foreach (var cattle in clist)
        //        {
        //            bool isVaccinated = filterVaccList.Any(v => v.CattleTag == cattle.Tag);
        //            filterCattleList.Add(new ViewCattle { Tag = cattle.Tag, Weight = cattle.Weight, CattleType = cattle.CattleType, isVaccinated = isVaccinated ? "Vaccinated" : "Not Vaccinated" });
        //        }
        //    }

        //    return Request.CreateResponse(HttpStatusCode.OK, filterCattleList);
        //}




        //[HttpGet]
        //public HttpResponseMessage FarmAllCattles(string status, string type, string isVaccinate , int id)
        //{
        //    List<Cattle> clist = db.Cattle.Where(s=> s.FarmID==id).ToList();
        //    List<ViewCattle> filterCattleList = new List<ViewCattle>();
        //    List<InjectedVaccination> filterVaccList = db.InjectedVaccination.ToList();

        //    // Apply status filter if provided
        //    if (!string.IsNullOrEmpty(status))
        //    {
        //        clist = clist.Where(n => n.Status == status).ToList();
        //    }

        //    // Apply type filter if provided
        //    if (!string.IsNullOrEmpty(type))
        //    {
        //        clist = clist.Where(n => n.CattleType == type).ToList();
        //    }

        //    // Process vaccination status filter
        //    if (!string.IsNullOrEmpty(isVaccinate))
        //    {
        //        foreach (var cattle in clist)
        //        {
        //            bool isVaccinated = filterVaccList.Any(v => v.CattleTag == cattle.Tag);
        //            if (isVaccinate == "Vaccinated" && isVaccinated)
        //            {
        //                filterCattleList.Add(new ViewCattle { Tag = cattle.Tag, Weight = cattle.Weight, CattleType = cattle.CattleType, isVaccinated = "Vaccinated" });
        //            }
        //            else if (isVaccinate == "Not Vaccinated" && !isVaccinated)
        //            {
        //                filterCattleList.Add(new ViewCattle { Tag = cattle.Tag, Weight = cattle.Weight, CattleType = cattle.CattleType, isVaccinated = "Not Vaccinated" });
        //            }
        //        }
        //    }
        //    else
        //    {
        //        // If no vaccination status filter is provided, include all based on the previous filters
        //        foreach (var cattle in clist)
        //        {
        //            bool isVaccinated = filterVaccList.Any(v => v.CattleTag == cattle.Tag);
        //            filterCattleList.Add(new ViewCattle { Tag = cattle.Tag, Weight = cattle.Weight, CattleType = cattle.CattleType, isVaccinated = isVaccinated ? "Vaccinated" : "Not Vaccinated" });
        //        }
        //    }

        //    return Request.CreateResponse(HttpStatusCode.OK, filterCattleList);
        //}


        //[Route("api/Cattle/SearchCattleByTag")]
        //[HttpGet]
        //public HttpResponseMessage SearchCattleByTag(string tag)
        //{
        //    if (string.IsNullOrEmpty(tag))
        //    {
        //        return Request.CreateResponse(HttpStatusCode.BadRequest, "Cattle tag is required.");
        //    }

        //    // Find the cattle with the specified tag
        //    var cattle = db.Cattle.FirstOrDefault(c => c.Tag == tag);
        //    if (cattle == null)
        //    {
        //        return Request.CreateResponse(HttpStatusCode.NotFound, "Cattle not found.");
        //    }

        //    // Check if the cattle is vaccinated
        //    bool isVaccinated = db.InjectedVaccination.Any(v => v.CattleTag == cattle.Tag);

        //    // Create the view model
        //    var viewCattle = new ViewCattle
        //    {
        //        Tag = cattle.Tag,
        //        Weight = cattle.Weight,
        //        CattleType = cattle.CattleType,
        //        isVaccinated = isVaccinated ? "Vaccinated" : "Not Vaccinated"
        //    };

        //    return Request.CreateResponse(HttpStatusCode.OK, viewCattle);
        //}




    //      [Route("api/Cattle/SearchCattleByTagAndFarm")]
        [HttpGet]
        public HttpResponseMessage SearchCattleByTagAndFarm(string tag, int farmid)
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

            // Check if the cattle is vaccinated
            bool isVaccinated = db.InjectedVaccination.Any(v => v.CattleTag == cattle.Tag);

            // Create the view model
            var viewCattle = new ViewCattle
            {
                Tag = cattle.Tag,
                Weight = cattle.Weight,
                CattleType = cattle.CattleType,
                isVaccinated = isVaccinated ? "Vaccinated" : "Not Vaccinated"
            };

            return Request.CreateResponse(HttpStatusCode.OK, viewCattle);
        }



        [Route("api/Cattle/FarmAllCattles")]
        [HttpGet]
        public HttpResponseMessage FarmAllCattles(string status, string type, string isVaccinate, int id)
        {
            // Filter cattle based on the farm ID
            List<Cattle> clist = db.Cattle.Where(s => s.FarmID == id).ToList();
            List<ViewCattle> filterCattleList = new List<ViewCattle>();
            List<InjectedVaccination> filterVaccList = db.InjectedVaccination.ToList();

            // Apply status filter if provided
            if (!string.IsNullOrEmpty(status))
            {
                clist = clist.Where(n => n.Status == status).ToList();
            }

            // Apply type filter if provided
            if (!string.IsNullOrEmpty(type))
            {
                clist = clist.Where(n => n.CattleType == type).ToList();
            }

            // Process vaccination status filter
            if (!string.IsNullOrEmpty(isVaccinate))
            {
                foreach (var cattle in clist)
                {
                    bool isVaccinated = filterVaccList.Any(v => v.CattleTag == cattle.Tag);
                    if (isVaccinate == "Vaccinated" && isVaccinated)
                    {
                        filterCattleList.Add(new ViewCattle { Tag = cattle.Tag, Weight = cattle.Weight, CattleType = cattle.CattleType, isVaccinated = "Vaccinated" });
                    }
                    else if (isVaccinate == "Not Vaccinated" && !isVaccinated)
                    {
                        filterCattleList.Add(new ViewCattle { Tag = cattle.Tag, Weight = cattle.Weight, CattleType = cattle.CattleType, isVaccinated = "Not Vaccinated" });
                    }
                }
            }
            else
            {
                // If no vaccination status filter is provided, include all based on the previous filters
                foreach (var cattle in clist)
                {
                    bool isVaccinated = filterVaccList.Any(v => v.CattleTag == cattle.Tag);
                    filterCattleList.Add(new ViewCattle { Tag = cattle.Tag, Weight = cattle.Weight, CattleType = cattle.CattleType, isVaccinated = isVaccinated ? "Vaccinated" : "Not Vaccinated" });
                }
            }

            return Request.CreateResponse(HttpStatusCode.OK, filterCattleList);
        }


        //[HttpGet]
        //public HttpResponseMessage FilterAllCattles(string status, string type, string isVaccinate)
        //{
        //    List<Cattle> clist = new List<Cattle>();
        //    List<ViewCattle> filterCattleList = new List<ViewCattle>();
        //    List<InjectedVaccination> filterVaccList = db.InjectedVaccination.ToList();

        //    if (status != "")
        //    {
        //        //
        //        clist = db.Cattle.Where(n => n.Status == status).ToList();
        //        foreach (var a in clist)
        //        {
        //            var isVaccinated = filterVaccList.Any(v => v.CattleTag == a.Tag);
        //            if (isVaccinated)
        //            {
        //                filterCattleList.Add(new ViewCattle { Tag = a.Tag, Weight = a.Weight, CattleType = a.CattleType, isVaccinated = "Vaccinated" });
        //            }
        //            else
        //            {
        //                filterCattleList.Add(new ViewCattle { Tag = a.Tag, Weight = a.Weight, CattleType = a.CattleType, isVaccinated = "Not Vaccinated" });
        //            }
        //        }
        //        if (type != "")
        //        {
        //            filterCattleList = new List<ViewCattle>();
        //            clist = db.Cattle.Where(n => n.Status == status && n.CattleType == type).ToList();
        //            foreach (var a in clist)
        //            {
        //                var isVaccinated = filterVaccList.Any(v => v.CattleTag == a.Tag);
        //                if (isVaccinated)
        //                {
        //                    filterCattleList.Add(new ViewCattle { Tag = a.Tag, Weight = a.Weight, CattleType = a.CattleType, isVaccinated = "Vaccinated" });
        //                }
        //                else
        //                {
        //                    filterCattleList.Add(new ViewCattle { Tag = a.Tag, Weight = a.Weight, CattleType = a.CattleType, isVaccinated = "Not Vaccinated" });
        //                }
        //            }
        //            if (isVaccinate != "")
        //            {
        //                filterCattleList = new List<ViewCattle>();
        //                clist = db.Cattle.Where(n => n.Status == status && n.CattleType == type).ToList();
        //                filterVaccList = db.InjectedVaccination.ToList();
        //                foreach (var a in clist)
        //                {
        //                    var isVaccinated = filterVaccList.Any(v => v.CattleTag == a.Tag);
        //                    if (isVaccinate == "Vaccinated")
        //                    {
        //                        if (isVaccinated)
        //                        {
        //                            filterCattleList.Add(new ViewCattle { Tag = a.Tag, Weight = a.Weight, CattleType = a.CattleType, isVaccinated = "Vaccinated" });
        //                        }
        //                    }
        //                    else if (isVaccinate == "Not Vaccinated")
        //                    {
        //                        if (!isVaccinated)
        //                        {
        //                            filterCattleList.Add(new ViewCattle { Tag = a.Tag, Weight = a.Weight, CattleType = a.CattleType, isVaccinated = "Not Vaccinated" });
        //                        }
        //                    }
        //                }
        //                return Request.CreateResponse(HttpStatusCode.OK, filterCattleList);
        //            }
        //        }
        //    }
        //    if (type != "")
        //    {
        //        clist = db.Cattle.Where(n => n.CattleType == type).ToList();
        //        // fetch vaccine record 
        //        foreach (var a in clist)
        //        {
        //            var isVaccinated = filterVaccList.Any(v => v.CattleTag == a.Tag);
        //            if (isVaccinated)   // save vaccinated cattle record 
        //            {
        //                filterCattleList.Add(new ViewCattle { Tag = a.Tag, Weight = a.Weight, CattleType = a.CattleType, isVaccinated = "Vaccinated" });
        //            }
        //            else // save not vaccinated cattle record 
        //            {
        //                filterCattleList.Add(new ViewCattle { Tag = a.Tag, Weight = a.Weight, CattleType = a.CattleType, isVaccinated = "Not Vaccinated" });
        //            }
        //        }
        //        if (isVaccinate != "")
        //        {
        //            filterCattleList = new List<ViewCattle>();
        //            clist = db.Cattle.Where(n => n.CattleType == type).ToList();
        //            foreach (var a in clist)
        //            {
        //                bool isVaccinated = filterVaccList.Any(v => v.CattleTag == a.Tag);
        //                if (isVaccinate == "Vaccinated") // save vaccinated cattle record 
        //                {
        //                    if (isVaccinated)
        //                    {
        //                        filterCattleList.Add(new ViewCattle { Tag = a.Tag, Weight = a.Weight, CattleType = a.CattleType, isVaccinated = "Vaccinated" });
        //                    }
        //                }
        //                else if (isVaccinate == "Not Vaccinated") // save not vaccinated cattle record 
        //                {
        //                    if (!isVaccinated)
        //                    {
        //                        filterCattleList.Add(new ViewCattle { Tag = a.Tag, Weight = a.Weight, CattleType = a.CattleType, isVaccinated = "Not Vaccinated" });
        //                    }
        //                }
        //            }
        //            return Request.CreateResponse(HttpStatusCode.OK, filterCattleList);

        //        }
        //    }
        //    if (isVaccinate != "")
        //    {
        //        // fetch vaccine record 
        //        filterCattleList = new List<ViewCattle>();
        //        clist = db.Cattle.ToList();
        //        filterVaccList = db.InjectedVaccination.ToList();
        //        foreach (var a in clist)
        //        {
        //            bool isVaccinated = filterVaccList.Any(v => v.CattleTag == a.Tag);
        //            if (isVaccinate == "Vaccinated")
        //            {
        //                if (isVaccinated)
        //                {
        //                    filterCattleList.Add(new ViewCattle { Tag = a.Tag, Weight = a.Weight, CattleType = a.CattleType, isVaccinated = "Vaccinated" });
        //                }
        //            }
        //            else if (isVaccinate == "Not Vaccinated")
        //            {
        //                if (!isVaccinated)
        //                {
        //                    filterCattleList.Add(new ViewCattle { Tag = a.Tag, Weight = a.Weight, CattleType = a.CattleType, isVaccinated = "Not Vaccinated" });
        //                }
        //            }
        //        }
        //        return Request.CreateResponse(HttpStatusCode.OK, $"done");
        //    }
        /*            List<Cattle> clist = new List<Cattle>();
                    List<ViewCattle> filterCattleList = new List<ViewCattle>();
                    List<InjectedVaccination> filterVaccList = db.InjectedVaccination.ToList();
                    //------- first check status then type then vaccine
                    if (status != "")
                    {
                        clist = db.Cattle.Where(n => n.Status == status).ToList();
                        foreach (var a in clist)
                        {
                            var isVaccinated = filterVaccList.Any(v => v.CattleTag == a.Tag);
                            if (isVaccinated)
                            {
                                filterCattleList.Add(new ViewCattle { Tag = a.Tag, Weight = a.Weight, CattleType = a.CattleType, isVaccinated = "Vaccinated" });
                            }
                            else
                            {
                                filterCattleList.Add(new ViewCattle { Tag = a.Tag, Weight = a.Weight, CattleType = a.CattleType, isVaccinated = "Not Vaccinated" });
                            }
                        }
                        if (status != "" && type != "")
                        {
                            filterCattleList = new List<ViewCattle>();
                            clist = db.Cattle.Where(n => n.Status == status && n.CattleType == type).ToList();
                            foreach (var a in clist)
                            {
                                var isVaccinated = filterVaccList.Any(v => v.CattleTag == a.Tag);
                                if (isVaccinated)
                                {
                                    filterCattleList.Add(new ViewCattle { Tag = a.Tag, Weight = a.Weight, CattleType = a.CattleType, isVaccinated = "Vaccinated" });
                                }
                                else
                                {
                                    filterCattleList.Add(new ViewCattle { Tag = a.Tag, Weight = a.Weight, CattleType = a.CattleType, isVaccinated = "Not Vaccinated" });
                                }
                            }
                            if (status != "" && type != "" && isVaccinate != "")
                            {
                                filterCattleList = new List<ViewCattle>();
                                clist = db.Cattle.Where(n => n.Status == status && n.CattleType == type).ToList();
                                filterVaccList = db.InjectedVaccination.ToList();
                                foreach (var a in clist)
                                {
                                    var isVaccinated = filterVaccList.Any(v => v.CattleTag == a.Tag);
                                    if (isVaccinate == "Vaccinated")
                                    {
                                        if (isVaccinated)
                                        {
                                            filterCattleList.Add(new ViewCattle { Tag = a.Tag, Weight = a.Weight, CattleType = a.CattleType, isVaccinated = "Vaccinated" });
                                        }
                                    }
                                    else if (isVaccinate == "Not Vaccinated")
                                    {
                                        if (!isVaccinated)
                                        {
                                            filterCattleList.Add(new ViewCattle { Tag = a.Tag, Weight = a.Weight, CattleType = a.CattleType, isVaccinated = "Not Vaccinated" });
                                        }
                                    }
                                }
                            }
                        }
                    }
                    //------- first check type then status then vaccine
                    if (type != "")
                    {
                        clist = db.Cattle.Where(n => n.CattleType == type).ToList();
                        // fetch vaccine record 
                        foreach (var a in clist)
                        {
                            var isVaccinated = filterVaccList.Any(v => v.CattleTag == a.Tag);
                            if (isVaccinated)   // save vaccinated cattle record 
                            {
                                filterCattleList.Add(new ViewCattle { Tag = a.Tag, Weight = a.Weight, CattleType = a.CattleType, isVaccinated = "Vaccinated" });
                            }
                            else // save not vaccinated cattle record 
                            {
                                filterCattleList.Add(new ViewCattle { Tag = a.Tag, Weight = a.Weight, CattleType = a.CattleType, isVaccinated = "Not Vaccinated" });
                            }
                        }
                        if (type != "" && isVaccinate != "")
                        {
                            filterCattleList = new List<ViewCattle>();
                            clist = db.Cattle.Where(n =>n.CattleType == type).ToList();
                            foreach (var a in clist)
                            {
                                bool isVaccinated = filterVaccList.Any(v => v.CattleTag == a.Tag);
                                if (isVaccinate == "Vaccinated") // save vaccinated cattle record 
                                {
                                    if (isVaccinated)
                                    {
                                        filterCattleList.Add(new ViewCattle { Tag = a.Tag, Weight = a.Weight, CattleType = a.CattleType, isVaccinated = "Vaccinated" });
                                    }
                                }
                                else if (isVaccinate == "Not Vaccinated") // save not vaccinated cattle record 
                                {
                                    if (!isVaccinated)
                                    {
                                        filterCattleList.Add(new ViewCattle { Tag = a.Tag, Weight = a.Weight, CattleType = a.CattleType, isVaccinated = "Not Vaccinated" });
                                    }
                                }
                            }
                            return Request.CreateResponse(HttpStatusCode.OK, filterCattleList);
                            }
                    }
                    //------- first check vaccine then status then type
                    if (isVaccinate != "")
                    {
                        // fetch vaccine record 
                        filterCattleList = new List<ViewCattle>();
                        clist = db.Cattle.ToList();
                        filterVaccList = db.InjectedVaccination.ToList();
                        foreach (var a in clist)
                        {
                            bool isVaccinated = filterVaccList.Any(v => v.CattleTag == a.Tag);
                            if (isVaccinate == "Vaccinated")
                            {
                                if (isVaccinated)
                                {
                                    filterCattleList.Add(new ViewCattle { Tag = a.Tag, Weight = a.Weight, CattleType = a.CattleType, isVaccinated = "Vaccinated" });
                                }
                            }
                            else if (isVaccinate == "Not Vaccinated")
                            {
                                if (!isVaccinated)
                                {
                                    filterCattleList.Add(new ViewCattle { Tag = a.Tag, Weight = a.Weight, CattleType = a.CattleType, isVaccinated = "Not Vaccinated" });
                                }
                            }
                        }
                        if (status != "" && isVaccinate!="")
                        {
                            filterCattleList = new List<ViewCattle>();
                            clist = db.Cattle.Where(n => n.Status == status).ToList();
                            foreach (var a in clist)
                            {
                                bool isVaccinated = filterVaccList.Any(v => v.CattleTag == a.Tag);
                                if (isVaccinate == "Vaccinated")
                                {
                                    if (isVaccinated)
                                    {
                                        filterCattleList.Add(new ViewCattle { Tag = a.Tag, Weight = a.Weight, CattleType = a.CattleType, isVaccinated = "Vaccinated" });
                                    }
                                }
                                else if (isVaccinate == "Not Vaccinated")
                                {
                                    if (!isVaccinated)
                                    {
                                        filterCattleList.Add(new ViewCattle { Tag = a.Tag, Weight = a.Weight, CattleType = a.CattleType, isVaccinated = "Not Vaccinated" });
                                    }
                                }
                            }
                            return Request.CreateResponse(HttpStatusCode.OK, filterCattleList);
                           }
                    }*/
        //      return Request.CreateResponse(HttpStatusCode.OK, "fll");
        //  }


        //[HttpGet]
        //public HttpResponseMessage ViewCattleinfo()
        //{
        //    var request=HttpContext.Current.Request;
        //    string id = request["id"];
        //    int cid = int.Parse(id);
        //    var cattle = db.Cattle.Where(n => n.ID == cid).FirstOrDefault();
        //    var res = (from c in db.Cattle
        //              join v in db.InjectedVaccination
        //              on cid equals v.CattleID
        //              select new { ID =c.ID,Gender=c.Gender,Weight=c.Weight, Age=c.Age, VaccineType=v.CattleID==0?"Not Vaccinated":"Vaccinated", VaccineiD=v.VaccinationID, VaccineDate=v.Date }).FirstOrDefault();
        //    CattleInfo cattleInfo = new CattleInfo();
        //    if (res != null)
        //    {
        //        cattleInfo.ID = res.ID;
        //        cattleInfo.Gender = res.Gender;
        //        cattleInfo.Weight= res.Weight;
        //        cattleInfo.Age = res.Age;
        //        if (res.VaccineType == "Vaccinated")
        //        {
        //            var res1=db.Vaccination.FirstOrDefault(n => n.ID==res.VaccineiD);
        //            cattleInfo.VaccineType = res1.Type;
        //            cattleInfo.Date = res.VaccineDate;
        //        }
        //        else
        //        {
        //            cattleInfo.VaccineType = res.VaccineType;
        //            cattleInfo.Date = null;
        //        }
        //    }
        //    return Request.CreateResponse(HttpStatusCode.OK,cattleInfo);
        //   }

        //[HttpGet]
        //public HttpResponseMessage ViewCattleinfo(String tag)
        //{
        //    var cattleInfo = (from c in db.Cattle
        //                      join v in db.InjectedVaccination on c.Tag equals v.CattleTag into gj
        //                      from subv in gj.DefaultIfEmpty()
        //                      where c.Tag == tag
        //                      select new CattleInfo
        //                      {
        //                          Tag = c.Tag,
        //                          Gender = c.Gender,
        //                          Weight = c.Weight,

        //                          VaccineType = subv == null ? "Not Vaccinated" : "Vaccinated",
        //                          //     VaccineID = subv == null ? (int?)null : subv.VaccinationID,
        //                          Date = subv == null ? (DateTime?)null : subv.Date
        //                      }).FirstOrDefault();

        //    return Request.CreateResponse(HttpStatusCode.OK, cattleInfo);
        //}

        //[HttpGet]
        //public HttpResponseMessage GetAllCattle()
        //{
        //    List<Cattle> cattle = new List<Cattle>();
        //    return Request.CreateResponse(HttpStatusCode.OK, db.Cattle);
        //}

    }
}
