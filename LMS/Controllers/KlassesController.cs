using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LMS.Models;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity.EntityFramework;
using LMS.ViewModels;
using System.IO;

namespace LMS.Controllers
{
    public class KlassesController : Controller
    {

        private ApplicationUserManager _userManager;
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Klasses
        [Authorize]
        public ActionResult Index()
        {
            //StockItem stockItem = db.Items.Include(i=>i.ItemType).Include(i => i.Shelves).Where(i => i.Id== id).Single();

            String userId =  User.Identity.GetUserId();
            User current = db.Users.Find(userId);
            //var klasser = from klass in db.Klasser 
            var klasser = (db.Klasser.Include(i =>i.Students)).ToList();

            if (UserManager.IsInRole(userId, "admin"))  //Visa alla kurser för admin.
            {
                return View(klasser);

            }
            else
            {
                List<Klass> selectedKlasser = new List<Klass>();

                foreach (var klass in klasser)
                {
                    if (klass.Students.Contains(current))
                    {
                        selectedKlasser.Add(klass);
                    }
                }
                //var klasser = db.Klasser.Include(i =>i.Students).Where(i=> i.Students.Contains(current));

                return View(selectedKlasser);
            }
        }

        [HttpPost]
        public ActionResult Details(HttpPostedFileBase file, String klassName)
        {

            if (file.ContentLength > 0)
            {
                String Name = User.Identity.Name;
                
                var fileName = Path.GetFileName(file.FileName);
                
                String strMappath = Server.MapPath("~/Filer/" +klassName +"/submit/"+Name);
                 if (!Directory.Exists(strMappath))
                    {
                        DirectoryInfo di = Directory.CreateDirectory(strMappath);
                    }                
                
                var path = Path.Combine(strMappath, fileName);
                file.SaveAs(path);
            }

            return RedirectToAction("Index");
        }
        // GET: Klasses/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                Klass klass = db.Klasser.Find(id);
                if (klass == null)
                {
                    return HttpNotFound();
                }
                int check;
                if (id.HasValue) check = id.Value;
                else check = 0;
                List<User> students = GetUsersInClass(check, "student");
                ViewBag.students = students;

                List<User> teachers = GetUsersInClass(check, "teacher");
                ViewBag.teachers = teachers;
                
                return View(klass);
            }
        }

        // GET: Klasses/Create
        public ActionResult Create()
        {
            //ViewBag.TeacherId = new SelectList(db.Users, "Id", "Email");
            //ViewBag.TeacherId2 = GetUsersInRole("teacher");
            
            //ViewBag.TeacherId = new SelectList(db.Users, "Id", "Email");
            return View();
        }

        // POST: Klasses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,startDate")] Klass klass)
        {
            if (ModelState.IsValid)
            {
                klass.sharedFolder = "shared";
                klass.submitFolder = "submit";
                db.Klasser.Add(klass);
                db.SaveChanges();
                return RedirectToAction("Index");
            
            
            }
            /* Test att hämta bara de användare som är lärare 
            String userId = User.Identity.GetUserId();
            User current = db.Users.Find(userId);
            //var klasser = from klass in db.Klasser 
            var users = (db.Users.Include(i => i.Id)).ToList();
            ViewBag.TeacherId = new SelectList(db.Users, "Id", "Email", klass.TeacherId);*/
            return View(klass);
        }


        public List<User> GetUsersInRole(string roleName)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));
            var role = roleManager.FindByName(roleName).Users.First();
            var usersInRole = db.Users.Where(u => u.Roles.Select(r => r.RoleId).Contains(role.RoleId)).ToList();
            return usersInRole;
        }

        public List<User> GetUsersInClass(int klassId, string roleName)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));
            var role = roleManager.FindByName(roleName).Users.First();
            //var usersInKlass = db.Users.Where(u=>u.Klasser.Select(k=>k.Id).Contains(klassId)).ToList();
            var usersInKlass = db.Users.Where(u => u.Klasser.Select(k => k.Id).Contains(klassId) && u.Roles.Select(r=>r.RoleId).Contains(role.RoleId)).ToList();
           
            //var usersInKlass = db.Users.Where(u=>u.Klasser.Select(k=>k.Id).Contains(klassId) && (u=>u.Roles.Select(r => r.RoleId).Contains(role.RoleId))).ToList();
           
            return usersInKlass;
        }


        // GET: Klasses/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Klass klass = db.Klasser.Find(id);
            if (klass == null)
            {
                return HttpNotFound();
            }

            PopulateAssignedTeachersData(klass);
            
            //ViewBag.TeacherId2 = GetUsersInRole("teacher");
            //ViewBag.TeacherId = new SelectList(db.Users, "Id", "Email", klass.TeacherId);
            return View(klass);
        }

        private void PopulateAssignedTeachersData(Klass klass)
        {
            var allTeachers = GetUsersInRole("teacher");
            var students = new HashSet<string>(klass.Students.Select(c => c.Id));
            var viewModel = new List<AssignedTeachersData>();
            foreach (var teacher in allTeachers)
            {
                viewModel.Add(new AssignedTeachersData
                {
                    TeacherId = teacher.Id,
                    Name = teacher.Name,
                    Assigned = students.Contains(teacher.Id)
                });
            }
            ViewBag.Teachers = viewModel;
        }

        // POST: Klasses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,TeacherId,Name,sharedFolder,submitFolder,startDate")] Klass klass, string[] selectedTeachers)
        {
            if (ModelState.IsValid)
            {
                db.Entry(klass).State = EntityState.Modified;
                db.SaveChanges();

                Klass newKlass = db.Klasser.Find(klass.Id);
                newKlass.Students = new List<User>(); // db.Items.Find(stockItem.Id).Shelves;

                foreach (var user in GetUsersInRole("teacher"))
                {
                    if (user.Klasser.Contains(newKlass))
                        user.Klasser.Remove(newKlass);
                    if ((null != selectedTeachers) && (selectedTeachers.Contains(user.Id)))
                        newKlass.Students.Add(user);
                }
                //bool changed = UpdateItemShelves(selectedShelves, newItem);

                db.Entry(newKlass).State = EntityState.Modified;
                db.SaveChanges();
                   
                return RedirectToAction("Index");
            }
            ViewBag.TeacherId = new SelectList(db.Users, "Id", "Email", klass.TeacherId);
            return View(klass);
        }

        // GET: Klasses/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Klass klass = db.Klasser.Find(id);
            if (klass == null)
            {
                return HttpNotFound();
            }
            return View(klass);
        }

        // POST: Klasses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Klass klass = db.Klasser.Find(id);
            db.Klasser.Remove(klass);
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
