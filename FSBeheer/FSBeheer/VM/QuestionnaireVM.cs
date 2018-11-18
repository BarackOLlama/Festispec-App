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
            set { _questionnaire.Version = value; }
        }

        internal Questionnaire ToModel()
        {
            return _questionnaire;
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



        public int QuestionCount()
        {
            return Questions.Count;
        }

        //public Event Event()
        //{
            // need logic to get Event corresponding to Questionnaire
        //}

        /*
         *  Logic
         */
        public bool AddQuestion(Question question)
        {
            // If the given question is already in the questionnaire, don't add it to avoid duplicates.
            if (Questions.Contains(question))
            {
                return false;
            }

            Questions.Add(question);
            UpdateVersion();
            return true;
        }

        public bool RemoveQuestion(Question question)
        {
            if (Questions.Remove(question))
            {
                UpdateVersion();
                return true;
            }

            return false;
        }

        // Increment the version number by one
        private int UpdateVersion()
        {
            return _questionnaire.Version += 1;
        }
    }
}
