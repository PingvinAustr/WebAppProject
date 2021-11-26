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

        rubricsEntities db = new rubricsEntities();


        // GET: Item
        [HttpGet]
        public ActionResult Index()
        {
            //получаем списки с данными из бд, которые отобразятся на страницу
            ViewBag.docs_list = db.Documents.ToList();
            ViewBag.rubrics_list = db.Rubrics.ToList();

            ViewBag.hint_text_bottom_box = "Всі доступні документи:";

            //если пользователь не залогинился на начальной странице, а попытался просто ввести адрес этой страницы в адресной строке - мы не даем ему зайти, возвращаем на страницу логинки
            if (ViewBag.auth != 1) return View("~/Views/Home/Index.cshtml");
          
            return View();
        }

        [HttpPost]
        public ActionResult Index(string InputTop)
        {
            //ОБЯЗАТЕЛЬНО СНАЧАЛА ОЗНАКОМИТЬСЯ С УСЛОВИЕМ 



            //Если поиск пустой - покажем все элементы (юзер-френдли)
            if (InputTop=="") ViewBag.docs_list = db.Documents.ToList();
            if (InputTop != "")
            {

                //если поиск не пустой  ЭТАП 1: НАЙТИ ДОКУМЕНТЫ КОТОРЫЕ СОДЕРЖАТ ТЕКСТ ЗАПРОСА
                string rubric_name="";
                List<Documents> docs_after_search = new List<Documents> { };
                List<Documents> docs_after_search_rubrics = new List<Documents> { };
                string[] rubrics_id = new string[50]; //массив рубрик тех книг, которые в своем названии или описании содержат текст запроса
                int num_of_added_rubrics = 0;

                foreach (Documents doc in db.Documents.ToList())
                {
                    //определим название рубрики текущего документа
                    foreach (Rubrics rub in db.Rubrics.ToList())
                    {
                        if (doc.Document_RubricID == rub.Rubric_ID) rubric_name = rub.Rubric_Name;
                    }


                    //если в названии, описании, либо же рубрике текущего документа есть текст запроса пользователя - запоминаем этот документ для дальнейшего отображения
                    if ((doc.Document_Name.IndexOf(InputTop) != -1) || (doc.Document_Description.IndexOf(InputTop) != -1)|| (rubric_name.IndexOf(InputTop)!=-1))
                    {
                        docs_after_search.Add(doc);
                        rubrics_id[num_of_added_rubrics] = rubric_name;
                        num_of_added_rubrics++;
                    }

                }



                //ЭТАП 2: ЕСЛИ ДОКУМЕНТ ОТНОСИТСЯ К РУБРИКЕ ДОКУМЕНТОВ, ВЫБРАННЫХ В ЭТАПЕ 1
                foreach (Documents doc in db.Documents.ToList())
                {
                    string rub_name = "";
                    int k = 0; //если этот документ уже есть в списке - мы его не выбираем второй раз
                    foreach (Documents dc in docs_after_search)
                    {
                        if (doc.Document_ID == dc.Document_ID) k++;
                    }


                    //получаем текстовое название рубрики
                    foreach (Rubrics rub in db.Rubrics.ToList())
                    {
                        if (doc.Document_RubricID == rub.Rubric_ID) rub_name = rub.Rubric_Name;
                    }


                    int kk = 0;
                    if (k == 0)  //если документ еще не был выбран
                    {
                        for (int i = 0; i < num_of_added_rubrics; i++)
                        {
                            if (rubrics_id[i] == rub_name) kk++;  //если этот документ соответствует по категории выбранным ранее
                        }

                        if (kk > 0) docs_after_search_rubrics.Add(doc);  //запоминаем его
                    }

                    
                }



                //Отсортируем полученные новоизбранные документы по убыванию дат создания (согласно условию)
                var ordered_rubrics = from u in docs_after_search_rubrics
                                      orderby u.Document_CreationDate descending
                                      select u;


                //Объединим документы из ЭТАПА 1 и ЭТАПА 2
                docs_after_search.AddRange(ordered_rubrics);

                //Выведем на экран
                ViewBag.docs_list = docs_after_search;
            }

            ViewBag.rubrics_list = db.Rubrics.ToList();
            ViewBag.hint_text_bottom_box = "Знайдено:";

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