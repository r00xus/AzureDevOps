using MDS.Azure.DevOps.Core.Models.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MDS.Azure.DevOps.Web.Models
{
    public class Config : ConfigBase
    {
        protected override string FilePath
        {
            get
            {
                return Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "appdata");
            }
        }
    }
}