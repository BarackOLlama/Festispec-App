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
        private CustomFSContext _context;

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
            _context = new CustomFSContext();

            var temp = _context.QuestionTypes.ToList().Select(e => new QuestionTypeVM(e));
            QuestionTypes = new ObservableCollection<QuestionTypeVM>(temp);
            SelectedQuestionType = QuestionTypes?[1];

            AddQuestionCommand = new RelayCommand(AddQuestion, CanAddQuestion);
            Question = new QuestionVM();
            Question.Type = _selectedQuestionType.ToModel();
            Question.QuestionnaireId = selectedQuestionnaireVM.Id;
        }

        public void AddQuestion()
        {

            //clear irrelevant fields to avoid confusion in case the user mistakenly filled them in.
            if (SelectedQuestionType.Name == "Multiple Choice Vraag")
            {//clear columns
                Question.Columns = null;
            }
            else if (SelectedQuestionType.Name == "Open Vraag")
            {//clear options and columns
                Question.Columns = null;
                Question.Options = null;
            }
            else if (SelectedQuestionType.Name == "Open Tabelvraag")
            {//clear options
                Question.Options = null;
            }
            _context.Questions.Add(Question.ToModel());
            _context.SaveChanges();
        }

        public bool CanAddQuestion()
        {
            return true;
        }

    }
}