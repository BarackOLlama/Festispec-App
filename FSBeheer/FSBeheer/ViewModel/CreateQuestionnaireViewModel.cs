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

namespace FSBeheer.ViewModel
{
    public class CreateQuestionnaireViewModel : ViewModelBase
    {
        private QuestionnaireVM _questionnaire;
        public QuestionnaireVM Questionnaire
        {
            get
            {
                return _questionnaire;
            }
            set
            {
                _questionnaire = value;
                base.RaisePropertyChanged("Questionnaire");
            }
        }

        private CustomFSContext _Context;
        public ObservableCollection<QuestionnaireVM> Questionnaires { get; }

        public RelayCommand AddQuestionnaireCommand { get; set; }

        public CreateQuestionnaireViewModel(int QuestionID)
        {
            _Context = new CustomFSContext();
            Questionnaires = _Context.QuestionnaireCrud.GetAllQuestionnaireVMs();

            AddQuestionnaireCommand = new RelayCommand(AddQuestionnaire);
            _questionnaire = new QuestionnaireVM();
        }

        public CreateQuestionnaireViewModel()
        {
            _Context = new CustomFSContext();
            Questionnaires = _Context.QuestionnaireCrud.GetAllQuestionnaireVMs();

            AddQuestionnaireCommand = new RelayCommand(AddQuestionnaire);
            _questionnaire = new QuestionnaireVM();
        }

        private void AddQuestionnaire()
        {
            _Context.QuestionnaireCrud.GetAllQuestionnaireVMs().Add(Questionnaire);
            _Context.QuestionnaireCrud.Add(Questionnaire);
            _Context.SaveChanges();
            Messenger.Default.Send(true, "UpdateQuestionnaires");
        }
    }
}
