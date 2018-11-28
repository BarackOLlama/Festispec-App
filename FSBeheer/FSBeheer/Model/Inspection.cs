using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace FSBeheer.Model
{
    public class Inspection
    {
        public Inspection()
        {
            Questionnaires = new ObservableCollection<Questionnaire>();
            Inspectors = new ObservableCollection<Inspector>();
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
        public int? InspectionDateId { get; set; }
        public virtual InspectionDate InspectionDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}
