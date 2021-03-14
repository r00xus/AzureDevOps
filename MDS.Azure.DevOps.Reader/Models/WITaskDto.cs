using System;

namespace MDS.Azure.DevOps.Reader.Models
{
    public class WITaskDto : WIBaseDto
    {
        public string mdsTaskWorkType { get; set; }

        public string mdsTaskActive { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? FinishDate { get; set; }

        public string mdsTaskDescription1 { get; set; }

        public string mdsTaskDescription2 { get; set; }

        public string State { get; set; }

        public decimal CompletedWork { get; set; }

        public decimal OriginalEstimate { get; set; }

        public string ProjectOnlineName { get; set; }
    }
}
