using System.IO;
using System.Linq;
using MDS.Azure.DevOps.Core;
using Microsoft.AspNetCore.Mvc;

namespace MDS.Azure.DevOps.Web.Controllers
{
    public class SettingsController : Controller
    {
        private string _fileName { get { return Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "appdata", "config.json"); } }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Load()
        {
            var config = AppConfig.Load(_fileName);

            return Json(config);
        }

        public IActionResult Save(AppConfig config)
        {
            config.Save(_fileName);

            return StatusCode(200);
        }

        public IActionResult Employees()
        {
            var config = AppConfig.Load(_fileName);

            var employees = config.Employees
                .Select(x => new { value = x.Name, text = x.NameShort }).ToList();

            return Json(employees);
        }
    }
}