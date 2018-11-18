using FSBeheer.VM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSBeheer.Crud
{
    class QuestionnaireCrud : AbstractCrud
    {

        public QuestionnaireCrud(CustomFSContext customFSContext) : base(customFSContext)
        {

        }

        public ObservableCollection<QuestionnaireVM> GetQuestionnaires()
        {
            var questionnaire = CustomFSContext.Questionnaires
                .ToList()
                .Select(i => new QuestionnaireVM(i));
            var _questionnaire = new ObservableCollection<QuestionnaireVM>(questionnaire);
            return _questionnaire;
        }

    }
}
