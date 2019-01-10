using FSBeheer.VM;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSBeheer.ViewModel
{
    public class GenerateReportViewModel : ViewModelBase
    {
        private CustomFSContext _context;
        public InspectionVM SelectedInspection { get; set; }
        public QuestionnaireVM Questionnaire { get; set; }
        public ObservableCollection<QuestionVM> Questions { get; set; }
        public string Title { get;
            set; }

        public GenerateReportViewModel()
        {
            _context = new CustomFSContext();

            Title = "";
        }

        public void SetInspection(int inspectionId)
        {
            if (inspectionId > 0)
            {
                SelectedInspection = _context.InspectionCrud.GetInspectionById(inspectionId);
                RetrieveQuestionnaire(inspectionId);
                RetrieveQuestions();
                RaisePropertyChanged(nameof(Questions));
            }
        }

        private void RetrieveQuestionnaire(int inspectionId)
        {
            if (SelectedInspection != null)
                Questionnaire = _context.QuestionnaireCrud.GetQuestionnaireByInspectionId(inspectionId);
        }

        private void RetrieveQuestions()
        {
            if (Questionnaire != null)
                Questions = _context.QuestionCrud.GetAllQuestionsByQuestionnaire(Questionnaire);
        }
    }
}
