using FSBeheer.View;
using FSBeheer.VM;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System;
using FSBeheer.Model;
using System.Runtime.InteropServices;
using System.Runtime.Caching;

namespace FSBeheer.ViewModel
{
    public class QuestionnaireManagementViewModel : ViewModelBase
    {
        private QuestionnaireVM _selectedQuestionnaire;
        private CustomFSContext _context;
        public ObservableCollection<QuestionnaireVM> Questionnaires { get; set; }
        public RelayCommand ShowEditQuestionnaireViewCommand { get; set; }
        public RelayCommand CreateQuestionnaireCommand { get; set; }
        public RelayCommand DeleteQuestionnaireCommand { get; set; }
        public RelayCommand<Window> CloseWindowCommand { get; set; }

        [DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(out int description, int reservedValue);
        public static bool IsInternetConnected()
        {
            return InternetGetConnectedState(out int description, 0);
        }

        public QuestionnaireManagementViewModel()
        {
            Messenger.Default.Register<bool>(this, "UpdateQuestionnaires", e => Init());
            Init();

            ShowEditQuestionnaireViewCommand = new RelayCommand(ShowEditQuestionnaireView);
            CreateQuestionnaireCommand = new RelayCommand(CreateQuestionnaire);
            DeleteQuestionnaireCommand = new RelayCommand(DeleteQuestionnaire);
            CloseWindowCommand = new RelayCommand<Window>(CloseWindow);
            SelectedQuestionnaire = Questionnaires?.First();
        }

        private void CloseWindow(Window obj)
        {
            obj.Close();
        }

        internal void Init()
        {
            _context = new CustomFSContext();
            GetData();
        }

        private void GetData()
        {
            ObjectCache cache = MemoryCache.Default;
            CacheItemPolicy policy = new CacheItemPolicy
            {
                AbsoluteExpiration = DateTimeOffset.Now.AddDays(1)
            };
            if (IsInternetConnected())
            {
                Questionnaires = _context.QuestionnaireCrud.GetAllQuestionnaires();
                cache.Set("questionnaires", Questionnaires, policy);
            }
            else
            {
                Questionnaires = cache["questionnaires"] as ObservableCollection<QuestionnaireVM>;
                if (Questionnaires == null)
                {
                    Questionnaires = new ObservableCollection<QuestionnaireVM>();
                }
            }
            RaisePropertyChanged(nameof(Questionnaires));
        }

        public QuestionnaireVM SelectedQuestionnaire
        {
            get { return _selectedQuestionnaire; }
            set
            {
                _selectedQuestionnaire = value;
                base.RaisePropertyChanged(nameof(SelectedQuestionnaire));
            }
        }

        public void ShowEditQuestionnaireView()
        {
            if (IsInternetConnected())
            {
                if (_selectedQuestionnaire == null || _selectedQuestionnaire.IsDeleted)
                {
                    MessageBox.Show("Geen vragenlijst geselecteerd.");
                }
                else
                {
                    new EditQuestionnaireView().ShowDialog();
                }
            }
            else
            {
                MessageBox.Show("U bent niet verbonden met het internet. Probeer het later opnieuw.");
            }
        }

        public void DeleteQuestionnaire()
        {
            if (IsInternetConnected())
            {
                if (_selectedQuestionnaire == null || _selectedQuestionnaire.IsDeleted)
                {
                    MessageBox.Show("Geen vragenlijst geselecteerd");
                }
                else
                {
                    var result = MessageBox.Show("Vraag verwijderen?", "Verwijder", MessageBoxButton.OKCancel);
                    if (result == MessageBoxResult.OK)
                    {
                        _selectedQuestionnaire.IsDeleted = true;
                        _context.SaveChanges();
                        Init();
                    }
                }
            }
            else
            {
                MessageBox.Show("U bent niet verbonden met het internet. Probeer het later opnieuw.");
            }
        }

        public void CreateQuestionnaire()
        {
            if(IsInternetConnected())
                new CreateQuestionnaireView().ShowDialog();
            else
                MessageBox.Show("U bent niet verbonden met het internet. Probeer het later opnieuw.");
        }

        public void FilterList(string filter)
        {
            if (string.IsNullOrEmpty(filter))
            {
                Questionnaires = _context.QuestionnaireCrud.GetAllQuestionnaires();
            }
            else
            {
                Questionnaires = _context.QuestionnaireCrud.GetAllQuestionnairesFiltered(filter);
            }
            RaisePropertyChanged(nameof(Questionnaires));
        }
    }
}