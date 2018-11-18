using FSBeheer.VM;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSBeheer.ViewModel
{
    public class CreateQuestionnaireViewModel
    {
        public QuestionnaireVM Questionnaire { get; set; }

        private CustomFSContext _Context;
        public ObservableCollection<QuestionnaireVM> Questionnaires { get; }

        public RelayCommand AddQuestionnaireCommand { get; set; }

        public CreateQuestionnaireViewModel()
        {
            _Context = new CustomFSContext();
            Questionnaires = _Context.QuestionnaireCrud.GetAllQuestionnaireVMs();

            AddQuestionnaireCommand = new RelayCommand(AddQuestionnaire);

        }

        private void AddQuestionnaire()
        {
            _Context.QuestionnaireCrud.GetAllQuestionnaireVMs().Add(Questionnaire);
            _Context.QuestionnaireCrud.Add(Questionnaire);
        }
    }
}
