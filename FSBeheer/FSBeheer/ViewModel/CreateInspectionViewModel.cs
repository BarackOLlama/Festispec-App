using FSBeheer.VM;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSBeheer.ViewModel
{
    public class CreateInspectionViewModel : ViewModelBase
    {
        private CustomFSContext _Context;

        public ObservableCollection<CustomerVM> Customers { get; }
        public ObservableCollection<EventVM> Events { get; }

        public RelayCommand CancelInspectionCommand { get; set; }
        public RelayCommand AddInspectionCommand { get; set; }

        public CreateInspectionViewModel()
        {
            _Context = new CustomFSContext();
            Customers = _Context.CustomerCrud.GetAllCustomerVMs();
            Events = _Context.EventCrud.GetAllEventVMs();
            CancelInspectionCommand = new RelayCommand(CancelInspection);
            AddInspectionCommand = new RelayCommand(AddInspection);
        }

        public void CancelInspection()
        {
            // CreateInspectionView sluiten en veranderingen ongedaan maken
        }

        public void AddInspection()
        {
            // Inspectie aanmaken in de database met alle velden die ingevuld zijn
        }
    }
}
