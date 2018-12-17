using FSBeheer.VM;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FSBeheer.ViewModel
{
    public class CreateEditQuestionViewModel : ViewModelBase
    {

        //variables and properties
        private CustomFSContext _context;
        private QuestionVM _question;
        public QuestionVM Question
        {
            get
            {
                return _question;
            }
            set
            {
                _question = value;
                base.RaisePropertyChanged(nameof(Question));
            }
        }

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
                base.RaisePropertyChanged(nameof(SelectedQuestionType));
            }
        }
        public ObservableCollection<QuestionTypeVM> QuestionTypes { get; set; }

        //commands
        public RelayCommand<Window> SaveQuestionChangesCommand { get; set; }
        public RelayCommand<Window> CreateQuestionCommand { get; set; }

        public RelayCommand<Window> CloseWindowCommand { get; set; }

        //constructors
        public CreateEditQuestionViewModel(int questionnaireId)
        {
            //create
            _context = new CustomFSContext();
            InitializeQuestionTypes();
            InitializeCommands();

            _question = new QuestionVM();
            _question.Type = _selectedQuestionType.ToModel();
            _question.QuestionnaireId = questionnaireId;


        }

        public CreateEditQuestionViewModel(int questionnaireId, int questionId)
        {
            //edit
            _context = new CustomFSContext();
            InitializeQuestionTypes();
            InitializeCommands();

            var question = _context.Questions
                .ToList()
                .Where(e => e.Id == questionId)
                .FirstOrDefault();
            _question = new QuestionVM(question);


        }

        //methods
        private void InitializeCommands()
        {
            SaveQuestionChangesCommand = new RelayCommand<Window>(SaveQuestionChanges);
            CreateQuestionCommand = new RelayCommand<Window>(CreateQuestion);
            CloseWindowCommand = new RelayCommand<Window>(CloseWindow);
        }

        private void InitializeQuestionTypes()
        {
            var types = _context
                .QuestionTypes
                .ToList()
                .Select(e => new QuestionTypeVM(e));
            QuestionTypes = new ObservableCollection<QuestionTypeVM>(types);
            _selectedQuestionType = QuestionTypes.FirstOrDefault();
            base.RaisePropertyChanged(nameof(SelectedQuestionType));
        }
        public void SaveQuestionChanges(Window window)
        {
            MessageBoxResult result = MessageBox.Show("Opslaan wijzigingen?", "Bevestiging", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                _context.SaveChanges();
                Messenger.Default.Send(true, "UpdateQuestions");
                window.Close();
            }

        }

        public void CreateQuestion(Window window)
        {

            var result = MessageBox.Show("Opslaan?", "Bevestiging", MessageBoxButton.OKCancel);

            if (result == MessageBoxResult.OK)
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
                Messenger.Default.Send(true, "UpdateQuestions");
                window.Close();
            }
        }


        public void CloseWindow(Window window)
        {
            MessageBoxResult result = MessageBox.Show("Aanpassing annuleren?", "Bevestig", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                window.Close();
            }
            //Must close the window it's opened upon confirmation
            //no need to discard changes as the context is separate from the previous window's.
        }
    }
}
