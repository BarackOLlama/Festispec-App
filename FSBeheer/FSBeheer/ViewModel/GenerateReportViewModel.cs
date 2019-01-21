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
        public ObservableCollection<QuestionPDFVM> QuestionPDFs { get; set; }
        public List<string> CheckboxesSelected { get; set; }``
        public RelayCommand CreatePDFCommand { get; set; }

        private PDFGenerator pdfGenerator;

        public string Filename { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Advice { get; set; }

        public GenerateReportViewModel()
        {
            _context = new CustomFSContext();
            CheckboxesSelected = new List<string>();

            CreatePDFCommand = new RelayCommand(CreatePDF);
        }

        private void CreatePDF()
        {
            if (QuestionPDFs.Count == 0)
                MessageBox.Show("Je kunt voor deze inspectie geen rapportage uitdraaien, omdat deze inspectie geen vragen bevat.");
            PassSelectedQuestions();
            PassSelectedCharts();
            if (Filename != null && Title != null && Questions != null)
            {
                pdfGenerator = new PDFGenerator(Filename, 
                    Title, 
                    Description, 
                    Advice, 
                    Questions,
                    CheckboxesSelected,
                    Customer, 
                    SelectedInspection, 
                    StartDate, 
                    EndDate,
                    _context);
                pdfGenerator.CreateStandardPDF();
            } else
            {
                MessageBox.Show("You have not entered all necessary fields (Bestandnaam, Titel)");
            }
            ResetQuestions();
            CheckboxesSelected = new List<string>();
        }

        public void SetInspection(int inspectionId)
        {
            if (inspectionId > 0)
            {
                SelectedInspection = _context.InspectionCrud.GetInspectionById(inspectionId);
                RetrieveQuestionnaire(inspectionId);
                RetrieveQuestionsAndQuestionPDFs();
                RaisePropertyChanged(nameof(Questions));
                RetrieveCustomer(SelectedInspection);
                RetrieveDate(SelectedInspection);
            }
            RaisePropertyChanged(nameof(Questions));
            RaisePropertyChanged(nameof(QuestionPDFs));
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
        }

        private void RetrieveQuestionnaire(int inspectionId)
        {
            if (SelectedInspection != null)
                Questionnaire = _context.QuestionnaireCrud.GetQuestionnaireByInspectionId(inspectionId);
        }

        private void RetrieveQuestionsAndQuestionPDFs()
        {
            if (Questionnaire != null)
            {
                Questions = _context.QuestionCrud.GetAllQuestionsByQuestionnaire(Questionnaire);
                QuestionPDFs = _context.QuestionPDFCrud.ConvertQuestionVMToQuestionPDFVM(Questions);
            } 
        }

        private void PassSelectedQuestions()
        {
            var removeQuestionsList = new List<QuestionVM>();
            foreach (QuestionPDFVM questionPDF in QuestionPDFs)
                if (questionPDF.DoNotShow)
                    for (int i = Questions.Count - 1; i >= 0; i--)
                        if (Questions[i].Content == questionPDF.Content)
                            removeQuestionsList.Add(Questions[i]);
            foreach (QuestionVM question in removeQuestionsList)
            {
                Questions.Remove(question);
            }

        }

        private void PassSelectedCharts()
        {
            int counter;
            var removeQuestionPDFsList = new List<QuestionPDFVM>();
            foreach (QuestionPDFVM questionPDF in QuestionPDFs)
            {
                counter = 0;
                foreach (QuestionVM question in Questions)
                {
                    // als de twee objecten niet gelijk zijn
                    if (questionPDF.Content != question.Content)
                        counter++;
                    if (counter == Questions.Count())
                        removeQuestionPDFsList.Add(questionPDF);
                }
            }
            //foreach (QuestionPDFVM questionPDF in removeQuestionPDFsList)
            //{
            //    QuestionPDFs.Remove(questionPDF);
            //}

            foreach (QuestionPDFVM questionPDF in QuestionPDFs)
            {
                if (!removeQuestionPDFsList.Contains(questionPDF))
                    if (questionPDF.BarChart)
                    {
                        CheckboxesSelected.Add("BarChart");
                    }
                    else if (questionPDF.PieChart)
                    {
                        CheckboxesSelected.Add("PieChart");
                    }
                    else
                    {
                        CheckboxesSelected.Add("NoChart");
                    }
            }
        }

        private void ResetQuestions()
        {
            Questions = _context.QuestionCrud.GetAllQuestionsByQuestionnaire(Questionnaire);
        }
    }
}
