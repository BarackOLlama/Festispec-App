using FSBeheer.Model;
using FSBeheer.View;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FSBeheer.ViewModel
{
    public class QuestionnaireListViewModel : ViewModelBase
    {
        private QuestionnaireVM _selectedQuestionnaire;
        private CustomFSContext _Context;
        public ObservableCollection<QuestionnaireVM> Questionnaires { get; set; }
        public RelayCommand ShowEditQuestionnaireViewCommand { get; set; }

        public RelayCommand ViewQuestionnaireCommand { get; set; }
        public RelayCommand CreateQuestionnaireCommand { get; set; }
        public QuestionnaireListViewModel()
        {
            _Context = new CustomFSContext();
            Questionnaires = _Context.QuestionnaireCrud.GetAllQuestionnaireVMs();
            ShowEditQuestionnaireViewCommand = new RelayCommand(ShowEditQuestionnaireView);
            CreateQuestionnaireCommand = new RelayCommand(CreateQuestionnaire);
            SelectedQuestionnaire = Questionnaires?.First();
            ViewQuestionnaireCommand = new RelayCommand(OpenCreateQuestionWindow);
        }
        public QuestionnaireVM SelectedQuestionnaire
        {
            get
            {
                return _selectedQuestionnaire;
            }
            set
            {
                _selectedQuestionnaire = value;
                base.RaisePropertyChanged("SelectedQuestionnaire");
            }
        }
        public void OpenCreateQuestionWindow()
        {
            new QuestionnaireEditView().ShowDialog();
        }
        private void ShowEditQuestionnaireView()
        {
            new EditQuestionnaireView().Show();
        }

        private void CreateQuestionnaire()
        {
            new CreateQuestionnaireView().Show();
        }
    }
}