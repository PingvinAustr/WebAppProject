using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GroupProject.Controllers
{
    public class AccessorController : Controller
    {
        rubricsEntities db = new rubricsEntities();
        
        // GET: Accessor
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.docs_list = db.Documents.ToList();
            ViewBag.rubrics_list = db.Rubrics.ToList();
            //если пользователь не залогинился на начальной странице, а попытался просто ввести адрес этой страницы в адресной строке - мы не даем ему зайти, возвращаем на страницу логинки
            if (ViewBag.auth != 1) return View("~/Views/Home/Index.cshtml");
            return View();
        }


        [HttpPost]
        public ActionResult Index(string InputTopDocName,string InputTopDocDesc, string InputTopDocRub, string InputTopDocDate, string InputTopDocAddress)
        {
            //Сохраняем введенные пользователем данные, чтобы при перезагрузке страницы не приходилось вводить все заново
            ViewBag.Input1_tosave = InputTopDocName;
            ViewBag.Input2_tosave = InputTopDocDesc;
            ViewBag.Input3_tosave = InputTopDocRub;
            ViewBag.Input4_tosave = InputTopDocDate;
            ViewBag.Input5_tosave = InputTopDocAddress;

            bool can_proceed1 = true;
            //Валидация №1 - Пустота полей
            if ((InputTopDocName == "") || (InputTopDocDesc == "") || (InputTopDocRub == "") || (InputTopDocDate == "") || (InputTopDocAddress == ""))
            {
                ViewBag.accessor_validation = "Заповніть всі поля!";
                can_proceed1 = false;
            }
            if (can_proceed1 == true)
            {
                int k = 0;
                //Валидация 2 - Проверка существования рубрики
                foreach (Rubrics rub in db.Rubrics.ToList())
                {
                    if (rub.Rubric_Name == InputTopDocRub) k++;
                }
                bool can_proceed2 = true;
                if (k == 0) { ViewBag.accessor_validation = "Такої рубрики не існує!"; can_proceed2 = false; }

                if (can_proceed2 == true)
                {
                    //Валидация 3 - Проверка формата даты
                    bool can_proceed3 = true;
                    try
                    {
                        DateTime.Parse(InputTopDocDate);
                    }
                    catch
                    {
                        can_proceed3 = false;
                    }

                    if (can_proceed3 == false) ViewBag.accessor_validation = "Неправильний формат дати!";
                    else
                    {
                        //Валидация 4 - Проверка дубликата названия
                        int rubric_id = 0;
                        foreach (Rubrics rub in db.Rubrics.ToList())
                        {
                            if (rub.Rubric_Name == InputTopDocRub) rubric_id = rub.Rubric_ID;
                        }


                        bool can_proceed4 = true;
                        foreach (Documents docs in db.Documents.ToList())
                        {
                            if (docs.Document_Name == InputTopDocName) { ViewBag.accessor_validation = "Документ з такою назвою вже існує!"; can_proceed4 = false; }
                        }


                        if (can_proceed4 == true)
                        {
                            //Если все этапы валидации пройдены - сохраняем документ и обнуляем поля ввода
                            Documents doc_to_add = new Documents { Document_Name = InputTopDocName, Document_Address = InputTopDocAddress, Document_CreationDate = InputTopDocDate, Document_Description = InputTopDocDesc, Document_RubricID = rubric_id };
                            db.Documents.Add(doc_to_add);
                            ViewBag.Input1_tosave = "";
                            ViewBag.Input2_tosave = "";
                            ViewBag.Input3_tosave = "";
                            ViewBag.Input4_tosave = "";
                            ViewBag.Input5_tosave = "";
                            db.SaveChanges();
                        }
                        
                    }
                }



            }
            ViewBag.docs_list = db.Documents.ToList();
            ViewBag.rubrics_list = db.Rubrics.ToList();

            return View();
        }


       
        public ActionResult IndexRemove(string InputRemove)
        {

           
            bool can_proceed = true;
            if (InputRemove == "")
            {
                ViewBag.remove_form = "Поле не заповнене";
                can_proceed = false;
            }
            if (can_proceed == true)
            {
                bool can_proceed1 = true;
                try
                {
                    int.Parse(InputRemove);
                }
                catch
                {
                    can_proceed1 = false;
                }

                if (can_proceed1==false) { ViewBag.remove_form = "Неправильний формат даних!"; }
                else
                {
                    bool can_proceed2 = false;
                    foreach(Documents docs in db.Documents.ToList())
                    {
                        if (docs.Document_ID == int.Parse(InputRemove)) can_proceed2 = true;
                    }
                    if (can_proceed2 == false) ViewBag.remove_form = "Документу з таким ID не існує!";
                    else
                    {
                        foreach (Documents docs in db.Documents.ToList())
                        {
                            if (docs.Document_ID == int.Parse(InputRemove)) db.Documents.Remove(docs);
                        }
                        db.SaveChanges();
                    }

                }
            }


            ViewBag.docs_list = db.Documents.ToList();
            ViewBag.rubrics_list = db.Rubrics.ToList();

            return View("~/Views/Accessor/Index.cshtml");
        }
    }
}