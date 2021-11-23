using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Windows;
namespace GroupProject.Controllers
{
    public class HomeController : Controller
    {

        //create data context
        rubricsEntities db = new rubricsEntities();


        [HttpGet]
        public ActionResult Buy(int id)
        {

            //CODE --> FORM
            ViewBag.UserID = id;
            ViewData["Message"] = "HI MIR";
            return View();

        }
        
        [HttpPost]
        public string Buy()
        {
            //FORM --> CODE 

            // добавляем информацию о покупке в базу данных
            //db.Purchases.Add(purchase);
            // сохраняем в бд все изменения
            //db.SaveChanges();

           
            return "Спасибо," +   ", за покупку!";
        }
        




        public ActionResult Index()
        {
            //Get all users from DB
            IEnumerable<Users> users = db.Users;

            //transfer all users to ViewBag
            ViewBag.Users = users;

            // return view
            return View();
        }


    }
}