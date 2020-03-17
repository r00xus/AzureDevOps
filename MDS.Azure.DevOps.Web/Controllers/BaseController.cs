using MDS.Azure.DevOps.Core.Models.Config;
using MDS.Azure.DevOps.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace MDS.Azure.DevOps.Web.Controllers
{
    public class BaseController : Controller
    {
        protected ConfigBase GetConfig()
        {
            return new Config().Load();
        }
    }
}