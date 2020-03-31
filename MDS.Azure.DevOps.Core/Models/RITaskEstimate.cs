using MDS.Azure.DevOps.Excel.Attributes;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Runtime.Serialization;

namespace MDS.Azure.DevOps.Core.Models
{
    [ListInfo(Title = "Оценка времени")]
    [DataContract]
    public class RITaskEstimate
    {
        [FieldInfo(Title = "ID Task")]
        [DataMember(Name = "taskId")]
        public int? TaskId { get; set; }

        [FieldInfo(Title = "Название Task", Width = 40)]
        [DataMember(Name = "taskName")]
        public string TaskName { get; set; }

        [FieldInfo()]
        [DataMember(Name = "state")]
        public string State { get; set; }

        [FieldInfo(Title = "Дата начала работы из Task", Format = "dd.MM.yyyy", Width = 10)]
        [DataMember(Name = "start")]
        public DateTime? Start { get; set; }

        [FieldInfo(Title = "Дата окончания работы из Task", Format = "dd.MM.yyyy", Width = 10)]
        [DataMember(Name = "end")]
        public DateTime? End { get; set; }

        [FieldInfo(Title = "Название постановки", Width = 40)]
        [DataMember(Name = "specName")]
        public string SpecName { get; set; }

        [FieldInfo(Title = "Постановщик")]
        [DataMember(Name = "analytic")]
        public string Analytic { get; set; }

        [FieldInfo(Title = "Дата оценки", Format = "dd.MM.yyyy")]
        [DataMember(Name = "date")]
        public DateTime? Date { get; set; }

        [FieldInfo(Title = "ФИО Программиста")]
        [DataMember(Name = "developer")]
        public string Developer { get; set; }

        [FieldInfo(Title = "Ответственный за проект")]
        [DataMember(Name = "reviewer")]
        public string Reviewer { get; set; }

        [FieldInfo(NoDisplay = true)]
        [DataMember(Name = "estimateDeveloperHours")]
        public decimal? EstimateDeveloperHours { get; set; }

        [FieldInfo(Title = "Срок, оценки программист", HorizontalAlignment = HorizontalAlignmentValues.Right, Width = 15)]
        [DataMember(Name = "estimateDeveloperDays")]
        public decimal? EstimateDeveloperDays { get { return EstimateDeveloperHours / 8; } }

        [FieldInfo(NoDisplay = true)]
        [DataMember(Name = "estimateReviewerHours")]
        public decimal? EstimateReviewerHours { get; set; }

        [FieldInfo(Title = "Срок, оценил ответственный за проект", HorizontalAlignment = HorizontalAlignmentValues.Right, Width = 15)]
        [DataMember(Name = "estimateReviewerDays")]
        public decimal? EstimateReviewerDays { get { return EstimateReviewerHours / 8; } }

        [FieldInfo(NoDisplay = true)]
        [DataMember(Name = "estimateFactHours")]
        public decimal? EstimateFactHours { get; set; }

        [FieldInfo(Title = "Срок, фактический", HorizontalAlignment = HorizontalAlignmentValues.Right, Width = 15)]
        [DataMember(Name = "estimateFactDays")]
        public decimal? EstimateFactDays { get { return EstimateFactHours / 8; } }
    }
}
