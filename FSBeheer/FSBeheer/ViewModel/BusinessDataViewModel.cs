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
    public class BusinessDataViewModel : ViewModelBase
    {
        private int _quarterNumber;
        public int QuarterNumber { get { return _quarterNumber; } set { _quarterNumber = value; Init(); } }
        private int _year;
        public int Year { get { return _year; } set { _year = value; Init(); } }

        public ObservableCollection<int> Quarters { get; set; }
        public ObservableCollection<int> ValidYears { get; set; }

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
        public ObservableCollection<InspectionVM> Inspections { get; set; }
        //public ObservableCollection<QuotationVM> Quotations { get; set; }
        public ObservableCollection<CustomerVM> Customers { get; set; }

        public BusinessDataViewModel()
        {
            Quarters = new ObservableCollection<int>() { 1, 2, 3, 4 };
            ValidYears = new ObservableCollection<int>();

            for(int i = 2014; i<DateTime.Now.Year + 1; i++)
            {
                ValidYears.Add(i);
            }

            //set the quarter to the previous one.
            int quarter = (DateTime.Now.Month - 1) / 3 + 1;
            //set the year

            if (quarter == 1)
            {
                _quarterNumber = 4;
                _year = DateTime.Now.Year - 1;
            }
            else
            {
                _quarterNumber = quarter;
                _year = DateTime.Now.Year;
            }
            Init();
        }

        public void Init()
        {
            using (var context = new CustomFSContext())
            {

                var inspectors = context
                    .Inspectors
                    .ToList()
                    .Where(e => !e.IsDeleted && e.InvalidDate > GetUpperDateBound())
                    .Select(e => new InspectorVM(e));
                Inspectors = new ObservableCollection<InspectorVM>(inspectors);
                base.RaisePropertyChanged(nameof(Inspectors));

                NewInspectorsCount = inspectors
                    .Where(e => e.CertificationDate >= GetLowerDateBound() &&
                    e.CertificationDate <= GetUpperDateBound())
                    .Count();
                base.RaisePropertyChanged(nameof(NewInspectorsCount));

                InactiveInspectorsCount = context.InspectorCrud.GetAllInspectorsFilteredByAvailability(new List<DateTime>() {
                    GetLowerDateBound(),
                    GetUpperDateBound()
                }).Count();
                base.RaisePropertyChanged(nameof(InactiveInspectorsCount));

                ActiveInspectorsCount = inspectors.Count() - InactiveInspectorsCount;
                base.RaisePropertyChanged(nameof(InactiveInspectorsCount));

                var inspections = context
                            .Inspections
                            .ToList()
                            .Where(e => !e.IsDeleted)
                            .Select(e => new InspectionVM(e));
                Inspections = new ObservableCollection<InspectionVM>(inspections);
                base.RaisePropertyChanged(nameof(Inspections));

                NewInspectionsCount = inspections.Where(e => e.InspectionDate?.StartDate.Date >= GetLowerDateBound()).Count();
                base.RaisePropertyChanged(nameof(NewInspectorsCount));

                var customers = context
                    .Customers
                    .ToList()
                    .Where(e => !e.IsDeleted)
                    .Select(e => new CustomerVM(e));
                Customers = new ObservableCollection<CustomerVM>(customers);
                base.RaisePropertyChanged(nameof(Customers));

                NewCustomersCount = customers
                    .Where(e => e?.StartingDate >= GetLowerDateBound() &&
                    e?.StartingDate <= GetUpperDateBound())
                    .Count();
                base.RaisePropertyChanged(nameof(NewCustomersCount));
            }
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
