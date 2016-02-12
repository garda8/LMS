using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LMS.Models;
using System.Globalization;
using System.IO;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using LMS.ViewModels;


using LMS.Repositories;
using System.Security.Claims;
//using System.Web.Security;


namespace LMS.Controllers
{
    public class schemaOccasionsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        CultureInfo provider = CultureInfo.InvariantCulture;

        private ApplicationUserManager _userManager;

        
        static string filePath;
        /* GET: schemaOccasions
        public ActionResult Index()
        {
            var occasions = db.occasions.Include(s => s.klass);
            return View(occasions.ToList());
        }*/

        // GET: schemaOccasions
        public ActionResult Index(int? id)
        {
            //var occasions = db.occasions.Include(s => s..klass);
            var occasions = db.occasions.Where(s => s.KlassId==id);
            ViewBag.KlassId = id;
            return View(occasions.ToList());
        }

        /*
        public ActionResult Downloads()
        {
            var dir = new System.IO.DirectoryInfo(Server.MapPath("~/App_Data/Images/"));
            System.IO.FileInfo[] fileNames = dir.GetFiles("*.*");
            List<string> items = new List<string>();

            foreach (var file in fileNames)
            {
                items.Add(file.Name);
            }

            return View(items);
        } */

        /*
        public ActionResult Download(string fileName)
        {
            String url = filePath + fileName;
            //return File(fileAndPath, System.Net.Mime.MediaTypeNames.Application.Octet);
            //byte[] filedata = System.IO.File.OpenRead(url);
            
            byte[] filedata = System.IO.File.ReadAllBytes(url);
            string contentType = MimeMapping.GetMimeMapping(url);

            var cd = new System.Net.Mime.ContentDisposition
            {
                FileName = fileName,
                Inline = true,
            };

            Response.AppendHeader("Content-Disposition", cd.ToString());

            return File(filedata, contentType);

        }*/

        public ActionResult SaveComment(string feedback, int fileId)
        {
            Fil fil = db.Filer.Find(fileId);
            string retval = "Kommentaren är sparad";
            if (ModelState.IsValid && fil != null)
            {
                String currentDate = DateTime.Now.ToShortDateString();
                fil.TeacherFeedback = feedback + " [" + currentDate + "] ";
                db.Entry(fil).State = EntityState.Modified;
                db.SaveChanges();
            }
            else {
                retval = "Not saved";
            }
            return Json(retval, JsonRequestBehavior.AllowGet);
            
        }  
        
        public FilePathResult Download(string fileName)
        {
            String fileAndPath = filePath + fileName;
            return File( fileAndPath, System.Net.Mime.MediaTypeNames.Application.Octet, Path.GetFileName(fileAndPath));
        }

        public FileContentResult FileDownload(int id)
        {
            //declare byte array to get file content from database and string to store file name
            byte[] fileData;
            string fileName;

            Fil fil = db.Filer.Find(id);

            //only one record will be returned from database as expression uses condtion on primary field
            //so get first record from returned values and retrive file content (binary) and filename
            fileData = (byte[])fil.Content.ToArray();
            fileName = fil.fileName;
            //return file and provide byte file content and file name
            return File(fileData, fil.ContentType, fileName);
        }
        
        // GET: schemaOccasions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null) {  return new HttpStatusCodeResult(HttpStatusCode.BadRequest); }
            
            schemaOccasion schemaOccasion = db.occasions.Find(id);
            if (schemaOccasion == null) {  return HttpNotFound(); }

            var item = db.Filer.FirstOrDefault(i => i.schemaOccasionId == id);

            if (item != null)
            {
                // found it
                ViewBag.Fil = item;

                int startPos = item.filePath.IndexOf("Filer");
                int endPos = item.filePath.IndexOf(item.fileName);
                string completePath = item.filePath;
                string dirPath = completePath.Substring(startPos, endPos - startPos);
                filePath = "..\\" + dirPath;
                var dir = new System.IO.DirectoryInfo(Server.MapPath("~/"+dirPath +"/"));
                System.IO.FileInfo[] fileNames = dir.GetFiles("*.*");
                List<string> items = new List<string>();
                foreach (var file in fileNames)
                {
                    items.Add(file.Name);
                }
                ViewBag.filer = items;
            }
            else
                ViewBag.Fil = null;
                //db.Filer.Where(f => f.schemaOccasionId.Equals(id)).First();





            String userId = User.Identity.GetUserId();
            ViewBag.userId = userId;
            /*if (UserManager.IsInRole(userId, "admin"))
                ViewBag.userRole = "admin";
            else if (UserManager.IsInRole(userId, "student"))
                ViewBag.userRole = "student";
            else if (UserManager.IsInRole(userId, "teacher"))
                ViewBag.userRole = "teacher";
            */




            List<StudentsTaskList> stasks = new List<StudentsTaskList>();
            
            //Hämta studenter knutna till klassen (för läraren)
            KlassesController klasses = new KlassesController();
            List<User> students = klasses.GetUsersInClass(schemaOccasion.KlassId, "student");
            
            foreach (User u in students) {
                //List<Fil> filer = db.Filer.Where(f => f.schemaOccasionId == id).ToList();
                List<Fil> filer = db.Filer.Where(f => f.owner==u.Id).ToList();
                StudentsTaskList stask = new StudentsTaskList();
                stask.id = u.Id;
                stask.Name = u.Name;
                foreach (Fil f in filer) {
                    if (f.schemaOccasionId == id)
                        stask.filer.Add(f);
                }
                stasks.Add(stask);
            }
            ViewBag.stasks = stasks;
            return View(schemaOccasion);

        }

        [HttpPost]
        public ActionResult Details(HttpPostedFileBase file, String schemaOccasionId)
        {

            schemaOccasion occasion = db.occasions.Find(Int32.Parse(schemaOccasionId));
            Klass klass = db.Klasser.Find(occasion.KlassId);

            if (file!=null && file.ContentLength > 0)
            {
                String Name = User.Identity.Name;
                
                var fileName = Path.GetFileName(file.FileName);

                //Spara fil i mappsystemet:
                String strMappath = Server.MapPath("~/Filer/" + klass.Name + "/" +occasion.name_for_schemaoccasion + "/submit/" + Name);
                if (!Directory.Exists(strMappath))
                {
                    DirectoryInfo di = Directory.CreateDirectory(strMappath);
                }

                var path = Path.Combine(strMappath, fileName);
                file.SaveAs(path);

                //Spara i databas, knuten till klassen som ges, schemaOccasion och student:
                Fil fil = new Fil();
                using (var reader = new System.IO.BinaryReader(file.InputStream))
                {
                    fil.Content = reader.ReadBytes(file.ContentLength);
                }
                fil.ContentType = file.ContentType;
                fil.date = DateTime.Now;
                fil.fileName = file.FileName;
                //skippa filepath.. 
                //fil.fileType skippa..
                fil.isShared = false;
                fil.owner = User.Identity.GetUserId();
                fil.schemaOccasionId = Int32.Parse(schemaOccasionId);

                db.Filer.Add(fil);
                db.SaveChanges();
            }
            return RedirectToAction("Index/"+klass.Id);
        }
        
        
        // GET: schemaOccasions/Create
        public ActionResult Create(int? id)
        {
            ViewBag.KlassId = id; 
            return View();
        }

        // POST: schemaOccasions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "name_for_schemaoccasion,description")] schemaOccasion schemaOccasion, string klassId, string startTime, string endTime)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    schemaOccasion.startTime = DateTime.Parse(startTime);
                    schemaOccasion.endTime = DateTime.Parse(endTime);
                    schemaOccasion.KlassId = Int32.Parse(klassId);

                    Klass klass = db.Klasser.Find(Int32.Parse(klassId));
                    String strMappath = Server.MapPath("~/Filer/" + klass.Name + "/shared/");
                    if (!Directory.Exists(strMappath))
                    {
                        DirectoryInfo di = Directory.CreateDirectory(strMappath);
                    }

                    schemaOccasion.path_to_inlamningsuppgift = strMappath;


                    db.occasions.Add(schemaOccasion);
                    db.SaveChanges();
                }
                catch (FormatException)
                {
                    Console.WriteLine("{0} is not in the correct format.", startTime);
                } 
                return RedirectToAction("Index/"+klassId);
            }

            ViewBag.KlassId = new SelectList(db.Klasser, "Id", "TeacherId", schemaOccasion.KlassId);
            return View(schemaOccasion);
        }

        // GET: schemaOccasions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            schemaOccasion schemaOccasion = db.occasions.Find(id);
            if (schemaOccasion == null)
            {
                return HttpNotFound();
            }
            /*ViewBag.KlassId = new SelectList(db.Klasser, "Id", "TeacherId", schemaOccasion.KlassId);*/
            ViewBag.KlassId = schemaOccasion.KlassId; 
            return View(schemaOccasion);
        }

        // POST: schemaOccasions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id, name_for_schemaoccasion,description")] schemaOccasion schemaOccasion, string startTime, string endTime, string klassId, HttpPostedFileBase file)
        {
            schemaOccasion edit = db.occasions.Find(schemaOccasion.Id);
            edit.name_for_schemaoccasion = schemaOccasion.name_for_schemaoccasion;
            Klass klass = db.Klasser.Find(Int32.Parse(klassId));
                   
            if (ModelState.IsValid)
            {
                //schemaOccasion edit = db.occasions.Find(schemaOccasion.Id);
                //edit.name_for_schemaoccasion = schemaOccasion.name_for_schemaoccasion;
                edit.description = schemaOccasion.description;
                edit.startTime = DateTime.Parse(startTime);
                edit.endTime = DateTime.Parse(endTime);
                
                db.Entry(edit).State = EntityState.Modified;
                db.SaveChanges();
                
            }

            if (file.ContentLength > 0)
            {
                

                var fileName = Path.GetFileName(file.FileName);

                String strMappath = Server.MapPath("~/Filer/" + klass.Name + "/" + edit.name_for_schemaoccasion + "/shared/");
                if (!Directory.Exists(strMappath))
                {
                    DirectoryInfo di = Directory.CreateDirectory(strMappath);
                }

                var path = Path.Combine(strMappath, fileName);
                file.SaveAs(path);

                Fil fil = new Fil();
                fil.date = DateTime.Now;
                fil.filePath = path;
                fil.isShared = true;
                fil.owner = User.Identity.Name;
                fil.schemaOccasionId = schemaOccasion.Id;
                fil.fileName = fileName;
                db.Filer.Add(fil);
                db.SaveChanges();
                  
            }

            return RedirectToAction("Index/" + edit.KlassId);

            /*ViewBag.KlassId = new SelectList(db.Klasser, "Id", "TeacherId", schemaOccasion.KlassId);
            return View(schemaOccasion);*/
        }

        // GET: schemaOccasions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            schemaOccasion schemaOccasion = db.occasions.Find(id);
            if (schemaOccasion == null)
            {
                return HttpNotFound();
            }
            return View(schemaOccasion);
        }

        // POST: schemaOccasions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            schemaOccasion schemaOccasion = db.occasions.Find(id);
            db.occasions.Remove(schemaOccasion);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
    }
}
