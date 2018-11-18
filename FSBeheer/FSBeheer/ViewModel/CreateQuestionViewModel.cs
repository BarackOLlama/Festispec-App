using FSBeheer.Model;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSBeheer.ViewModel
{
    public class CreateQuestionViewModel : ViewModelBase
    {
        private QuestionnaireVM _selectedQuestionnaireVM;
        private Question _question;
        private QuestionTypeVM _selectedQuestionType;
        public ObservableCollection<QuestionTypeVM> QuestionTypes { get; set; }

        public QuestionTypeVM SelectedQuestionType
        {
            get
            {
                return _selectedQuestionType;
            }
            set
            {
                _selectedQuestionType = value;
                base.RaisePropertyChanged("SelectedQuestionType");
            }
        }
        public CreateQuestionViewModel(QuestionnaireVM selectedQuestionnaireVM)
        {
            selectedQuestionnaireVM = _selectedQuestionnaireVM;

            _question = new Question();

            using (var context = new FSContext())
            {
                var temp = context.QuestionTypes.ToList().Select(e => new QuestionTypeVM(e));
                QuestionTypes = new ObservableCollection<QuestionTypeVM>(temp);
            }
        }

        public Question Question
        {
            get
            {
                return _question;
            }
            set
            {
                _question = value;
                base.RaisePropertyChanged("Question");
            }
        }


        public void InsertQuestion()
        {
            int questionnaireId = _selectedQuestionnaireVM.Id;
        }


    }
}
