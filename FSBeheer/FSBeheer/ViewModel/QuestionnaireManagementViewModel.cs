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
        public QuestionnaireManagementViewModel()
        {
            Messenger.Default.Register<bool>(this, "UpdateQuestionnaires", e => Init());
            Init();
            Questionnaires = _context.QuestionnaireCrud.GetAllQuestionnaireVMs();
            ShowEditQuestionnaireViewCommand = new RelayCommand(ShowEditQuestionnaireView);
            CreateQuestionnaireCommand = new RelayCommand(CreateQuestionnaire);
            SelectedQuestionnaire = Questionnaires?.First();
        }

        internal void Init()
        {
            _context = new CustomFSContext();
            var questionnaires = _context.Questionnaires.ToList().Select(e => new QuestionnaireVM(e));
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
            if (_selectedQuestionnaire == null)
            {
                MessageBox.Show("Geen vragenlijst geselecteerd.");
            }
            else
            {
                new EditQuestionnaireView().ShowDialog();
            }
        }

        public void CreateQuestionnaire()
        {
            new CreateQuestionnaireView().ShowDialog();
        }
    }
}