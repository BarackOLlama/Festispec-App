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
        public QuestionVM Question { get; set; }

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
                HandleEnabledComponents();
            }
        }
        public ObservableCollection<QuestionTypeVM> QuestionTypes { get; set; }

        //enable/disable components
        public bool ColumnsIsEnabled { get; set; }
        public bool OptionsIsEnabled { get; set; }

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
            InitializeQuestionTypes();
            InitializeCommands();

            Question = new QuestionVM
            {
                Type = SelectedQuestionType.ToModel(),
                QuestionnaireId = questionnaireId
            };


        }

        public CreateEditQuestionViewModel(int questionnaireId, int questionId)
        {
            //edit
            _context = new CustomFSContext();
            InitializeQuestionTypes();
            InitializeCommands();

            Question = _context.QuestionCrud.GetQuestionById(questionId);
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
            RaisePropertyChanged(nameof(SelectedQuestionType));
        }

        public bool QuestionIsValid()
        {
            if (Question == null)
            {
                MessageBox.Show("Iets is fout gegaan. Open dit scherm a.u.b. opnieuw.");
                return false;
            }
            if (Question.Content.Trim() == string.Empty)
            {
                MessageBox.Show("Een vraag mag niet leeg zijn..");
                return false;
            }

            Regex multiplechoiceRegex = new Regex("^(\\w{1}\\|{1}\\w{1,};?){2,}");
            //https://regexr.com/
            //example data
            //A|100;B|200;C|500;D|1000
            //A | 100; c | 200
            //A | 200

            if (SelectedQuestionType.Name != "Open Vraag" && SelectedQuestionType.Name != "Open Tabelvraag")
            {
                if (!multiplechoiceRegex.IsMatch(Question.Options))
                {
                    MessageBox.Show("De multiple choice syntax is incorrect.\n" +
                        "Voorbeeld: A|Waar;B|Niet waar");
                    return false;
                }

            }

            return true;
        }

        private void HandleEnabledComponents()
        {
            if (SelectedQuestionType.Name == "Multiple Choice Vraag")
            {
                ColumnsIsEnabled = false;
                OptionsIsEnabled = true;
            }else if (SelectedQuestionType.Name == "Open Vraag")
            {
                ColumnsIsEnabled = false;
                OptionsIsEnabled = false;
            }else if (SelectedQuestionType.Name == "Open Tabelvraag")
            {
                ColumnsIsEnabled = true;
                OptionsIsEnabled = false;
            }else
            {
                ColumnsIsEnabled = true;
                OptionsIsEnabled = true;
            }
        }

        public void SaveQuestionChanges(Window window)
        {
            if (!QuestionIsValid()) return;

            if (SelectedQuestionType.Name == "Open Vraag" || SelectedQuestionType.Name == "Open Tabelvraag")
            {
                Regex multiplechoiceRegex = new Regex("^(\\w{1}\\|{1}\\w{1,};?){2,}");
                if (!multiplechoiceRegex.IsMatch(Question.Content))
                {
                    MessageBox.Show("De multiple choice syntax is incorrect.\n" +
                        "Voorbeeld: A|Waar;B|Niet waar");
                    return;
                }
            }

            if (IsInternetConnected())
            {
                MessageBoxResult result = MessageBox.Show("Wijzigingen opslaan?", "Bevestiging", MessageBoxButton.OKCancel);
                if (result == MessageBoxResult.OK)
                {
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
            else
            {
                MessageBox.Show("U bent niet verbonden met het internet. Probeer het later opnieuw.");
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
