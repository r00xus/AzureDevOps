using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MDS.Azure.DevOps.Core.Models.Config
{
    [DataContract]
    public class SpecialDay
    {
        [DataMember(Name = "description")]
        public string Description { get; set; }

        [DataMember(Name = "start")]
        public DateTime Start { get; set; }

        [DataMember(Name = "end")]
        public DateTime End { get; set; }

        [DataMember(Name = "hours")]
        public decimal Hours { get; set; }

    }
}
