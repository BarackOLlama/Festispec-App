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
        }

        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string State { get; set; }
        public string Notes { get; set; }
        public ObservableCollection<Questionnaire> Questionnaires { get; set; }
    }
}
