 using LostandFound.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace LostandFound.Controllers
{
    public class HomeController : Controller
    {
        private Model1 db = new Model1();
        public LostandFound.Tools.Tools tools = new LostandFound.Tools.Tools();
        public ActionResult Index()
        {
            if (Request.Cookies["AID"]!=null)
            {
                return RedirectToAction("Index", "Users");
            }
            else
            {
                var goods = db.Goods.ToList();
                goods = goods.OrderByDescending(x=>x.Date).Take(5).ToList();
                ViewData["Goods"] = goods;
                return View();
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(string UserName, string UserPassWord)
        {
            if (string.IsNullOrEmpty(UserName)&&string.IsNullOrEmpty(UserPassWord))
            {
                ViewBag.Msg = "Account or password cannot be empty！";
            }
            else
            {
                var result = db.User.Where(x => x.UserName == UserName && x.UserPassWord == UserPassWord).FirstOrDefault();
                if (result==null)
                {
                    ViewBag.Msg = "Account or password entered incorrectly！";
                }
                else
                {
                    HttpCookie UserCookie = tools.CreateCookie("UserName", result.UserName);
                    Response.Cookies.Add(UserCookie);
                    HttpCookie ImageCookie = tools.CreateCookie("UerImage", result.Image.ToString());
                    Response.Cookies.Add(ImageCookie);
                    #region CheckUserStatus
                    if (result.Status=="0")
                    {
                        ViewBag.Msg = "Your account has not been approved by the administrator, please be patient!";
                    }
                    else if(result.Status == "2")
                    {
                      
                        HttpCookie IDCookie = tools.CreateCookie("AID", result.UID.ToString());
                        Response.Cookies.Add(IDCookie);
                        return RedirectToAction("Index", "Users");
                    }
                    else
                    {
                        #region Set(UserName、UID)Cookie
                        HttpCookie IDCookie = tools.CreateCookie("UID", result.UID.ToString());
                        Response.Cookies.Add(IDCookie);

                     
                        #endregion
                        return RedirectToAction("Index");
                    }
                    #endregion
                  
                }
            }
            return View();
        }
        [HttpGet]
        public ActionResult Regist()
        {
            ViewBag.Msg = "";
            List<string> vs = new List<string>() { "What is your favorite fruit？", "What is your favorite place to go?", "What song do you like the most?" };

            ViewData["Ques"] = vs;
            return View();
        }
        [HttpPost]
        public ActionResult Regist(User user)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Msg = "";
                return View(user);
            }
            else
            {
                if (db.User.Where(x => x.UserName == user.UserName).Count() > 0)
                {
                    ViewBag.Msg = "Current username is already registered";
                    return View(user);
                }
                if (db.User.Where(x => x.StudentID == user.StudentID).Count() > 0)
                {
                    ViewBag.Msg = "Current StudentID is already registered";
                    return View(user);
                }
                user.Status = "0";
                db.User.Add(user);
                db.SaveChanges();
                return RedirectToAction("Login");
            }
           
        }

        [HttpGet]
        public ActionResult Forget()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Forget(string Email)
        {
            Regex r = new Regex("^\\s*([A-Za-z0-9_-]+(\\.\\w+)*@(\\w+\\.)+\\w{2,5})\\s*$");
            if (string.IsNullOrEmpty(Email))
            {
                ViewBag.Msg = "Email is required！";
            }
            else
            {
                if (r.IsMatch(Email))
                {
                    var user = db.User.Where(x => x.Email == Email).FirstOrDefault();
                    if (user==null)
                    {
                        ViewBag.Msg = "The mailbox you entered does not exist！";
                    }
                    else
                    {
                        HttpCookie aCookie = tools.CreateCookie("Email", Email);
                        Response.Cookies.Add(aCookie);
                        return RedirectToActionPermanent("AnswerQue");
                    }
                }
                else
                {
                    ViewBag.Msg = "Please enter the correct email format！";

                }
            }
            return View();

        }
        [HttpGet]
        public ActionResult AnswerQue()
        {
            HttpCookie aCookie = Request.Cookies["Email"];
            string Email = aCookie.Value;

            var user = db.User.Where(x => x.Email == Email).FirstOrDefault();
            List<string> list = new List<string>() {user.Que1,user.Que2 };

            ViewData["Que"] = list;

            return View();
        }
        [HttpPost]
        public ActionResult AnswerQue(string Que,string Ans)
        {

            HttpCookie aCookie = Request.Cookies["Email"];
            string Email = aCookie.Value;

            var user = db.User.Where(x => x.Email == Email).FirstOrDefault();
            List<string> list = new List<string>() { user.Que1, user.Que2 };

            ViewData["Que"] = list;

            

            var user1 = db.User.Where(x => x.Email == Email&&x.Que1==Que&&x.Ans1==Ans).FirstOrDefault();
            var user2 = db.User.Where(x => x.Email == Email && x.Que2 == Que && x.Ans2 == Ans).FirstOrDefault();
            if (user1==null)
            {
                ViewBag.Msg = "The answer is incorrect, please re-enter!";
            }
            else
            {
                return RedirectToAction("ChangePassword");
            }
            if (user2==null)
            {
                ViewBag.Msg = "The answer is incorrect, please re-enter!";
            }
            else
            {
               return RedirectToAction("ChangePassword");
            }

            return View();
        }
        [HttpGet]
        public ActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ChangePassword(string Password1,string Password2)
        {
            try
            {
                if (string.IsNullOrEmpty(Password1) && string.IsNullOrEmpty(Password2))
                {
                    ViewBag.Msg = "Can't be empty!";
                }
                else
                {
                    if (Password1 == Password2)
                    {
                        HttpCookie hc = Request.Cookies["Email"];
                        var user = db.User.Where(x => x.Email == hc.Value).FirstOrDefault();
                        user.UserPassWord = Password1;
                        db.Entry<User>(user).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();

                        return RedirectToActionPermanent("Login");
                    }
                    else
                    {
                        ViewBag.Msg = "Enter the password twice differently!";
                    }
                }
               
            }
            catch (Exception ex)
            {

            }
            return View();
        }

        public ActionResult Information()
        {
            HttpCookie UIDCookie = Request.Cookies["UID"];
            int uid =Convert.ToInt32(UIDCookie.Value);

            var user = db.User.Where(x => x.UID == uid).FirstOrDefault();

            return View(user);
        }

        [HttpPost]
        public ActionResult Information(User user)
        {
            HttpCookie UserCookie = tools.CreateCookie("UserName", user.UserName);
            Response.Cookies.Add(UserCookie);

            HttpCookie ImageCookie = tools.CreateCookie("UerImage", user.Image.ToString());
            Response.Cookies.Add(ImageCookie);

            
            db.Entry<User>(user).State = System.Data.Entity.EntityState.Modified;

            db.SaveChanges();
            return RedirectToAction("Index");
        }

         public ActionResult UpLoadProcess(HttpPostedFileBase imgFile)
        {

            Hashtable hash = new Hashtable();
            if (Request.Files.Count <= 0)
            {
                hash = new Hashtable();
                hash["error"] = 1;
                hash["message"] = "Please select a file.";
                return Json(hash);
            }
            imgFile = Request.Files[0];
            string fileTypes = "gif,jpg,jpeg,png,bmp";
            string fileName = imgFile.FileName;
            string fileExt = Path.GetExtension(fileName).ToLower();

            if (string.IsNullOrEmpty(fileExt) || Array.IndexOf(fileTypes.Split(','), fileExt.Substring(1).ToLower()) == -1)
            {
                hash = new Hashtable();
                hash["error"] = 1;
                hash["message"] = "Upload file extension is an extension that is not allowed.";
                return Json(hash);
            }

            string filePathName = string.Empty;
            string localPath = Path.Combine(HttpRuntime.AppDomainAppPath, "Resource");
            if (Request.Files.Count == 0)
            {
                return Json(new { jsonrpc = 2.0, error = new { code = 102, message = "Save failed" }, id = "id" });
            }
            string ex = Path.GetExtension(imgFile.FileName);
            filePathName = Guid.NewGuid().ToString("N") + ex;
            if (!System.IO.Directory.Exists(localPath))
            {
                System.IO.Directory.CreateDirectory(localPath);
            }
            imgFile.SaveAs(Path.Combine(localPath, filePathName));
            return Json(new
            {
                error = 0,
                message = "/Resource" + "/" + filePathName
            });

        }

        public ActionResult Logot()
        {
            #region DeleteCookie
            if (Request.Cookies["UID"]==null)
            {
                HttpCookie uid = tools.DeleteCookie("AID");
                Response.Cookies.Add(uid);
            }
            else
            {
                HttpCookie uid = tools.DeleteCookie("UID");
                Response.Cookies.Add(uid);
            }
            
            HttpCookie UserName = tools.DeleteCookie("UserName");
            Response.Cookies.Add(UserName);
            HttpCookie UerImage = tools.DeleteCookie("UerImage");
            Response.Cookies.Add(UerImage);
            #endregion

            return RedirectToAction("Index");
        }

        public ActionResult Status()
        {
             HttpCookie uidcookie = Request.Cookies["UID"];
            int uid = Convert.ToInt32(uidcookie.Value);

            var goods = from g in db.Goods
                         join u in db.UserGoods on
                         g.GID equals u.GID
                         where u.UID == uid
                         select new{ GID = u.GID, GoodName = g.GoodsName, OID = u.OID, UID = u.UID, Type = u.Type };

            List<UserGoodsOut> list = new List<UserGoodsOut>();
            foreach (var item in goods)
            {
                string str = string.Empty;
                string type = item.Type.Trim();
                if (type == "0")
                {
                    str = "Approval";
                }
                else if (type == "1")
                {
                    str = "Processing";
                }
                else if (type == "2")
                {
                    str = "Finish";
                }
                else
                {
                    str = "Delete";
                }
                list.Add(new UserGoodsOut() {OID=item.OID, GoodName = item.GoodName,  UID = item.UID, Type = str,GID=item.GID });
            }
            return View(list);
        }

        public ActionResult Post()
        {
            HttpCookie uidcookie = Request.Cookies["UID"];
            int uid = Convert.ToInt32(uidcookie.Value);

            var goods = from g in db.Goods
                        join u in db.UserGoods on
                        g.GID equals u.GID
                        where u.UID == uid
                        where u.Type=="1"
                        select g;


            return View(goods);
         
        }

        [HttpGet]
        public ActionResult Edit(int? GID)
        {
            var result = db.Goods.Where(x => x.GID == GID).FirstOrDefault();

            return View(result);
        }

        [HttpPost]
        public ActionResult Edit(Goods goods)
        {
            return View();
        }

        public ActionResult GetMessage()
        {
            string str_s = string.Empty;
            bool b_result = false;

            if (Request.Cookies["UID"]!=null)
            {
                HttpCookie uidcookie = Request.Cookies["UID"];
                int uid = Convert.ToInt32(uidcookie.Value);

                var user = db.User.Find(uid);
                string msg = user.Message;
                if (string.IsNullOrEmpty(msg))
                {
                    b_result = false;
                }
                else
                {
                    b_result = true;
                    user.Message = string.Empty;
                    db.Entry<User>(user).State = EntityState.Modified;
                    db.SaveChanges();
                    str_s = msg;
                }
            }
            return Content("{\"b_result\":\"" + b_result + "\",\"str_s\":\"" + str_s + "\"}", "json");
        }
    }
}