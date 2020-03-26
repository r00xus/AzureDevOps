using MDS.Azure.DevOps.Excel.Attributes;
using System;

namespace MDS.Azure.DevOps.Core.Models
{
    [ListInfo(Title = "Оценка времени")]
    public class RITaskEstimate
    {
        [FieldInfo(Title = "ID Task")]
        public int? TaskId { get; set; }

        [FieldInfo(Title = "Название Task")]
        public string TaskName { get; set; }

        [FieldInfo(Title = "Дата начала работы из Task")]
        public DateTime? Start { get; set; }

        [FieldInfo(Title = "Дата окончания работы из Task")]
        public DateTime? End { get; set; }

        [FieldInfo(Title = "Название постановки")]
        public string SpecName { get; set; }

        [FieldInfo(Title = "Постановщик")]
        public string Analytic { get; set; }

        [FieldInfo(Title = "Дата оценки", Format = "dd.MM.yyyy")]
        public DateTime Date { get; set; }

        [FieldInfo(Title = "ФИО Программиста")]
        public string Developer { get; set; }

        [FieldInfo(Title = "Ответственный за проект")]
        public string Reviewer { get; set; }

        [FieldInfo(Title = "Срок, оценил программист")]
        public decimal EstimateDeveloper { get; set; }

        [FieldInfo(Title = "Срок, оценил ответственный за проект")]
        public decimal EstimateReviewer { get; set; }

        [FieldInfo(Title = "Срок, фактический")]
        public decimal EstimateFact { get; set; }
    }
}
