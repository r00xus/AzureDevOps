using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MDS.Azure.DevOps.Core.Models
{
    [DataContract]
    public class WICustomField
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "projectOnlineName")]
        public string ProjectOnlineName { get; set; }
    }
}
