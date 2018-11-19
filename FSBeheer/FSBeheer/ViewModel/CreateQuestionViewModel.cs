using FSBeheer.Model;
using FSBeheer.VM;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FSBeheer.ViewModel
{
    public class CreateQuestionViewModel : ViewModelBase
    {
        public QuestionVM Question { get; set; }
        private QuestionnaireVM _selectedQuestionnaireVM;
        private Question _question;
        private QuestionTypeVM _selectedQuestionType;
        public ObservableCollection<QuestionTypeVM> QuestionTypes { get; set; }
        public RelayCommand AddQuestionCommand { get; set; }

        public CreateQuestionViewModel(QuestionnaireVM selectedQuestionnaireVM)
        {
            _selectedQuestionnaireVM = selectedQuestionnaireVM;

            _question = new Question();

            using (var context = new CustomFSContext())
            {
                var temp = context.QuestionTypes.ToList().Select(e => new QuestionTypeVM(e));
                QuestionTypes = new ObservableCollection<QuestionTypeVM>(temp);
                SelectedQuestionType = QuestionTypes?.First();
            }

            AddQuestionCommand = new RelayCommand(AddQuestion, CanAddQuestion);
            Question = new QuestionVM();
        }


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

        public void AddQuestion()
        {
            using (var context = new FSContext())
            {

            }
        }

        public bool CanAddQuestion()
        {
            return true;
        }

    }
}
