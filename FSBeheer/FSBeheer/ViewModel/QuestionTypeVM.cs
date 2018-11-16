using FSBeheer.Model;
using GalaSoft.MvvmLight;

namespace FSBeheer.ViewModel
{
    public class QuestionTypeVM : ViewModelBase
    {
        private QuestionType e;
        private QuestionType _questionType;

        public QuestionTypeVM(QuestionType e)
        {
            this.e = e;
        }

        public string Name
        {
            get
            {
                return _questionType.Name;
            }
        }
    }
}