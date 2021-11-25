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
            ViewBag.docs_list = db.Documents.ToList();
            ViewBag.rubrics_list = db.Rubrics.ToList();

            ViewBag.hint_text_bottom_box = "Всі доступні документи:";
            //Response.Write("[" + ViewBag.hint_text_bottom_box + "]");
            return View();
        }

        [HttpPost]
        public ActionResult Index(string InputTop)
        {
            if (InputTop=="") ViewBag.docs_list = db.Documents.ToList();
            if (InputTop != "")
            {
                string rubric_name="";
                List<Documents> docs_after_search = new List<Documents> { };
                List<Documents> docs_after_search_rubrics = new List<Documents> { };
                string[] rubrics_id = new string[50]; //массив рубрик тех книг, которые в своем названии или описании содержат текст запроса
                int num_of_added_rubrics = 0;

                foreach (Documents doc in db.Documents.ToList())
                {

                    foreach (Rubrics rub in db.Rubrics.ToList())
                    {
                        if (doc.Document_RubricID == rub.Rubric_ID) rubric_name = rub.Rubric_Name;
                    }


                    if ((doc.Document_Name.IndexOf(InputTop) != -1) || (doc.Document_Description.IndexOf(InputTop) != -1)|| (rubric_name.IndexOf(InputTop)!=-1))
                    {
                        docs_after_search.Add(doc);
                        rubrics_id[num_of_added_rubrics] = rubric_name;
                        num_of_added_rubrics++;
                    }

                }

                foreach (Documents doc in db.Documents.ToList())
                {
                    string rub_name = "";
                    int k = 0; //если этот документ уже есть в списке - мы его не выбираем второй раз
                    foreach (Documents dc in docs_after_search)
                    {
                        if (doc.Document_ID == dc.Document_ID) k++;
                    }

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

                        if (kk > 0) docs_after_search_rubrics.Add(doc);
                    }

                    
                }



                //Отсортируем полученные новоизбранные документы по убыванию дат создания
                var ordered_rubrics = from u in docs_after_search_rubrics
                                      orderby u.Document_CreationDate descending
                                      select u;





                /*
                foreach (Documents doc in docs_after_search_rubrics)
                {
                    Response.Write(doc.Document_ID + " " + doc.Document_Name + " "+doc.Document_CreationDate+"   |  ");
                }
                Response.Write("Sorted");
                foreach (Documents doc in ordered_rubrics)
                {
                    Response.Write(doc.Document_ID + " " + doc.Document_Name + " " + doc.Document_CreationDate + "   |  ");

                }
                */
                docs_after_search.AddRange(ordered_rubrics);


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