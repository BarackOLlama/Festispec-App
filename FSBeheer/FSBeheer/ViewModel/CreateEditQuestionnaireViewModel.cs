using FSBeheer.View;
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
    public class CreateEditQuestionnaireViewModel :ViewModelBase
    {
        private QuestionnaireVM _questionnaire;
        private CustomFSContext _context;
        private int _questionnaireId;
        private int QuestionnaireId
        {
            get
            {
                if (Questionnaire != null)
                {
                    return Questionnaire.Id;
                }
                return _questionnaireId;
            }
            set
            {
                _questionnaireId = value;
            }
        }
        public QuestionnaireVM Questionnaire
        {
            get { return _questionnaire; }
            set
            {
                _questionnaire = value;
                base.RaisePropertyChanged("Questionnaire");
            }
        }

        private QuestionVM _selectedQuestion;
        public QuestionVM SelectedQuestion
        {
            get
            {
                return _selectedQuestion;
            }
            set
            {
                _selectedQuestion = value;
                base.RaisePropertyChanged("SelectedQuestion");
            }
        }
        public ObservableCollection<int?> InspectionNumbers { get; set; }
        public ObservableCollection<QuestionVM> Questions { get; set; }
        private int? _selectedInspectionNumber;
        public int? SelectedInspectionNumber
        {
            get { return _selectedInspectionNumber; }
            set
            {
                _selectedInspectionNumber = value;
                base.RaisePropertyChanged("SelectedInspectionNumber");
            }
        }

        public RelayCommand OpenCreateQuestionViewCommand { get; set; }
        public RelayCommand<Window> SaveQuestionnaireChangesCommand { get; set; }
        public RelayCommand OpenEditQuestionViewCommand { get; set; }
        public RelayCommand CreateQuestionnaireCommand { get; set; }

        public RelayCommand DeleteQuestionCommand { get; set; }
        public RelayCommand<Window> CloseWindowCommand { get; set; }

        public CreateEditQuestionnaireViewModel(int questionnaireId)
        {
            //edit
            QuestionnaireId = questionnaireId;
            Messenger.Default.Register<bool>(this, "UpdateQuestions", cl => Init());
            Init();
            var questionnaireEntity = _context.Questionnaires.ToList().Where(e => e.Id == questionnaireId).FirstOrDefault();
            Questionnaire = new QuestionnaireVM(questionnaireEntity);

            var questions = _context.Questions
                .Include("QuestionType")
                .ToList()
                .Where(e => e.QuestionnaireId == _questionnaire.Id && !e.IsDeleted)
                .Select(e => new QuestionVM(e));
            Questions = new ObservableCollection<QuestionVM>(questions);

            var inspectionNumbers = _context.Inspections
                .ToList()
                .Where(e => !e.IsDeleted)
                .Select(e => (int?)e.Id);
            InspectionNumbers = new ObservableCollection<int?>(inspectionNumbers);
            _selectedInspectionNumber = inspectionNumbers.FirstOrDefault();
            SelectedQuestion = questions.FirstOrDefault();

            InitializeCommands();
        }

        public CreateEditQuestionnaireViewModel()
        {
            //create
            Messenger.Default.Register<bool>(this, "UpdateQuestions", cl => Init());
            Init();
            _context = new CustomFSContext();
            _questionnaire = new QuestionnaireVM();
            InitializeCommands();
        }

        //methods

        private void InitializeCommands()
        {
            OpenCreateQuestionViewCommand = new RelayCommand(OpenCreateQuestionView);
            SaveQuestionnaireChangesCommand = new RelayCommand<Window>(SaveQuestionnaireChanges);
            OpenEditQuestionViewCommand = new RelayCommand(OpenEditQuestionView);
            DeleteQuestionCommand = new RelayCommand(DeleteQuestion);
            CreateQuestionnaireCommand = new RelayCommand(CreateQuestionnaire);
            CloseWindowCommand = new RelayCommand<Window>(CloseWindow);

        }

        internal void Init()
        {
            _context = new CustomFSContext();
            var questions = _context.Questions
                .ToList()
                .Where(e => e.QuestionnaireId == QuestionnaireId && !e.IsDeleted)
                .Select(e => new QuestionVM(e));
            Questions = new ObservableCollection<QuestionVM>(questions);
            base.RaisePropertyChanged("Questions");
        }

        private void CloseWindow(Window window)
        {
            var result = MessageBox.Show("Terug gaan zonder wijzigingen op te slaan?", "", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                window.Close();
            }
        }
        private void SaveQuestionnaireChanges(Window window)
        {
            MessageBoxResult result = MessageBox.Show("Opslaan wijzigingen?", "Bevestiging", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                _context.SaveChanges();
                Messenger.Default.Send(true, "UpdateQuestionnaires");
                window.Close();
            }
        }

        public void OpenCreateQuestionView()
        {
            new CreateQuestionView().ShowDialog();
        }

        private void OpenEditQuestionView()
        {
            if (_selectedQuestion == null)
            {
                MessageBox.Show("Geen vraag geselecteerd.");
            }
            else
            {
                new EditQuestionView().ShowDialog();
            }
        }

        public void UpdateQuestions()
        {
            var questions = _context.Questions.ToList().Select(e => new QuestionVM(e));
            Questions = new ObservableCollection<QuestionVM>(questions);
        }

        private void CreateQuestionnaire()
        {
            _context.Questionnaires.Add(Questionnaire.ToModel());
            _context.SaveChanges();
            Messenger.Default.Send(true, "UpdateQuestionnaires");
        }

        public void DeleteQuestion()
        {
            if (_selectedQuestion == null || _selectedQuestion.IsDeleted)
            {
                MessageBox.Show("Geen vraag geselecteerd.");
            }
            else
            {
                var result = MessageBox.Show("Vraag verwijderen?", "Verwijder", MessageBoxButton.OKCancel);
                if (result == MessageBoxResult.OK)
                {
                    _selectedQuestion.IsDeleted = true;
                    _context.SaveChanges();
                    this.Init();
                }
            }
        }
    }
}
