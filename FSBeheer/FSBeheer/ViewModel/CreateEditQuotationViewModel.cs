using FSBeheer.VM;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;

namespace FSBeheer.ViewModel
{
    public class CreateEditQuotationViewModel : ViewModelBase
    {
        public QuotationVM Quotation { get; set; }

        public QuotationVM SelectedQuotation { get; set; }

        public RelayCommand SaveChangesCommand { get; set; }

        public RelayCommand AddCommand { get; set; }

        public RelayCommand<Window> DiscardCommand { get; set; }

        private CustomFSContext _context;

        [DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(out int description, int reservedValue);
        public static bool IsInternetConnected()
        {
            return InternetGetConnectedState(out int description, 0);
        }

        public CreateEditQuotationViewModel(QuotationVM SelectedQuotation)
        {
            _context = new CustomFSContext();
            SaveChangesCommand = new RelayCommand(SaveChanges);
            Quotation = SelectedQuotation;
        }

        public CreateEditQuotationViewModel()
        {
            _context = new CustomFSContext();
        }

        private void AddQuotation()
        {
            _context.QuotationCrud.GetQuotations().Add(Quotation);
            _context.QuotationCrud.Add(Quotation);
        }

        private void SaveChanges()
        {
            if (IsInternetConnected())
            {
                MessageBoxResult result = MessageBox.Show("Wijzigingen opslaan?", "Bevestiging opslaan", MessageBoxButton.OKCancel);
                if (result == MessageBoxResult.OK)
                {
                    _context.QuotationCrud.GetQuotations().Add(Quotation);
                    _context.SaveChanges();

                    Messenger.Default.Send(true, "UpdateInspectorList"); // Stuurt object true naar ontvanger, die dan zijn methode init() uitvoert, stap II
                }
            }
            else
            {
                MessageBox.Show("U bent niet verbonden met het internet. Probeer het later opnieuw.");
            }
        }

        public void SetQuotation(QuotationVM quotationVM)
        {
            if (quotationVM == null)
            {
                Quotation = new QuotationVM();
                _context.Quotations.Add(Quotation.ToModel());
                RaisePropertyChanged(nameof(Quotation)); // a sign that a property has changed for viewing
            }
            else
            {
                Quotation = new QuotationVM(_context.Quotations.FirstOrDefault(q => q.Id == quotationVM.Id));
                RaisePropertyChanged(nameof(quotationVM));
            }
        }

    }
}
