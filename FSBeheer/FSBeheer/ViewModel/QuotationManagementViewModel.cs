using FSBeheer.View;
using FSBeheer.VM;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;
using System.Windows;

namespace FSBeheer.ViewModel
{
    public class QuotationManagementViewModel : ViewModelBase
    {
        private CustomFSContext _Context;

        public ObservableCollection<QuotationVM> Quotations { get; set; }
        private QuotationVM _quotation { get; set; }
        public QuotationVM Quotation
        {
            get { return _quotation; }
            set
            {
                _quotation = value;
                RaisePropertyChanged(nameof(Quotation));
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
            if (Quotation == null)
            {
                MessageBox.Show("Er is geen inspectie geselecteerd. Kies een inspectie en kies daarna de optie 'Wijzig'.");
            }
            else
            {
                new CreateEditQuotationView(Quotation).Show();
            }
        }

        private void ShowCreateQuotationView()
        {
            new CreateEditQuotationView().Show();
        }
    }
}
