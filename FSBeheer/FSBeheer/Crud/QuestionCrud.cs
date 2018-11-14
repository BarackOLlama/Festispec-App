using FSBeheer.VM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Data.Entity;

namespace FSBeheer.Crud
{
    class QuestionCrud : AbstractCrud
    {
        public QuestionCrud(CustomFSContext customFSContext) : base(customFSContext)
        {

        }
        public void Add(QuestionVM _question) => CustomFSContext.Questions.Add(_question.ToModel());

        public ObservableCollection<QuestionVM> GetAllQuestionVMs()
        {
            var questions = CustomFSContext.Questions
               .ToList()
               .Select(c => new QuestionVM(c));
            var result = new ObservableCollection<QuestionVM>(questions);

            return result;
        }

        public ObservableCollection<QuestionVM> GetQuestionById(int questionId)
        {
            var questions = CustomFSContext.Questions
               .ToList()
               .Where(c => c.Id == questionId)
               .Select(c => new QuestionVM(c));
            var result = new ObservableCollection<QuestionVM>(questions);

            return result;
        }

        public void Modify(QuestionVM question)
        {
            CustomFSContext.Entry(question?.ToModel()).State = EntityState.Modified;
            CustomFSContext.SaveChanges();
        }

        public void Delete(QuestionVM question)
        {
            CustomFSContext.Questions.Attach(question?.ToModel());
            CustomFSContext.Questions.Remove(question?.ToModel());
            CustomFSContext.SaveChanges();
        }

        public ObservableCollection<QuestionVM> GetFilteredQuestionsByString(string must_contain)
        {
            if (must_contain == null)
            {
                throw new ArgumentNullException(nameof(must_contain));
            }
            var questions = CustomFSContext.Questions
                .ToList()
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
