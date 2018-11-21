using FSBeheer.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSBeheer.Crud
{
    class QuestionTypeCrud : AbstractCrud
    {
        public QuestionTypeCrud(CustomFSContext customFSContext) : base(customFSContext)
        {
        }

        public void Add(QuestionTypeVM questionType) => CustomFSContext.QuestionTypes.Add(questionType.ToModel());

        public ObservableCollection<QuestionTypeVM> GetQuestionTypeVMs()
        {
            var questiontypes = CustomFSContext.QuestionTypes
                .ToList()
                .Select(e => new QuestionTypeVM(e));
            return new ObservableCollection<QuestionTypeVM>(questiontypes);
        }


    }
}
