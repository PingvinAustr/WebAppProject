using System.Web.Mvc;
using System.Web.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Windows;
using GroupProject.Models;


namespace GroupProject.Controllers
{
    public class UserController : Controller
    {

        // GET: Item
        [HttpGet]
        public ActionResult Index()
        {
            
            //Response.Write("[" + ViewBag.Abobus + "]");
            return View();
        }


        public void Aboba()
        {
            Response.Write("re");
        }


        public void Execute(RequestContext requestContext)
        {
            string ip = requestContext.HttpContext.Request.UserHostAddress;
            var response = requestContext.HttpContext.Response;
            //response.Write("<h2>Ваш IP-адрес: " + ip + "</h2>");
           
        }
    }
}