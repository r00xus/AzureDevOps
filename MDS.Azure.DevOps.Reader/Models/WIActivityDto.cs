using System;

namespace MDS.Azure.DevOps.Reader.Models
{
    public class WIActivityDto : WIBaseDto
    {
        public string AssigndTo { get; set; }
        public DateTime? TargetDate { get; set; }
        public WITaskDto Task { get; set; } = new WITaskDto();
        public decimal CompletedWork { get; set; }
        public string State { get; set; }
        public string AreaPath { get; set; }
    }
}
