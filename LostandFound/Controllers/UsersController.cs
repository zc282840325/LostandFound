using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LostandFound.Models;

namespace LostandFound.Controllers
{
    public class UsersController : Controller
    {
        private Model1 db = new Model1();

        // GET: Users
        public ActionResult Index()
        {
            if (Request.Cookies["AID"]==null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View(db.User.Where(x=>x.Status=="0").ToList());
            }
        }

        // GET: Users/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.User.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UID,UserName,UserPassWord,Email,StudentID,Que1,Ans1,Que2,Ans2,Message,Image,Status")] User user)
        {
            if (ModelState.IsValid)
            {
                user.Status = "0";
                db.User.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.User.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UID,UserName,UserPassWord,Email,StudentID,Que1,Ans1,Que2,Ans2,Message,Image,Status")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.User.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.User.Find(id);
            user.Status = "3";
            user.Message = "You have an operation that does not meet the requirements. The account has been deleted and cannot be logged in!";
            db.Entry<User>(user).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("UserList");
        }

        public ActionResult Approve(int? id)
        {
            if (id==default(int))
            {
                //
            }
            else
            {
                var user = db.User.Where(x => x.UID == id).FirstOrDefault();
                user.Status = "1";
                db.Entry<User>(user).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public ActionResult UserList()
        {
            if (Request.Cookies["AID"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View(db.User.Where(x=>x.Status=="1").ToList());
            }
        }
        public ActionResult Waring(int? id)
        {
            ViewBag.UID = id;
            return View();
        }
        [HttpPost]
        public ActionResult Waring(string Message1, string uid)
        {
            if (string.IsNullOrEmpty(Message1))
            {
                return View();
            }
            else
            {
                int id = Convert.ToInt32(uid);
                var user = db.User.Where(x=>x.UID==id).FirstOrDefault();
                user.Message = Message1;
                db.Entry<User>(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("UserList");
            }

          
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
