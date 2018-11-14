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
    }
}
