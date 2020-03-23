using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MDS.Azure.DevOps.Core.Models.Config
{
    [DataContract]
    public class CustomDay
    {
        [JsonConverter(typeof(StringEnumConverter)), DataMember(Name = "dayOfWeek")]
        public DayOfWeek DayOfWeek { get; set; }
        
        [DataMember(Name = "hours")]
        public decimal Hours { get; set; }
    }
}
