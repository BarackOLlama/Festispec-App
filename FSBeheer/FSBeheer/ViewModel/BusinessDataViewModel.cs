using FSBeheer.VM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSBeheer.ViewModel
{
    public class BusinessDataViewModel
    {
        //ViewModel for the BusinessDataView
        //this view must display generic business data for the previous quarter
        //Define boundaries for each quarter?
        //Display the following:
        //Amounts of specific roles(?)
        //Number of current employees
        //Employees that joined in the previous quarter
        //Amount of inspector days worked(?)
        //current number of customers
        //customers joined in the last quarter

        //number of roles?

        public int QuarterNumber { get; set; }
        public int Year { get; set; }
        //counts

        //Inspectors
        public int NewInspectorsCount { get; set; }
        public int ActiveInspectorsCount { get; set; }
        public int InactiveInspectorsCount { get; set; }
        //Inspections
        public int NewInspectionsCount { get; set; }
        //Customers
        public int NewCustomersCount { get; set; }

        //collections
        public ObservableCollection<InspectorVM> Inspectors { get; set; }
        public ObservableCollection<QuestionnaireVM> Questionnares { get; set; }
        public ObservableCollection<QuestionVM> Questions { get; set; }
        public ObservableCollection<AnswerVM> Answers { get; set; }
        public ObservableCollection<InspectionVM> Inspections { get; set; }
        //public ObservableCollection<QuotationVM> Quotations { get; set; }
        public ObservableCollection<CustomerVM> Customers { get; set; }

        public BusinessDataViewModel()
        {
            int quarterNumber = GetQuarter();
            if (quarterNumber - 1 == 0)
            {
                Year = DateTime.Now.Year - 1;
                QuarterNumber = 4;
            }
            else
            {
                Year = DateTime.Now.Year;
                QuarterNumber = GetQuarter();
            }

            using (var context = new CustomFSContext())
            {
                var inspectors = context
                    .Inspectors
                    .Include("InspectionInspectors")
                    .ToList()
                    .Where(e => !e.IsDeleted)
                    .Where(e=> e?.InvalidDate <= GetUpperDateBound())
                    .Select(e => new InspectorVM(e));
                Inspectors = new ObservableCollection<InspectorVM>(inspectors);

                NewInspectorsCount = inspectors.Where(e => e.CertificationDate?
                .Date >= GetLowerDateBound() && e.CertificationDate?.Date <= GetUpperDateBound()).Count();

                ActiveInspectorsCount = inspectors.Where(e => e.Inspection != null).Count();
                InactiveInspectorsCount = inspectors.Where(e => e.Inspection == null).Count();

                var questionnaires = context
                    .Questionnaires
                    .ToList()
                    .Where(e => !e.IsDeleted)
                    .Select(e => new QuestionnaireVM(e));
                Questionnares = new ObservableCollection<QuestionnaireVM>(questionnaires);

                var questions = context
                    .Questions
                    .ToList()
                    .Where(e => !e.IsDeleted)
                    .Select(e => new QuestionVM(e));
                Questions = new ObservableCollection<QuestionVM>(questions);

                var answers = context
                    .Answers
                    .ToList()
                    .Where(e => !e.IsDeleted)
                    .Select(e => new AnswerVM(e));
                Answers = new ObservableCollection<AnswerVM>(answers);

                var inspections = context
                    .Inspections
                    .ToList()
                    .Where(e => !e.IsDeleted)
                    .Select(e => new InspectionVM(e));
                Inspections = new ObservableCollection<InspectionVM>(inspections);

                NewInspectionsCount = inspections.Where(e => e.InspectionDate?.StartDate.Date >= GetLowerDateBound()).Count();

                var customers = context
                    .Customers
                    .ToList()
                    .Where(e => !e.IsDeleted)
                    .Select(e => new CustomerVM(e));
                Customers = new ObservableCollection<CustomerVM>(customers);

                NewCustomersCount = customers.Where(e => e?.StartingDate >= GetLowerDateBound() && e?.StartingDate <= GetUpperDateBound()).Count();
            }
        }


        private int GetQuarter()
        {
            return (DateTime.Now.Month - 1) / 3 + 1;
        }

        public DateTime GetLowerDateBound()
        {
            switch (QuarterNumber)
            {
                case 1:
                    return new DateTime(Year, 1, 1);
                case 2:
                    return new DateTime(Year, 4, 1);
                case 3:
                    return new DateTime(Year, 7, 1);
                case 4:
                    return new DateTime(Year, 10, 1);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public DateTime GetUpperDateBound()
        {
            switch (QuarterNumber)
            {
                case 1:
                    return new DateTime(Year, 3, 31);
                case 2:
                    return new DateTime(Year, 6, 30);
                case 3:
                    return new DateTime(Year, 9, 30);
                case 4:
                    return new DateTime(Year, 12, 31);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
