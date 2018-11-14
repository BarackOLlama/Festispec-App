using FSBeheer.VM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

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
    }
}
