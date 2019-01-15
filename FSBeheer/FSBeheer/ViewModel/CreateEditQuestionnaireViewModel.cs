using FSBeheer.Model;
using FSBeheer.View;
using FSBeheer.VM;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;

namespace FSBeheer.ViewModel
{
    public class CreateEditQuestionnaireViewModel : ViewModelBase
    {
        private CustomFSContext _context;
        private QuestionnaireVM _questionnaire;
        public QuestionnaireVM Questionnaire
        {
            get { return _questionnaire; }
            set
            {
                _questionnaire = value;
                base.RaisePropertyChanged(nameof(Questionnaire));
            }
        }

        private QuestionVM _selectedQuestion;
        public QuestionVM SelectedQuestion
        {
            get { return _selectedQuestion; }
            set
            {
                _selectedQuestion = value;
                base.RaisePropertyChanged(nameof(SelectedQuestion));
            }
        }

        public string SelectedQuestionnaireTemplate { get; set; }
        public ObservableCollection<string> QuestionnaireTemplateNames { get; set; }
        public ObservableCollection<InspectionVM> Inspections { get; set; }
        public ObservableCollection<QuestionVM> Questions { get; set; }
        public int SelectedIndex { get; set; }

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
            _context = new CustomFSContext();
            Messenger.Default.Register<bool>(this, "UpdateQuestions", cl => FetchAndSetQuestions(_questionnaire.Id));
            Questionnaire = _context.QuestionnaireCrud.GetQuestionnaireById(questionnaireId);
            FetchAndSetQuestions(_questionnaire.Id);
            SelectedQuestion = Questions.FirstOrDefault();
            InitializeCommands();
            //FetchAndSetInspectionNumbersAndSelectedInspection();
        }

        public CreateEditQuestionnaireViewModel()
        {
            //create
            _context = new CustomFSContext();
            Questionnaire = new QuestionnaireVM
            {
                Id = _context.Questionnaires.ToList().Max(e => e.Id) + 1
            };
            InitializeCommands();
            FetchAndSetInspectionNumbersAndSelectedInspection();
            QuestionnaireTemplateNames = new ObservableCollection<string>()
            {
                "geen",
                "Metal Festival",
                "Winter Festival",
                "Hard Bass Festival"
            };
            SelectedQuestionnaireTemplate = QuestionnaireTemplateNames.First();
        }

        //methods

        private void InitializeCommands()
        {
            OpenCreateQuestionViewCommand = new RelayCommand(OpenCreateQuestionView);
            SaveQuestionnaireChangesCommand = new RelayCommand<Window>(SaveQuestionnaireChanges);
            OpenEditQuestionViewCommand = new RelayCommand(OpenEditQuestionView);
            DeleteQuestionCommand = new RelayCommand(DeleteQuestion);
            CreateQuestionnaireCommand = new RelayCommand<Window>(CreateQuestionnaire);
            CloseWindowCommand = new RelayCommand<Window>(CloseWindow);
        }

        private void FetchAndSetInspectionNumbersAndSelectedInspection()
        {
            Inspections = _context.InspectionCrud.GetAllInspections();
            if (Questionnaire.Inspection == null)
            {
                Questionnaire.Inspection = new InspectionVM();
            }
            SelectedIndex = GetIndex(Questionnaire.Inspection, Inspections);
            RaisePropertyChanged(nameof(SelectedIndex));
        }

        private int GetIndex(InspectionVM Obj, ObservableCollection<InspectionVM> List)
        {
            for (int i = 0; i < List.Count; i++)
                if (List[i].Id == Obj.Id)
                    return i;
            return -1;
        }

        private void FetchAndSetQuestions(int questionnaireId)
        {
            Questions = _context.QuestionCrud.GetAllQuestionsByQuestionnaire(Questionnaire);
            RaisePropertyChanged(nameof(Questions));
        }

        private void CloseWindow(Window window)
        {
            var result = MessageBox.Show("Terug gaan zonder wijzigingen op te slaan?", "Vragenlijst sluiten", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                window.Close();
            }
        }

        private bool QuestionnaireIsValid()
        {
            if (Questionnaire.Name == null)
            {
                MessageBox.Show("Een vragenlijst moet een naam hebben.");
                return false;
            }

            if (Questionnaire.Name.Trim() == string.Empty)
            {
                MessageBox.Show("Een vragenlijst moet een vraag hebben.");
                return false;
            }

            return true;
        }

        private void SaveQuestionnaireChanges(Window window)
        {
            if (!QuestionnaireIsValid()) return;

            if (IsInternetConnected())
            {
                MessageBoxResult result = MessageBox.Show("Opslaan wijzigingen?", "Bevestiging", MessageBoxButton.OKCancel);
                if (result == MessageBoxResult.OK)
                {
                    _context.Entry(Questionnaire.ToModel()).State = System.Data.Entity.EntityState.Modified;
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
            if (IsInternetConnected())
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

        private void CreateQuestionnaire(Window window)
        {
            if (!QuestionnaireIsValid()) return;

            if (IsInternetConnected())
            {
                var result = MessageBox.Show("Opslaan nieuwe vragenlijst?", "Nieuwe vragenlijst", MessageBoxButton.OKCancel);

                if (result == MessageBoxResult.OK)
                {
                    if (SelectedQuestionnaireTemplate != "geen")
                    {
                        SetQuestionnaireFromTemplate();
                    }
                    _context.Questionnaires.Add(Questionnaire.ToModel());
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

        public void SetQuestionnaireFromTemplate()
        {
            switch (SelectedQuestionnaireTemplate)
            {
                case "Metal Festival":
                    _context.Questions.Add(new Question()
                    {
                        Content = "Hebben alle bands al hun nummers kunnen spelen?",
                        QuestionType = _context.QuestionTypes.FirstOrDefault(e => e.Name == "Open Vraag"),
                        Comments = "",
                        QuestionnaireId = Questionnaire.Id
                    });
                    _context.Questions.Add(new Question()
                    {
                        Content = "Zijn er nog grote complicaties opgetreden?",
                        QuestionType = _context.QuestionTypes.FirstOrDefault(e => e.Name == "Multiple Choice vraag"),
                        Comments = "Hierbij worden problemen bedoelt die voor vertraging van optredens e.d. hebben gezorgd.",
                        Options = "A|Ja;B|Nee",
                        QuestionnaireId = Questionnaire.Id
                    });
                    _context.Questions.Add(new Question()
                    {
                        Content = "Hoe is de sfeer?",
                        QuestionType = _context.QuestionTypes.FirstOrDefault(e => e.Name == "Open Vraag"),
                        Comments = "Deze vraag is het best beantwoord tegen het einde van het festival.",
                        QuestionnaireId = Questionnaire.Id
                    });
                    _context.Questions.Add(new Question()
                    {
                        Content = "Zijn er moshpits ontstaan tijdens het festival?",
                        QuestionType = _context.QuestionTypes.FirstOrDefault(e => e.Name == "Multiple Choice vraag"),
                        Options="A|Het gehele festival is een moshpit;B|Een paar",
                        QuestionnaireId = Questionnaire.Id
                    });
                    _context.Questions.Add(new Question()
                    {
                        Content = "Is er veel gevaarlijk afval op het festival?",
                        QuestionType = _context.QuestionTypes.FirstOrDefault(e => e.Name == "Open Vraag"),
                        Columns = "Met gevaarlijk afval worden glasscherven e.d. bedoeld; dingen waar bezoekers zich snel aan kunnen bezeren.",
                        QuestionnaireId = Questionnaire.Id
                    });
                    break;
                case "Winter Festival":
                    _context.Questions.Add(new Question()
                    {
                        Content = "Is er gluhwein te koop?",
                        QuestionType = _context.QuestionTypes.FirstOrDefault(e => e.Name == "Open Vraag"),
                        QuestionnaireId = Questionnaire.Id
                    });
                    _context.Questions.Add(new Question()
                    {
                        Content = "Dragen veel mensen kleding met een kerst-thema?",
                        QuestionType = _context.QuestionTypes.FirstOrDefault(e => e.Name == "Multiple Choice vraag"),
                        Options = "A|Jazeker;B|Nee;C|Misschien",
                        QuestionnaireId = Questionnaire.Id
                    });
                    _context.Questions.Add(new Question()
                    {
                        Content = "Is de Kerstman er ook?",
                        QuestionType = _context.QuestionTypes.FirstOrDefault(e => e.Name == "Open Vraag"),
                        QuestionnaireId = Questionnaire.Id
                    });
                    _context.Questions.Add(new Question()
                    {
                        Content = "Bij welke band was het publiek het meest enthousiast?",
                        QuestionType = _context.QuestionTypes.FirstOrDefault(e => e.Name == "Open Vraag"),
                        QuestionnaireId = Questionnaire.Id
                    });
                    _context.Questions.Add(new Question()
                    {
                        Content = "Heeft het tijdens het festival gesneeuwd?",
                        QuestionType = _context.QuestionTypes.FirstOrDefault(e => e.Name == "Multiple Choice vraag"),
                        Options = "A|Ja;B|Nee;C|Een heel klein beetje",
                        QuestionnaireId = Questionnaire.Id
                    });
                    break;
                case "Hard Bass Festival":
                    _context.Questions.Add(new Question()
                    {
                        Content = "Hoe hoog is het volume dicht bij het podium?",
                        QuestionType = _context.QuestionTypes.FirstOrDefault(e => e.Name == "Multiple Choice vraag"),
                        Options="A|100DB of lager;B|120DB+;C|Mijn oren zijn stuk",
                        Comments ="Gebruik hiervoor de decibelmeter.",
                        QuestionnaireId = Questionnaire.Id
                    });
                    _context.Questions.Add(new Question()
                    {
                        Content = "Lijken er veel bezoekers onder de invloed te zijn van iets anders dan alcohol?",
                        QuestionType = _context.QuestionTypes.FirstOrDefault(e => e.Name == "Multiple Choice vraag"),
                        Options = "A|Nee;B|Ja, een paar.;C|Het is een festival.",
                        Comments="Onder de invloed van andere middelen zoals drugs en dergelijke middelen",
                        QuestionnaireId = Questionnaire.Id
                    });
                    _context.Questions.Add(new Question()
                    {
                        Content = "Is het druk op het festival?",
                        QuestionType = _context.QuestionTypes.FirstOrDefault(e => e.Name == "Open Vraag"),
                        QuestionnaireId = Questionnaire.Id
                    });
                    _context.Questions.Add(new Question()
                    {
                        Content = "Ligt er veel glas op de grond?",
                        QuestionType = _context.QuestionTypes.FirstOrDefault(e => e.Name == "Multiple Choice vraag"),
                        Options = "A|Ja;B|Nee",
                        QuestionnaireId = Questionnaire.Id
                    });
                    _context.Questions.Add(new Question()
                    {
                        Content = "Is het druk bij de bars?",
                        QuestionType = _context.QuestionTypes.FirstOrDefault(e => e.Name == "Open Vraag"),
                        QuestionnaireId = Questionnaire.Id
                    });
                    break;
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
                        this.FetchAndSetQuestions(Questionnaire.Id);
                        //so that the Questionnaires in QuestionnaireManagementViewModel display the correct number of questions.
                        Messenger.Default.Send(true, "UpdateQuestionnaires");
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
