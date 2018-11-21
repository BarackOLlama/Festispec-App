using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSBeheer.Model
{
    public class Inspection
    {
        public Inspection()
        {
            Questionnaires = new ObservableCollection<Questionnaire>();
            Inspectors = new ObservableCollection<Inspector>();
            InspectionDates = new ObservableCollection<InspectionDate>();
        }

        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Notes { get; set; }
        public int? EventId { get; set; }
        public virtual Event Event { get; set; }
        public int? StatusId { get; set; }
        public virtual Status Status { get; set; }
        public virtual ObservableCollection<Questionnaire> Questionnaires { get; set; }
        public virtual ObservableCollection<Inspector> Inspectors { get; set; }
        public virtual ObservableCollection<InspectionDate> InspectionDates { get; set; }
        public bool IsDeleted { get; set; }
    }
}
