using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace FSBeheer.Model
{
    public partial class Questionnaire
    {
        public Questionnaire()
        {
            Questions = new ObservableCollection<Question>();
        }

        public int Id { get; set; }
        [MaxLength(99)]
        public string Name { get; set; }
        public string Instructions { get; set; }
        public int Version { get; set; }
        public string Comments { get; set; }
        public int? InspectionId { get; set; }
        public virtual Inspection Inspection { get; set; }
        public virtual ObservableCollection<Question> Questions { get; set; }
        public bool IsDeleted { get; set; }
    }
}
