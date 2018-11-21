using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace FSBeheer.Model
{
    public partial class Answer
    {
        [Key]
        public int Id { get; set; }
        public string Content { get; set; }
        public int? QuestionId { get; set; }
        public virtual Question Question { get; set; }
        public int? InspectorId { get; set; }
        public virtual Inspector Inspector { get; set; }
        public bool IsDeleted { get; set; }
    }
}
