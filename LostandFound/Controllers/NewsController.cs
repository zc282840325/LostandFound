using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LostandFound.Models;
using PagedList;

namespace LostandFound.Controllers
{
    public class NewsController : Controller
    {
        private Model1 db = new Model1();

        // GET: News
        public ActionResult Index(int? page, string GoodsName)
        {
            var goods = db.Goods.Include(g => g.ItemType);
            int pageNumber = page ?? 1;

            int pageSize = 5;
            if (string.IsNullOrEmpty(GoodsName))
            {
                goods = goods.OrderByDescending(x => x.Date);
            }
            else
            {
                goods = goods.Where(x=>x.GoodsName.Contains(GoodsName)).OrderByDescending(x => x.Date);
            }
           

            IPagedList<Goods> pagedList = goods.ToPagedList(pageNumber, pageSize);

            return View(pagedList);
        }
        public ActionResult ItemManagenment()
        {
            return View();
        }
        public ActionResult Approve()
        {
            var goods = from g in db.Goods
                        join u in db.UserGoods on
                        g.GID equals u.GID
                        where u.Type=="0"
                        select g;
           
            return View(goods);
        }

        public ActionResult Finished()
        {
            var goods = from g in db.Goods
                        join u in db.UserGoods on
                        g.GID equals u.GID
                        where u.Type == "3"
                        select g;
            return View(goods);
        }
        public ActionResult Progress()
        {
            var goods = from g in db.Goods
                        join u in db.UserGoods on
                        g.GID equals u.GID
                        where u.Type == "1"
                        select g;
            return View(goods);
        }

        public ActionResult ApproveTask(int id)
        {
            if (id != default(int))
            {
                var goods = db.UserGoods.Where(x => x.GID == id).FirstOrDefault();
                goods.Type = "1";
                db.Entry<UserGoods>(goods).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Approve");
        }

        public ActionResult FinishedTask(int id)
        {
            if (id!=default(int))
            {
                var goodsUser = db.UserGoods.Where(x => x.GID == id).FirstOrDefault();
                goodsUser.Type = "3";
                db.Entry<UserGoods>(goodsUser).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Post","Home");
        }

        public ActionResult Refuse(string message,int gid)
        {
            string str_s = "Successfully Replied!";
            bool b_result = true;

            var usergoods = db.UserGoods.Where(x => x.GID == gid).FirstOrDefault();
            int uid = usergoods.UID;

            var user = db.User.Find(uid);
            user.Message += "Your approval was rejected due to:" + message;
            db.Entry<User>(user).State = EntityState.Modified;
            db.SaveChanges();

            db.Goods.Remove(db.Goods.Find(gid));
            db.SaveChanges();

            return Content("{\"b_result\":\"" + b_result + "\",\"str_s\":\"" + str_s + "\"}", "json");
        }

        // GET: News/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Goods goods = db.Goods.Find(id);
            if (goods == null)
            {
                return HttpNotFound();
            }
            return View(goods);
        }

        // GET: News/Create
        public ActionResult Create()
        {
            if (Request.Cookies["UID"]==null)
            {
                if (Request.Cookies["AID"] != null)
                {
                    ViewBag.TID = new SelectList(db.ItemType.Where(x => x.FID >= 1 && x.FID <= 4), "TID", "TypeName");
                    return View();
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            else
            {
                ViewBag.TID = new SelectList(db.ItemType.Where(x => x.FID >= 1 && x.FID <= 4), "TID", "TypeName");
                return View();
            }
           
        }

        // POST: News/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "GID,TID,GoodsName,GoodsImage,Description,Date,Address,Status")] Goods goods)
        {
            if (Request.Cookies["UID"]==null)
            {
                if (ModelState.IsValid)
                {
                    db.Goods.Add(goods);
                    db.SaveChanges();
                    HttpCookie cookie = Request.Cookies["AID"];
                    int uid = Convert.ToInt32(cookie.Value);
                    db.UserGoods.Add(new UserGoods() { GID = goods.GID, UID = uid, Type = "1" });
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ViewBag.TID = new SelectList(db.ItemType, "TID", "TypeName", goods.TID);
                return View(goods);
            }
            else
            {
                if (ModelState.IsValid)
                {
                    db.Goods.Add(goods);
                    db.SaveChanges();

                    HttpCookie cookie = Request.Cookies["UID"];
                    int uid = Convert.ToInt32(cookie.Value);
                    db.UserGoods.Add(new UserGoods() { GID = goods.GID, UID = uid, Type = "0" });
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }

                ViewBag.TID = new SelectList(db.ItemType, "TID", "TypeName", goods.TID);
                return View(goods);
            }
           
        }

        // GET: News/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Goods goods = db.Goods.Find(id);
            if (goods == null)
            {
                return HttpNotFound();
            }
            ViewBag.TID = new SelectList(db.ItemType.Where(x=>x.FID>=1&&x.FID<=4), "TID", "TypeName", goods.TID);
            return View(goods);
        }

        // POST: News/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "GID,TID,GoodsName,GoodsImage,Description,Date,Address,Status")] Goods goods)
        {
       
            if (ModelState.IsValid)
            {
                db.Entry(goods).State = EntityState.Modified;
                db.SaveChanges();
                if (Request.Cookies["UID"] == null)
                {
                    return RedirectToAction("Progress", "News");
                }
                else
                {
                    return RedirectToAction("Post", "Home");
                   
                }
              
            }
            ViewBag.TID = new SelectList(db.ItemType, "TID", "TypeName", goods.TID);
            return View(goods);
        }

        // GET: News/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Goods goods = db.Goods.Find(id);
            if (goods == null)
            {
                return HttpNotFound();
            }
            return View(goods);
        }

        // POST: News/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Request.Cookies["UID"]==null)
            {
                db.Goods.Remove(db.Goods.Find(id));
                db.SaveChanges();
                return RedirectToAction("Finished", "News"); 
            }
            else
            {
                HttpCookie uidcookie = Request.Cookies["UID"];
                int uid = Convert.ToInt32(uidcookie.Value);
                UserGoods goods = db.UserGoods.Where(x => x.GID == id && x.UID == uid).FirstOrDefault();
                goods.Type = "3";
                db.Entry<UserGoods>(goods).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Post", "Home");
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
