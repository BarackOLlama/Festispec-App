using FSBeheer.View;
using FSBeheer.VM;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

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
        public QuestionnaireManagementViewModel()
        {
            Messenger.Default.Register<bool>(this, "UpdateQuestionnaires", e => Init());
            Init();
            Questionnaires = _context.QuestionnaireCrud.GetAllQuestionnaireVMs();
            ShowEditQuestionnaireViewCommand = new RelayCommand(ShowEditQuestionnaireView);
            CreateQuestionnaireCommand = new RelayCommand(CreateQuestionnaire);
            DeleteQuestionnaireCommand = new RelayCommand(DeleteQuestionnaire);
            SelectedQuestionnaire = Questionnaires?.First();
        }

        internal void Init()
        {
            _context = new CustomFSContext();
            var questionnaires = _context.Questionnaires.
                ToList().
                Where(e=>!e.IsDeleted).
                Select(e => new QuestionnaireVM(e));
            Questionnaires = new ObservableCollection<QuestionnaireVM>(questionnaires);
            RaisePropertyChanged("Questionnaires");
        }

        public QuestionnaireVM SelectedQuestionnaire
        {
            get { return _selectedQuestionnaire; }
            set
            {
                _selectedQuestionnaire = value;
                base.RaisePropertyChanged("SelectedQuestionnaire");
            }
        }

        public void ShowEditQuestionnaireView()
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

        public void DeleteQuestionnaire()
        {
            if (_selectedQuestionnaire == null || _selectedQuestionnaire.IsDeleted)
            {
                MessageBox.Show("Geen vragenlijst geselecteerd");
            }else
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

        public void CreateQuestionnaire()
        {
            new CreateQuestionnaireView().ShowDialog();
        }
    }
}