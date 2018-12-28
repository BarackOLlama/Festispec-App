using FSBeheer.VM;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FSBeheer.ViewModel
{
    public class CreateEditInspectorViewModel : ViewModelBase
    {

        public InspectorVM Inspector { get; set; }

        public InspectorVM SelectedInspector { get; set; }

        public RelayCommand SaveChangesCommand { get; set; }

        public RelayCommand AddCommand { get; set; }

        public RelayCommand<Window> DiscardCommand { get; set; }

        [DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(out int description, int reservedValue);
        public static bool IsInternetConnected()
        {
            return InternetGetConnectedState(out int description, 0);
        }

        private CustomFSContext _context;

        public CreateEditInspectorViewModel()
        {
            _context = new CustomFSContext();
            SaveChangesCommand = new RelayCommand(SaveChanges);
            Inspector = SelectedInspector;
        }

        private void AddInspector()
        {
            _context.InspectorCrud.GetAllInspectors().Add(Inspector);
        }


        private void SaveChanges()
        {
            if (IsInternetConnected())
            {
                MessageBoxResult result = MessageBox.Show("Wijzigingen opslaan?", "Bevestiging opslaan", MessageBoxButton.OKCancel);
                if (result == MessageBoxResult.OK)
                {
                    _context.InspectorCrud.GetAllInspectors().Add(Inspector);
                    _context.SaveChanges();

                    Messenger.Default.Send(true, "UpdateInspectorList"); // Stuurt object true naar ontvanger, die dan zijn methode init() uitvoert, stap II
                }
            }
            else
            {
                MessageBox.Show("U bent niet verbonden met het internet. Probeer het later opnieuw.");
            }
        }

        public void SetInspector(InspectorVM inspector)
        {
            if (inspector == null)
            {
                Inspector = new InspectorVM();
                _context.Inspectors.Add(Inspector.ToModel());
            }
            else
            {
                Inspector = new InspectorVM(_context.Inspectors.FirstOrDefault(c => c.Id == inspector.Id));
            }
            RaisePropertyChanged(nameof(Inspector)); // a sign that a property has changed for viewing
        }

        private void Discard(Window window)
        {
            MessageBoxResult result = MessageBox.Show("Sluiten zonder opslaan?", "Bevestiging annulering", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.Cancel)
            {
                _context.Dispose();
                Inspector = null;
                window?.Close();
            }
        }



    }
}
