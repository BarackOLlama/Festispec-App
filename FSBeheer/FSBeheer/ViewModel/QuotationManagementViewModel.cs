using FSBeheer.Model;
using FSBeheer.View;
using FSBeheer.VM;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FSBeheer.ViewModel
{
    class QuotationManagementViewModel : ViewModelBase
    {
        private CustomFSContext _Context;

        public ObservableCollection<QuotationVM> Quotations { get; set; }
        private QuotationVM _SelectedQuotation { get; set; }
        public QuotationVM SelectedQuotation
        {
            get
            {
                return _SelectedQuotation;
            }
            set
            {
                _SelectedQuotation = value;
                base.RaisePropertyChanged(nameof(SelectedQuotation));
            }
        }
        
        public RelayCommand ShowEditQuotationViewCommand { get; set; }
        public RelayCommand ShowCreateQuotationViewCommand { get; set; }

        public QuotationManagementViewModel()
        {
            _Context = new CustomFSContext();
            Quotations = _Context.QuotationCrud.GetQuotations();

            ShowEditQuotationViewCommand = new RelayCommand(ShowEditQuotationView);
            ShowCreateQuotationViewCommand = new RelayCommand(ShowCreateQuotationView);
        }

        private void ShowEditQuotationView()
        {
            if (_SelectedQuotation == null)
            {
                MessageBox.Show("Er is geen inspectie geselecteerd. Kies een inspectie en kies daarna de optie 'Wijzig'.");
            }
            else
            {
                new CreateEditQuotationView(_SelectedQuotation).Show();
            }
        }

        private void ShowCreateQuotationView()
        {

        }
    }
}
