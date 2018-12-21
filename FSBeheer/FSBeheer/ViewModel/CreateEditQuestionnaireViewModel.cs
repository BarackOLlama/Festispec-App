using FSBeheer.View;
using FSBeheer.VM;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FSBeheer.ViewModel
{
    public class CreateEditQuestionnaireViewModel :ViewModelBase
    {
        private CustomFSContext _context;
        public QuestionnaireVM Questionnaire { get; set; }

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
        public RelayCommand<Window> CreateQuestionnaireCommand { get; set; }

        public RelayCommand DeleteQuestionCommand { get; set; }
        public RelayCommand<Window> CloseWindowCommand { get; set; }

        [DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(out int description, int reservedValue);
        public static bool IsInternetConnected()
        {
            return InternetGetConnectedState(out int description, 0);
        }

        public CreateEditQuestionnaireViewModel(int questionnaireId)
        {
            //edit
            Messenger.Default.Register<bool>(this, "UpdateQuestions", cl => Init());
            Questionnaire = _context.QuestionnaireCrud.GetQuestionnaireById(questionnaireId);

            Init();

            var inspectionNumbers = _context.Inspections
                .ToList()
                .Where(e => !e.IsDeleted)
                .Select(e => (int?)e.Id);
            InspectionNumbers = new ObservableCollection<int?>(inspectionNumbers);
            _selectedInspectionNumber = inspectionNumbers.FirstOrDefault();

            SelectedQuestion = Questions.FirstOrDefault();

            InitializeCommands();
        }

        public CreateEditQuestionnaireViewModel()
        {
            //create
            Messenger.Default.Register<bool>(this, "UpdateQuestions", cl => Init());
            Questionnaire = new QuestionnaireVM();
            Init();
            InitializeCommands();
        }

        //methods

        private void InitializeCommands()
        {
            OpenCreateQuestionViewCommand = new RelayCommand(OpenCreateQuestionView);
            SaveQuestionnaireChangesCommand = new RelayCommand<Window>(SaveQuestionnaireChanges);
            OpenEditQuestionViewCommand = new RelayCommand(OpenEditQuestionView);
            DeleteQuestionCommand = new RelayCommand(DeleteQuestion);
            CreateQuestionnaireCommand = new RelayCommand<Window>(SaveQuestionnaire);
            CloseWindowCommand = new RelayCommand<Window>(CloseWindow);
        }

        internal void Init()
        {
            _context = new CustomFSContext();
            Questions = _context.QuestionCrud.GetAllQuestionsByQuestionnaire(Questionnaire);
            RaisePropertyChanged(nameof(Questions));
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
            if (IsInternetConnected())
            {
                MessageBoxResult result = MessageBox.Show("Opslaan wijzigingen?", "Bevestiging", MessageBoxButton.OKCancel);
                if (result == MessageBoxResult.OK)
                {
                    _context.SaveChanges();
                    Messenger.Default.Send(true, "UpdateQuestionnaires");
                    window.Close();
                }
            }
            else
            {
                MessageBox.Show("U bent niet verbonden met het internet. Probeer het later opnieuw.");
            }
        }

        public void OpenCreateQuestionView()
        {
            if(IsInternetConnected())
                new CreateQuestionView().ShowDialog();
            else
                MessageBox.Show("U bent niet verbonden met het internet. Probeer het later opnieuw.");
        }

        private void OpenEditQuestionView()
        {
            if (IsInternetConnected())
            {
                if (_selectedQuestion == null)
                    MessageBox.Show("Geen vraag geselecteerd.");
                else
                    new EditQuestionView().ShowDialog();
            }
            else
            {
                MessageBox.Show("U bent niet verbonden met het internet. Probeer het later opnieuw.");
            }
        }

        public void UpdateQuestions()
        {
            Questions = _context.QuestionCrud.GetAllQuestions();
            RaisePropertyChanged(nameof(Questions));
        }

        private void SaveQuestionnaire(Window window)
        {
            if (IsInternetConnected())
            {
                _context.Questionnaires.Add(Questionnaire.ToModel());
                _context.SaveChanges();
                Messenger.Default.Send(true, "UpdateQuestionnaires");
                window.Close();
            }
            else
            {
                MessageBox.Show("U bent niet verbonden met het internet. Probeer het later opnieuw.");
            }
        }

        public void DeleteQuestion()
        {
            if (IsInternetConnected())
            {
                if (_selectedQuestion == null || _selectedQuestion.IsDeleted)
                    MessageBox.Show("Geen vraag geselecteerd.");
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
            else
            {
                MessageBox.Show("U bent niet verbonden met het internet. Probeer het later opnieuw.");
            }
        }
    }
}
