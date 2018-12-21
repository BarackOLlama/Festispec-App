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

        private CustomFSContext _Context;

        public CreateEditInspectorViewModel()
        {
            _Context = new CustomFSContext();
            SaveChangesCommand = new RelayCommand(SaveChanges);
            Inspector = SelectedInspector;
        }

        public CreateEditInspectorViewModel()
        {
            _Context = new CustomFSContext();
        }

        private void AddInspector()
        {
            _Context.InspectorCrud.GetAllInspectors().Add(Inspector);
            _Context.InspectorCrud.Add(Inspector);
        }


        private void SaveChanges()
        {
            if (IsInternetConnected())
            {
                MessageBoxResult result = MessageBox.Show("Save changes?", "Confirm action", MessageBoxButton.OKCancel);
                if (result == MessageBoxResult.OK)
                {
                    _Context.InspectorCrud.GetAllInspectors().Add(Inspector);
                    _Context.SaveChanges();

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
                _Context.Inspectors.Add(Inspector.ToModel());
            }
            else
            {
                Inspector = new InspectorVM(_Context.Inspectors.FirstOrDefault(c => c.Id == inspector.Id));
            }
            RaisePropertyChanged(nameof(Inspector)); // a sign that a property has changed for viewing
        }

        private void Discard(Window window)
        {
            MessageBoxResult result = MessageBox.Show("Close without saving?", "Confirm discard", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.Cancel)
            {
                _Context.Dispose();
                Inspector = null;
                window?.Close();
            }
        }



    }
}
