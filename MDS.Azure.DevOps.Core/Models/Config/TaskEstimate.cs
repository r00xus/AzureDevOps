using System;
using System.Runtime.Serialization;

namespace MDS.Azure.DevOps.Core.Models.Config
{
    [DataContract]
    public class TaskEstimate
    {
        [DataMember(Name = "specName")]
        public string SpecName { get; set; }

        [DataMember(Name = "analytic")]
        public string Analytic { get; set; }

        [DataMember(Name = "developer")]
        public string Developer { get; set; }

        [DataMember(Name = "reviewer")]
        public string Reviewer { get; set; }

        [DataMember(Name = "estimateReviewer")]
        public decimal EstimateReviewer { get; set; }

        [DataMember(Name = "estimateDeveloper")]
        public decimal EstimateDeveloper { get; set; }

        [DataMember(Name = "taskId")]
        public int? TaskId { get; set; }

        [DataMember(Name = "date")]
        public DateTime Date { get; set; }
    }
}
