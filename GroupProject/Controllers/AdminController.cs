using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GroupProject.Controllers
{
    public class AdminController : Controller
    {
        rubricsEntities db = new rubricsEntities();



        // GET: Admin
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.docs_list = db.Documents.ToList(); 
            ViewBag.user_list = db.Users.ToList(); 
            ViewBag.roles_list = db.Roles.ToList(); 
            ViewBag.rubrics_list = db.Rubrics.ToList();

            //если пользователь не залогинился на начальной странице, а попытался просто ввести адрес этой страницы в адресной строке - мы не даем ему зайти, возвращаем на страницу логинки
            if (ViewBag.auth != 1) return View("~/Views/Home/Index.cshtml");
            return View();
        }


        [HttpPost]
        public ActionResult Index(string InputTopName, string InputTopPass, string InputTopRole)
        {
            int ro_id = 0, kk = 0,kkk=0;

            //Валидация (пустота полей)
            if (InputTopName == "") kkk++;
            else ViewBag.input_top_name_save = InputTopName;
            if (InputTopPass == "") kkk++;
            else ViewBag.input_top_pass_save = InputTopPass;
            if (InputTopRole == "") kkk++;
            else ViewBag.input_top_ro_save = InputTopRole;



            if (kkk != 0) { ViewBag.admin_validation_error = "Заповніть всі поля!"; }
            else
            {
                //Проверяем какую роль ввел пользователь
                switch (InputTopRole.ToLower())
                {
                    case "admin":
                        {
                            ro_id = 1; break;
                        }
                    case
                        "accessor":
                        {
                            ro_id = 2; break;
                        }
                    case "user":
                        {
                            ro_id = 3; break;
                        }
                    default:
                        {
                            ViewBag.admin_validation_error = "Такої ролі не існує!";
                            kk++;
                            break;
                        }
                }
                //Если такая роль существует - сохраняем новго пользователя
                if (kk == 0)
                {
                    Users user_to_add = new Users { UserName = InputTopName, UserPassword = InputTopPass, UserRoleID = ro_id };
                    bool can_add = true;

                    foreach (Users user in db.Users.ToList())
                    {
                        if ((user.UserName == user_to_add.UserName) && (user.UserPassword == user_to_add.UserPassword)) { can_add = false; }
                    }

                    if (can_add == true)
                    {
                        db.Users.Add(user_to_add);
                        db.SaveChanges();
                        ViewBag.admin_validation_error = "";
                    }
                    else
                    {
                        ViewBag.admin_validation_error = "Користувач з таким логіном та паролем вже існує!";
                    }
                }
            }
                ViewBag.user_list = db.Users.ToList();

                ViewBag.roles_list = db.Roles.ToList();
            
            return View();
        }
    }
}