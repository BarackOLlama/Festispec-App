using FSBeheer.View;
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
    public class QuestionnaireManagementViewModel
    {
        private CustomFSContext _Context;
        public ObservableCollection<QuestionnaireVM> Questionnaires { get; }
        public RelayCommand ShowEditQuestionnaireViewCommand { get; set; }

        public QuestionnaireManagementViewModel()
        {
            _Context = new CustomFSContext();
            Questionnaires = _Context.QuestionnaireCrud.GetQuestionnaires();
            ShowEditQuestionnaireViewCommand = new RelayCommand(ShowEditQuestionView);
        }

        private void ShowEditQuestionView()
        {
            new EditQuestionnaireView().Show();
        }
    }
}
