using System.Linq;
using System.Web.Mvc;
using MDS.Azure.DevOps.Core.Models.Config;
using MDS.Azure.DevOps.Web.Models;
using MDS.Azure.DevOps.Web2.ActionResults;

namespace MDS.Azure.DevOps.Web.Controllers
{
    public class SettingsController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Load()
        {
            var config = GetConfig();

            return new JsonNetResult
            {
                Data = config
            };
        }

        public ActionResult Save(Config config)
        {
            config.Save();

            return new HttpStatusCodeResult(200);
        }

        public ActionResult Employees()
        {
            var config = GetConfig();

            var employees = config.Employees
                .OrderBy(x => x.Name)
                .Select(x => new { value = x.Name, text = x.Name }).ToList();

            return Json(employees);
        }
    }
}