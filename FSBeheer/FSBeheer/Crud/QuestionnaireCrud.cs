using FSBeheer.ViewModel;
using FSBeheer.VM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSBeheer.Crud
{
    public class QuestionnaireCrud : AbstractCrud
    {

        public QuestionnaireCrud(CustomFSContext customFSContext) : base(customFSContext)
        {

        }

        public ObservableCollection<QuestionnaireVM> GetAllQuestionnaireVMs()
        {
            var questionnaire = CustomFSContext.Questionnaires
                .ToList()
                .Select(i => new QuestionnaireVM(i));
            var _questionnaire = new ObservableCollection<QuestionnaireVM>(questionnaire);
            return _questionnaire;
        }

        public void Add(QuestionnaireVM _questionnaire)
        {
            CustomFSContext.Questionnaires.Add(_questionnaire.ToModel());
            CustomFSContext.SaveChanges();
        }

    }
}
