using FSBeheer.VM;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FSBeheer.ViewModel
{
    public class GenerateReportViewModel : ViewModelBase
    {
        private CustomFSContext _context;
        public InspectionVM SelectedInspection { get; set; }
        public CustomerVM Customer { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public QuestionnaireVM Questionnaire { get; set; }
        public ObservableCollection<QuestionVM> Questions { get; set; }
        public RelayCommand CreateStandardPDFCommand { get; set; }

        private PDFGenerator pdfGenerator;

        public string Filename { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Advice { get; set; }

        public bool BarChart { get; set; }

        public bool DoNotShow { get; set; }

        public bool PieChart { get; set; }

        public RelayCommand CreateBarChartCommand { get; set; }

        public RelayCommand CreatePieChartCommand { get; set; }

        public GenerateReportViewModel()
        {
            _context = new CustomFSContext();
            Description = "";

            CreateStandardPDFCommand = new RelayCommand(CreatePDF);
        }

        private void CreatePDF()
        {
            if (Filename != null && Title != null && Questions != null)
            {
                pdfGenerator = new PDFGenerator(Filename, 
                    Title, 
                    Description, 
                    Advice, 
                    Questions, 
                    Customer, 
                    SelectedInspection, 
                    StartDate, 
                    EndDate);
                pdfGenerator.CreateStandardPDF();
            } else
            {
                MessageBox.Show("Sonuvabitch you have not filled the required fields");
            }
        }

        public void SetInspection(int inspectionId)
        {
            if (inspectionId > 0)
            {
                SelectedInspection = _context.InspectionCrud.GetInspectionById(inspectionId);
                RetrieveQuestionnaire(inspectionId);
                RetrieveQuestions();
                RaisePropertyChanged(nameof(Questions));
                RetrieveCustomer(SelectedInspection);
                RetrieveDate(SelectedInspection);
            }
        }

        private void RetrieveCustomer(InspectionVM inspectionVM)
        {
            EventVM eventVM = new EventVM();
            CustomerVM customerVM = new CustomerVM();
            if (inspectionVM.Event != null && inspectionVM.Event.Zipcode != null)
                eventVM = inspectionVM.Event;
            if (eventVM.Customer != null)
                Customer = eventVM.Customer;
        }

        private void RetrieveDate(InspectionVM inspectionVM)
        {
            if (inspectionVM.InspectionDate.StartDate != null && inspectionVM.InspectionDate.EndDate != null)
            {
                StartDate = inspectionVM.InspectionDate.StartDate;
                EndDate = inspectionVM.InspectionDate.EndDate;
            }
            else
            {
                // work in progress
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
