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
using System.Text.RegularExpressions;
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
                _question.Type = _selectedQuestionType.ToModel();
                HandleEnabledComponents();
            }
        }
        private ObservableCollection<QuestionTypeVM> _questionTypes;
        public ObservableCollection<QuestionTypeVM> QuestionTypes
        {
            get
            {
                return _questionTypes;
            }
            set
            {
                _questionTypes = value;
                base.RaisePropertyChanged(nameof(QuestionTypes));
            }
        }
        //enable/disable components
        private bool _columnsIsEnabled;
        public bool ColumnsIsEnabled
        {
            get
            {
                return _columnsIsEnabled;
            }
            set
            {
                _columnsIsEnabled = value;
                base.RaisePropertyChanged(nameof(ColumnsIsEnabled));
            }
        }


        private bool _optionsIsEnabled;
        public bool OptionsIsEnabled
        {
            get
            {
                return _optionsIsEnabled;
            }
            set
            {
                _optionsIsEnabled = value;
                base.RaisePropertyChanged(nameof(OptionsIsEnabled));
            }
        }

        //commands
        public RelayCommand<Window> SaveQuestionChangesCommand { get; set; }
        public RelayCommand<Window> CreateQuestionCommand { get; set; }
        public RelayCommand<Window> CloseWindowCommand { get; set; }

        [DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(out int description, int reservedValue);
        public static bool IsInternetConnected()
        {
            return InternetGetConnectedState(out int description, 0);
        }

        //constructors
        public CreateEditQuestionViewModel(int questionnaireId)
        {
            //create
            _context = new CustomFSContext();
            InitializeQuestionTypesAndBooleans();
            InitializeCommands();

            _question = new QuestionVM
            {
                Type = _selectedQuestionType.ToModel(),
                QuestionnaireId = questionnaireId
            };
            base.RaisePropertyChanged(nameof(Question));

        }

        public CreateEditQuestionViewModel(int questionnaireId, int questionId)
        {
            //edit
            _context = new CustomFSContext();
            InitializeQuestionTypesAndBooleans();
            InitializeCommands();

            _question = _context.QuestionCrud.GetQuestionById(questionId);
            base.RaisePropertyChanged(nameof(Question));
        }

        //methods
        private void InitializeCommands()
        {
            SaveQuestionChangesCommand = new RelayCommand<Window>(SaveQuestionChanges);
            CreateQuestionCommand = new RelayCommand<Window>(CreateQuestion);
            CloseWindowCommand = new RelayCommand<Window>(CloseWindow);
        }

        private void InitializeQuestionTypesAndBooleans()
        {
            var types = _context
                .QuestionTypes
                .ToList()
                .Select(e => new QuestionTypeVM(e));
            _questionTypes = new ObservableCollection<QuestionTypeVM>(types);
            base.RaisePropertyChanged(nameof(QuestionTypes));
            if (_question == null)
            {
                _selectedQuestionType = QuestionTypes.FirstOrDefault();
            }
            else
            {
                _selectedQuestionType = new QuestionTypeVM(_question.Type);
            }
            RaisePropertyChanged(nameof(SelectedQuestionType));
            _optionsIsEnabled = true;
            _columnsIsEnabled = false;
        }

        public bool QuestionIsValid()
        {
            #region basic question validation
            if (Question.Type == null)
            {
                MessageBox.Show("Een vraag moet een vraagtype hebben.");
                return false;
            }

            if (Question.Content == null)
            {
                MessageBox.Show("Een vraag mag niet leeg zijn..");
                return false;
            }

            if (Question.Content.Trim() == string.Empty)
            {
                MessageBox.Show("Een vraag mag niet leeg zijn..");
                return false;
            }

            if (SelectedQuestionType == null)
            {
                MessageBox.Show("Een vraag moet een vraagtype hebben.");
                return false;
            }
            #endregion basic question validation

            #region multiple choice validation
            Regex multiplechoiceRegex = new Regex("^(\\w{1}\\|{1}\\w{1,};?){2,}");
            //https://regexr.com/
            //example data
            //A|100;B|200;C|500;D|1000
            //A | 100; c | 200
            //A | 200

            if (SelectedQuestionType.Name == "Multiple Choice vraag" || SelectedQuestionType.Name == "Multiple Choice Tabelvraag")
            {
                if (Question.Options == null)
                {
                    MessageBox.Show("Het veld opties mag bij deze vraagtype niet leeg zijn.");
                    return false;
                }

                if (!multiplechoiceRegex.IsMatch(Question.Options))
                {
                    MessageBox.Show("De multiple choice syntax is incorrect.\n" +
                        "Voorbeeld: A|Waar;B|Niet waar");
                    return false;
                }
            }
            #endregion multiple choice validation

            #region tabel validation
            if (SelectedQuestionType.Name == "Open Tabelvraag" || SelectedQuestionType.Name == "Multiple Choice Tabelvraag")
            {
                if (Question.Columns == null)
                {
                    MessageBox.Show("Bij de geselecteerde vraagtype mag het veld kolommen niet leeg zijn.");
                    return false;
                }

                var columnResults = Question.Columns.Split('|');

                if (columnResults.Length < 3)
                {
                    MessageBox.Show("Incorrecte syntax. Voer een getal (1-9) voor het aantal antwoorden, vervolgt met een '|' en een kolomnaam.\nEen kolomvraag moet minstens 2 kolomnamen hebben.");
                    return false;
                }

                //the string must begin with a number between 1 and 9
                if (!new Regex("^[1-9]{1}$").IsMatch(columnResults[0]))
                {
                    MessageBox.Show("Het eerste karakter van de kolom moet een getal (1-9) zijn, vervolgt door een '|'+" +
                        "en een kolomnaam.\nVolg alle behalve de laatste kolomnaam met een '|'. ");
                    return false;
                }

            }
            #endregion tabel validation

            #region schaal vraag validation

            if (SelectedQuestionType.Name == "Schaal Vraag")
            {
                if (Question.Options == null)
                {
                    MessageBox.Show("Voor het type schaalvraag met het veld Schaal niet leeg zijn.");
                    return false;
                }

                if (Question.Options.Trim() == string.Empty)
                {
                    MessageBox.Show("Voor het type schaalvraag met het veld Schaal niet leeg zijn.");
                    return false;
                }

                if (!Regex.IsMatch(Question.Options, @"^\w+\|\d+;\w+\|\d+$"))
                {
                    MessageBox.Show("Een schaalvraag moet aan de volgende syntax voldoen: negatief|1;positief|10");
                    return false;
                }
                else
                {
                    var scales = Question.Options.Split(';');
                    var scale1 = scales[0].Split('|');
                    var scale2 = scales[2].Split('|');

                    if (Int32.Parse(scale1[1]) >= Int32.Parse(scale2[1]))
                    {
                        MessageBox.Show("De syntax voor een schaalvraag is negatief|1;positief|10\n" +
                            "De waarde na de eerste '|' moet lager zijn dan die achter de tweede.");
                    }
                }
            }
            #endregion schaal vraag validation

            return true;
        }

        private void HandleEnabledComponents()
        {
            switch (SelectedQuestionType.Name)
            {
                case "Multiple Choice vraag":
                    //Multiple choice has options but no columns
                    OptionsIsEnabled = true;
                    ColumnsIsEnabled = false;
                    break;
                case "Open Vraag":
                    //Open question has neither options nor columns
                    OptionsIsEnabled = false;
                    ColumnsIsEnabled = false;
                    break;
                case "Open Tabelvraag":
                    //An open columnquestion has columns but no options
                    OptionsIsEnabled = false;
                    ColumnsIsEnabled = true;
                    break;
                case "Multiple Choice Tabelvraag":
                    //A multiple choice columnquestion has both options and columns
                    OptionsIsEnabled = true;
                    ColumnsIsEnabled = true;
                    break;
                case "Schaal Vraag":
                    OptionsIsEnabled = true;
                    ColumnsIsEnabled = false;
                    break;
            }
        }

        public void SaveQuestionChanges(Window window)
        {
            if (!QuestionIsValid()) return;

            if (IsInternetConnected())
            {
                MessageBoxResult result = MessageBox.Show("Wijzigingen opslaan?", "Bevestiging", MessageBoxButton.OKCancel);
                if (result == MessageBoxResult.OK)
                {
                    RemoveRedundantProperties();
                    Question.Type = SelectedQuestionType.ToModel();
                    _context.SaveChanges();
                    Messenger.Default.Send(true, "UpdateQuestions");
                    window.Close();
                }
            }
        }

        public void CreateQuestion(Window window)
        {
            if (!QuestionIsValid()) return;

            if (IsInternetConnected())
            {
                var result = MessageBox.Show("Opslaan?", "Bevestiging", MessageBoxButton.OKCancel);

                if (result == MessageBoxResult.OK)
                {
                    RemoveRedundantProperties();
                    _context.Questions.Add(Question.ToModel());
                    _context.SaveChanges();
                    Messenger.Default.Send(true, "UpdateQuestions");
                    window.Close();
                }
            }
            else
            {
                MessageBox.Show("U bent niet verbonden met het internet. Probeer het later opnieuw.");
            }
        }

        private void RemoveRedundantProperties()
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
            else if (SelectedQuestionType.Name == "Schaal Vraag")
            {//only scale.
                Question.Columns = null;
            }
        }

        public void CloseWindow(Window window)
        {
            MessageBoxResult result = MessageBox.Show("Scherm sluiten?", "Sluiten", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                window.Close();
            }
            //Must close the window it's opened upon confirmation
            //no need to discard changes as the context is separate from the previous window's.
        }
    }
}
