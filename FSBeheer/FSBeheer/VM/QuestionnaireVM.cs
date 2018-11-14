using FSBeheer.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSBeheer.VM
{
    public class QuestionnaireVM
    {
        private Questionnaire _questionnaire;

        public QuestionnaireVM(Questionnaire q)
        {
            _questionnaire = q;
        }

        public int Id
        {
            get { return _questionnaire.Id; }
        }

        public string Name
        {
            get { return _questionnaire.Name; }
        }

        public string Instructions
        {
            get { return _questionnaire.Instructions; }
        }

        public int Version
        {
            get { return _questionnaire.Version; }
        }

        public string Comments
        {
            get { return _questionnaire.Comments; }
        }

        public int? InspectionId
        {
            get { return _questionnaire.InspectionId; }
        }

        public virtual Inspection Inspection
        {
            get { return _questionnaire.Inspection; }
        }

        public virtual ObservableCollection<Question> Questions
        {
            get { return _questionnaire.Questions; }
        }
    }
}
