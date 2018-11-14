using FSBeheer.Model;

namespace FSBeheer.ViewModel
{
    public class QuestionnaireVM
    {
        private Questionnaire _questionnaire;

        public QuestionnaireVM(Questionnaire questionnaire)
        {
            _questionnaire = questionnaire;
        }

        public int Id
        {
            get
            {
                return _questionnaire.Id;
            }
        }

        public string Name
        {
            get
            {
                return _questionnaire.Name;
            }
        }
    }
}