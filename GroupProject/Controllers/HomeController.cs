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

       
        rubricsEntities db = new rubricsEntities();
        //контекст базы данных

      
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

            //сохраним ID юзера
            ViewBag.transfer_user_id = UserIDSaved;


            // Если неправильный логин/пароль - заново загружаем страницу и просим ввести повторно
           if (k==0) return View();

           //если пароль и логин верны - открывем новую страницу согласно роли пользователя
            if ((k != 0) && (UserRoleIDSaved == 3)) {
                ViewBag.auth = 1;  ViewBag.hint_text_bottom_box = "Всі доступні документи:"; ViewBag.docs_list = db.Documents.ToList(); ViewBag.rubrics_list = db.Rubrics.ToList();
                return View("~/Views/User/Index.cshtml"); }
            if ((k != 0) && (UserRoleIDSaved == 1)) { ViewBag.auth = 1;  ViewBag.user_list = db.Users.ToList(); ViewBag.roles_list=db.Roles.ToList(); return View("~/Views/Admin/Index.cshtml"); }
            if ((k!=0) &&(UserRoleIDSaved==2)) { ViewBag.auth = 1;  ViewBag.forJS = 0; ViewBag.docs_list = db.Documents.ToList(); ViewBag.rubrics_list = db.Rubrics.ToList();  return View("~/Views/Accessor/Index.cshtml"); }

            return View();
        }

      


    }
}