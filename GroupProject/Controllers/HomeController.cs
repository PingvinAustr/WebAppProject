using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Windows;
using GroupProject.Models;

namespace GroupProject.Controllers
{
    public class HomeController : Controller
    {

        //create data context
        rubricsEntities db = new rubricsEntities();


        //[HttpGet]
        /*
        public ActionResult Signin(string test)
        {

            //CODE --> FORM
            ViewBag.UserID = 1;
            ViewData["Message"] = "HI MIR";
            return View();

        }
        
        [HttpPost]
        public string Signin(List<string> names)
        {
            //FORM --> CODE 

            // добавляем информацию о покупке в базу данных
            //db.Purchases.Add(purchase);
            // сохраняем в бд все изменения
            //db.SaveChanges();

           
            return "Спасибо," +  names.Count+ ", за покупку!";
        }
        */

        int UserIDSaved = 0, UserRoleIDSaved=0;


       


        [HttpGet]
        public ActionResult Index()
        {
            //Get all users from DB
            IEnumerable<Users> users = db.Users;

            //transfer all users to ViewBag
            ViewBag.Users = users;

            //Отобразится при инициализации страницы
            // ViewBag.newvar = "init";
            // return view
            return View();
        }


        [HttpPost]
        public ActionResult Index(string Input1, string Input2)
        {
            //Get all users from DB
            IEnumerable<Users> users = db.Users;

            //transfer all users to ViewBag
            ViewBag.Users = users;


            //Валидация двух полей ввода
            if (Input1 == "") ViewBag.validation_input1 = "*обов'язкове поле";
            if (Input2 == "") ViewBag.validation_input2 = "*обов'язкове поле";

            //Если одно из полей пустое, а второе - нет, то мы НЕ должны удалять контент уже заполненного поля
            if ((Input1 == "") && (Input2 != "")) ViewBag.to_save_second_input = Input2;
            if ((Input1 != "") && (Input2 == "")) ViewBag.to_save_first_input = Input1;

            
            int k = 0;
            //Если оба поля заполнены - проверяем правильность пароля и логина
            if ((Input1 != "") && (Input2 != ""))
            {
               
                var user_list = db.Users.ToList();
                foreach (Users user in user_list)
                {
                    if ((user.UserPassword == Input2) && (user.UserName == Input1))
                    {
                        UserIDSaved = user.UserID;
                        UserRoleIDSaved = int.Parse(user.UserRoleID.ToString());

                        k++;
                    }
                }
                //Если не нашлось соответствия Логин+Пароль введенных пользователем - выдаем сообщение об ошибке
                if (k == 0) { ViewBag.validation_input1 = "Неправильний логін або пароль!"; }
             

            }

            ViewBag.transfer_user_id = UserIDSaved;


            // return view
           if (k==0) return View();
           
           if ((k!=0)&&(UserRoleIDSaved==3)) return View("~/Views/User/Index.cshtml");
            return View();
        }

      


    }
}