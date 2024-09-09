using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TraahvIndividual.Models;
using System.Net;
using System.Net.Mail;
using OfficeOpenXml;
using OfficeOpenXml.Table;
using System.Globalization;
using System.Threading;

namespace TraahvIndividual.Controllers
{
    public class HomeController : Controller
    {
        TrahvidContext db = new TrahvidContext();
        public ActionResult Index(string lang)
        {

            string selectedLang = !string.IsNullOrEmpty(lang) ? lang : "est";
            ViewBag.Language = selectedLang;

            CultureInfo newCulture = new CultureInfo(selectedLang);
            Thread.CurrentThread.CurrentCulture = newCulture;
            Thread.CurrentThread.CurrentUICulture = newCulture;
            HttpContext.Items["lang"] = selectedLang;

            return View();

            //var penalties = db.Traahv.AsQueryable();  // Запрос к базе данных

            //// Если введен номер машины для поиска, отфильтруем результаты
            //if (!string.IsNullOrEmpty(searchCarNumber))
            //{
            //    penalties = penalties.Where(p => p.SoidukeNumber.Contains(searchCarNumber));
            //}

            //return View(penalties.ToList());  // Возвращаем результат в представление
        }



        //[AuthorizeUser("Admin2@gmail.com")]
        public ActionResult Traahv(string lang)
        {
            var translations = new Dictionary<string, Dictionary<string, string>>
    {
        { "ru", new Dictionary<string, string>
            {
                { "VehicleNumber", "Номер автомобиля" },
                { "OwnerName", "Имя владельца" },
                { "OwnerEmail", "Электронная почта владельца" },
                { "ViolationDate", "Дата нарушения" },
                { "Speeding", "Превышение скорости" },
                { "FineAmount", "Размер штрафа" },
                { "AddNew", "Добавить новый" },
                { "Edit", "Изменить" },
                { "Delete", "Удалить" },
            }
        },
        { "eng", new Dictionary<string, string>
            {
                { "VehicleNumber", "Vehicle Number" },
                { "OwnerName", "Owner's Name" },
                { "OwnerEmail", "Owner's Email" },
                { "ViolationDate", "Violation Date" },
                { "Speeding", "Speeding" },
                { "FineAmount", "Fine Amount" },
                { "AddNew", "Add New" },
                { "Edit", "Edit" },
                { "Delete", "Delete" },
            }
        },
        { "est", new Dictionary<string, string>
            {
                { "VehicleNumber", "Sõiduki Number" },
                { "OwnerName", "Omaniku Nimi" },
                { "OwnerEmail", "Omaniku Epost" },
                { "ViolationDate", "Rikkumisekuupaev" },
                { "Speeding", "Kiiruse Ületamine" },
                { "FineAmount", "Trahvi Suurus" },
                { "AddNew", "Lisa uus" },
                { "Edit", "Muuda" },
                { "Delete", "Kustuta" },
            }
        }
    };

            // Выбираем язык
            string selectedLang = !string.IsNullOrEmpty(lang) ? lang : "est";
            ViewBag.Language = selectedLang;

            // Передаем переведенные строки в представление
            ViewBag.Translations = translations[selectedLang];

            IEnumerable<Traahv> traahvs = db.Traahv.ToList();
            return View(traahvs);
        }
        //[Authorize]
        public ActionResult TraahvUsers(string searchCarNumber = null)
        {
            var penalties = db.Traahv.AsQueryable();  // Запрос к базе данных

            // Если введен номер машины для поиска, отфильтруем результаты
            if (!string.IsNullOrEmpty(searchCarNumber))
            {
                penalties = penalties.Where(p => p.SoidukeNumber.Contains(searchCarNumber));
            }

            return View(penalties.ToList());
        }
        [HttpGet]
        public ActionResult CreateTraahv()
        {

            return View();
        }


        public ActionResult CreateTraahv(Traahv trahv)
        {
            trahv.CalculateFine();
            db.Traahv.Add(trahv);
            db.SaveChanges();
            trahv.SendMessage();
            return RedirectToAction("Traahv");
        }

        [HttpGet]
        public ActionResult EditTraahv(int? id)
        {
            Traahv g = db.Traahv.Find(id);
            if (g == null)
            {
                return HttpNotFound();

            }
            return View(g);
        }
        [HttpPost, ActionName("EditTraahv")]
        public ActionResult EditTraahvConfirmed(Traahv guest)
        {
            guest.CalculateFine();
            db.Entry(guest).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Traahv");
        }

        public ActionResult DeleteTraahv(int id)
        {
            Traahv d = db.Traahv.Find(id);
            if (d == null)
            {
                return HttpNotFound();

            }
            return View(d);
        }
        [HttpPost, ActionName("DeleteTraahv")]
        public ActionResult DeleteTraahvConfirmed(int id)
        {
            Traahv d = db.Traahv.Find(id);
            if (d == null)
            {
                return HttpNotFound();
            }

            db.Traahv.Remove(d);
            db.SaveChanges();
            return RedirectToAction("Traahv");
        }
        //public ActionResult Index(string searchCarNumber = null)
        //{
        //    var penalties = db.Traahv.AsQueryable();  // Запрос к базе данных

        //    // Если введен номер машины для поиска, отфильтруем результаты
        //    if (!string.IsNullOrEmpty(searchCarNumber))
        //    {
        //        penalties = penalties.Where(p => p.SoidukeNumber.Contains(searchCarNumber));
        //    }

        //    return View(penalties.ToList());  // Возвращаем результат в представление
        //}
        public ActionResult TrahvSearch(string searchCarNumber = null)
        {
            var penalties = db.Traahv.AsQueryable();  // Запрос к базе данных

            // Если введен номер машины для поиска, отфильтруем результаты
            if (!string.IsNullOrEmpty(searchCarNumber))
            {
                penalties = penalties.Where(p => p.SoidukeNumber.Contains(searchCarNumber));
            }

            return View(penalties.ToList());  // Возвращаем результат в представление
        }
        [HttpGet]
        public ActionResult DetailTrahv(int id)
        {
            Traahv g = db.Traahv.Find(id);
            if (g == null)
            {
                return HttpNotFound();
            }

            return View(g);
        }
        public ActionResult ExportToExcel()
        {
            var traahvData = GetTraahvData();

            // Устанавливаем лицензию
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Traahv Data");

                // Добавляем заголовки
                worksheet.Cells[1, 1].Value = "Soiduke Number";
                worksheet.Cells[1, 2].Value = "Omaniku Nimi";
                worksheet.Cells[1, 3].Value = "Omaniku Epost";
                worksheet.Cells[1, 4].Value = "Rikkumise kuupaev";
                worksheet.Cells[1, 5].Value = "Kiiruse Uletamine";
                worksheet.Cells[1, 6].Value = "Trahvi Suurus";

                // Заполняем данные
                int row = 2;
                foreach (var item in traahvData)
                {
                    worksheet.Cells[row, 1].Value = item.SoidukeNumber;
                    worksheet.Cells[row, 2].Value = item.OmanikuNimi;
                    worksheet.Cells[row, 3].Value = item.OmanikuEpost;
                    worksheet.Cells[row, 4].Value = item.Rikkumisekuupaev.ToString("yyyy-MM-dd");
                    worksheet.Cells[row, 5].Value = item.KiiruseUletamine;
                    worksheet.Cells[row, 6].Value = item.TrahviSuurus;
                    row++;
                }

                // Генерируем и возвращаем Excel-файл
                var excelData = package.GetAsByteArray();
                return File(excelData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Trahvid.xlsx");
            }
        }

        private IEnumerable<Traahv> GetTraahvData()
        {
            IEnumerable<Traahv> traahvs = db.Traahv.ToList();

            return traahvs;

        }

    }



}