using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDS.Azure.DevOps.Core.Models.Config
{
    public class CustomDay
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public DayOfWeek DayOfWeek { get; set; }

        public decimal Hours { get; set; }
    }
}
