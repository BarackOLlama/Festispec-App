using FSBeheer.Crud;
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
        private QuestionVM _questionVM;

        public QuestionVM Question
        {
            get
            {
                return _questionVM;
            }
            set
            {
                _questionVM = value;
                base.RaisePropertyChanged("Question");
            }
        }
        private QuestionnaireVM _selectedQuestionnaireVM;
        private QuestionTypeVM _selectedQuestionType;
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
                Question.Type = value.ToModel();
            }
        }
        public ObservableCollection<QuestionTypeVM> QuestionTypes { get; set; }
        public RelayCommand AddQuestionCommand { get; set; }

        public CreateQuestionViewModel(QuestionnaireVM selectedQuestionnaireVM)
        {
            _selectedQuestionnaireVM = selectedQuestionnaireVM;
            _questionVM = new QuestionVM();

            using (var context = new CustomFSContext())
            {
                var temp = context.QuestionTypes.ToList().Select(e => new QuestionTypeVM(e));
                QuestionTypes = new ObservableCollection<QuestionTypeVM>(temp);
                SelectedQuestionType = QuestionTypes?[1];
            }

            AddQuestionCommand = new RelayCommand(AddQuestion, CanAddQuestion);
            Question = new QuestionVM();
            Question.Type = _selectedQuestionType.ToModel();
            Question.
        }

        public void AddQuestion()
        {
            using (var context = new FSContext())
            {
                context.Questions.Add(Question.ToModel());
                context.SaveChanges();
            }
        }

        public bool CanAddQuestion()
        {
            return true;
        }

    }
}