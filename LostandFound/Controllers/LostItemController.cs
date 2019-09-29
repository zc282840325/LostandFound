using LostandFound.Models;
using System.Linq;
using System.Web.Mvc;

namespace LostandFound.Controllers
{
    public class LostItemController : Controller
    {
        private Model1 db = new Model1();
        public LostandFound.Tools.Tools tools = new LostandFound.Tools.Tools();
        // GET: LostItem
        public ActionResult Index()
        {
            var list = db.ItemType.Where(x => x.FID == 0);

            return View(list);
        }

        public ActionResult Find(int? FID, string Name)
        {
            ViewBag.Title = Name;
            var list = db.ItemType.Where(x => x.FID == FID);

            return View(list);
        }

        public ActionResult DetailsList(int? TID, string Name)
        {
            ViewBag.Title = Name;
            var list =  from g in db.Goods
                        join u in db.UserGoods on
                        g.GID equals u.GID
                        where u.Type == "1"
                        where g.TID==TID
                        where g.Status== "Found"
                        select g;

            return View(list);
        }

        public ActionResult Details(int? GID)
        {
            var list = db.Goods.Where(x => x.GID == GID);
            return View(list);
        }
        public ActionResult Found(int id)
        {
            string str_s = string.Empty;

            if (Request.Cookies["UID"] == null)
            {
                ViewBag.Msg = "You are not logged in, please log in first, please do this!";
            }
            else
            {
                var goodsUser = db.UserGoods.Where(x => x.GID == id).FirstOrDefault();
                var user = db.User.Find(goodsUser.UID);
                var goods = db.Goods.Find(goodsUser.GID);
                if (goods.Status=="Found")
                {
                    ViewBag.Msg = "You can contact the User:\r\nE - mail:" + user.Email + "StudentID:" + user.StudentID;
                }
                else
                {
                    ViewBag.Msg = "You can contact the Admin:\r\nE - mail:admin@admin.com, Tel:**************";

                }
            }
            return View();
        }
    }
}