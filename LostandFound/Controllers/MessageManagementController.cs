using LostandFound.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace LostandFound.Controllers
{
    public class MessageManagementController : Controller
    {
        // GET: MessageManagement
        private Model1 db = new Model1();
        public ActionResult Index()
        {
            return View(db.Message.ToList());
        }
        [HttpGet]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Message goods = db.Message.Find(id);
            if (goods == null)
            {
                return HttpNotFound();
            }
            return View(goods);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id)
        {
           db.Message.Remove(db.Message.Find(id));
           db.SaveChanges();
            return RedirectToAction("Index");
        }


        public ActionResult Save(string message,int mid,int uid)
        {
            string str_s = "Successfully Replied!";
            bool b_result = true;

            var user = db.User.Find(uid);
            user.Message  += "Your question has been answered:" + message;
            db.Entry<User>(user).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();


            var result = db.Message.Find(mid);
            result.Status = "1";
            db.Entry<Message>(result).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return Content("{\"b_result\":\"" + b_result + "\",\"str_s\":\"" + str_s + "\"}", "json");

        }
    }
}