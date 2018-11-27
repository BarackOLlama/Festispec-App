using FSBeheer.View;
using FSBeheer.VM;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;
using System.Linq;

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
            _context = new CustomFSContext();
            Questionnaires = _context.QuestionnaireCrud.GetAllQuestionnaireVMs();

            ShowEditQuestionnaireViewCommand = new RelayCommand(ShowEditQuestionnaireView);
            CreateQuestionnaireCommand = new RelayCommand(CreateQuestionnaire);
            SelectedQuestionnaire = Questionnaires?.First();
            //_context.Dispose();
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
            new EditQuestionnaireView().ShowDialog();
        }

        public void CreateQuestionnaire()
        {
            new CreateQuestionnaireView().Show();
        }
    }
}