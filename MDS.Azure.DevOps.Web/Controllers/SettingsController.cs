using System.Linq;
using MDS.Azure.DevOps.Core.Models.Config;
using Microsoft.AspNetCore.Mvc;

namespace MDS.Azure.DevOps.Web.Controllers
{
    public class SettingsController : BaseController
    {
        public IActionResult Load()
        {
            var config = GetConfig();

            return Json(config);
        }

        public IActionResult Save(ConfigBase config)
        {
            config.Save();

            return StatusCode(200);
        }

        public IActionResult Employees()
        {
            var config = GetConfig();

            var employees = config.Employees
                .Select(x => new { value = x.Name, text = x.NameShort }).ToList();

            return Json(employees);
        }
    }
}