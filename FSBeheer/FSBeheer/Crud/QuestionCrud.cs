using FSBeheer.VM;
using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Data.Entity;

namespace FSBeheer.Crud
{
    public class QuestionCrud : AbstractCrud
    {
        public QuestionCrud(CustomFSContext customFSContext) : base(customFSContext)
        {

        }
        public void Add(QuestionVM _question) => CustomFSContext.Questions.Add(_question.ToModel());

        public ObservableCollection<QuestionVM> GetAllQuestions()
        {
            var questions = CustomFSContext.Questions
                .Include(nameof(Model.QuestionType))
               .ToList()
               .Where(q => q.IsDeleted == false)
               .Select(c => new QuestionVM(c));
            return new ObservableCollection<QuestionVM>(questions);
        }

        public ObservableCollection<QuestionVM> GetAllQuestionsByQuestionnaire(QuestionnaireVM questionnaire)
        {
            var questions = CustomFSContext.Questions
                .Include(nameof(Model.QuestionType))
               .ToList()
               .Where(q => q.IsDeleted == false)
               .Where(q => q.Questionnaire.Id == questionnaire.Id)
               .Select(c => new QuestionVM(c));
            return new ObservableCollection<QuestionVM>(questions);
        }

        public QuestionVM GetQuestionById(int questionId)
        {
            var question = CustomFSContext.Questions
               .ToList()
               .Where(c => c.IsDeleted == false)
               .FirstOrDefault(c => c.Id == questionId);
            return new QuestionVM(question);
        }

        public ObservableCollection<QuestionVM> GetFilteredQuestionsByString(string must_contain)
        {
            if (must_contain == null)
            {
                throw new ArgumentNullException(nameof(must_contain));
            }
            var questions = CustomFSContext.Questions
                .ToList()
                .Where(c => c.IsDeleted == false)
                .Where(c =>
                c.Content.Contains(must_contain) ||
                c.QuestionType.Name.Equals(must_contain) ||
                c.Comments.Contains(must_contain) ||
                c.Id.Equals(must_contain)
                ).Distinct()
                .Select(c => new QuestionVM(c));

            var result = new ObservableCollection<QuestionVM>(questions);

            return result;

        }
    }
}
