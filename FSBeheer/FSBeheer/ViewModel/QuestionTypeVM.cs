using FSBeheer.Model;
using GalaSoft.MvvmLight;

namespace FSBeheer.ViewModel
{
    public class QuestionTypeVM : ViewModelBase
    {
        private QuestionType _questionType;

        public QuestionTypeVM(QuestionType questiontype)
        {
            _questionType = questiontype;
        }

        public string Name
        {
            get
            {
                return _questionType.Name;
            }
        }

        public override string ToString()
        {
            return Name;
        }

        internal QuestionType ToModel()
        {
            return _questionType;
        }
    }
}