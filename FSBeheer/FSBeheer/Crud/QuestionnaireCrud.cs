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

        public ObservableCollection<QuestionnaireVM> GetAllQuestionnaires()
        {
            var questionnaire = CustomFSContext.Questionnaires
                .ToList()
                .Where(i => i.IsDeleted == false)
                .Select(i => new QuestionnaireVM(i));
            return new ObservableCollection<QuestionnaireVM>(questionnaire);
        }

        public QuestionnaireVM GetQuestionnaireById(int id)
        {
            var questionnaire = CustomFSContext.Questionnaires
                .Where(q => q.IsDeleted == false)
                .FirstOrDefault(q => q.Id == id);
            return new QuestionnaireVM(questionnaire);
        }

        public ObservableCollection<QuestionnaireVM> GetAllQuestionnairesFiltered(string must_contain)
        {
            if (string.IsNullOrEmpty(must_contain))
            {
                throw new ArgumentNullException(nameof(must_contain));
            }

            must_contain = must_contain.ToLower();

            var questionnaires = CustomFSContext.Questionnaires
                .ToList()
                .Where(c => c.IsDeleted == false)
                .Where(e =>
                e.Id.ToString().ToLower().Contains(must_contain) ||
                e.Name.ToLower().Contains(must_contain) ||
                e.Version.ToString().ToLower().Contains(must_contain)
                ).Distinct()
                .Select(e => new QuestionnaireVM(e));
            return new ObservableCollection<QuestionnaireVM>(questionnaires);
        }
    }
}
