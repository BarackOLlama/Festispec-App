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
        private QuotationVM _SelectedQuotation { get; set; }
        public QuotationVM SelectedQuotation
        {
            get { return _SelectedQuotation; }
            set
            {
                _SelectedQuotation = value;
                RaisePropertyChanged(nameof(SelectedQuotation));
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
            if (SelectedQuotation == null)
            {
                MessageBox.Show("Er is geen inspectie geselecteerd. Kies een inspectie en kies daarna de optie 'Wijzig'.");
            }
            else
            {
                new CreateEditQuotationView(SelectedQuotation).Show();
            }
        }

        private void ShowCreateQuotationView()
        {
            new CreateEditQuotationView().Show();
        }
    }
}
