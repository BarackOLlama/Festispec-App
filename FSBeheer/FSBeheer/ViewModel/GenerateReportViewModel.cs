using FSBeheer.VM;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSBeheer.ViewModel
{
    public class GenerateReportViewModel : ViewModelBase
    {
        private CustomFSContext _context;
        public InspectionVM SelectedInspection;
        public QuestionnaireVM Questionnaire;
        public string Title;

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
            }
        }

        private void RetrieveQuestionnaire(int inspectionId)
        {
            if (SelectedInspection != null)
                Questionnaire = _context.QuestionnaireCrud.GetQuestionnaireByInspectionId(inspectionId);
        }
    }
}
