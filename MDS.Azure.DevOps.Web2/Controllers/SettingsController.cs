using System.Linq;
using System.Web.Mvc;
using MDS.Azure.DevOps.Core.Models.Config;
using MDS.Azure.DevOps.Web.Models;
using MDS.Azure.DevOps.Web2.ActionResults;

namespace MDS.Azure.DevOps.Web.Controllers
{
    public class SettingsController : BaseController
    {
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
                .Select(x => new { value = x.Name, text = x.NameShort }).ToList();

            return Json(employees);
        }
    }
}