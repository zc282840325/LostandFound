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
    public class NewsController : Controller
    {
        private Model1 db = new Model1();

        // GET: News
        public ActionResult Index()
        {
            var goods = db.Goods.Include(g => g.ItemType);
            return View(goods.ToList());
        }
        public ActionResult ItemManagenment()
        {
            return View();
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
                        where u.Type == "0"
                        select g;
            return View(goods);
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
            ViewBag.TID = new SelectList(db.ItemType.Where(x => x.FID>= 1 && x.FID<= 4), "TID", "TypeName");
            return View();
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
                    return RedirectToAction("Progress", "News");
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
                return RedirectToAction("Index");
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
            HttpCookie uidcookie = Request.Cookies["UID"];
            int uid = Convert.ToInt32(uidcookie.Value);
            UserGoods goods = db.UserGoods.Where(x => x.GID == id && x.UID == uid).FirstOrDefault();
            db.Entry<UserGoods>(goods).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToRoute(new { controller = "Home", action = "Post" });
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
