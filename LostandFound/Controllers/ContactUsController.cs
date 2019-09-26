using LostandFound.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LostandFound.Controllers
{
    public class ContactUsController : Controller
    {
        // GET: ContactUs
        private Model1 db = new Model1();
        public LostandFound.Tools.Tools tools = new LostandFound.Tools.Tools();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SubmitMessage(string msg)
        {
            string b_result = string.Empty;
            if (Request.Cookies["UID"]==null)
            {
                b_result = "You are not logged in, please log in first, please do this!";
            }
            else
            {
                HttpCookie httpCookie = Request.Cookies["UID"];
                int uid = Convert.ToInt32(httpCookie.Value);
                db.Message.Add(new Message() { UID = uid, Message1 = msg });
                db.SaveChanges();
                b_result = "The information has been submitted, thank you for your valuable comments!";
            }
            return Content("{\"b_result\":\"" + b_result + "\"}", "json");
        }
    }
}