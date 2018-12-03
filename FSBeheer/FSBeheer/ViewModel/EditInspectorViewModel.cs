using FSBeheer.VM;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FSBeheer.ViewModel
{
    public class EditInspectorViewModel : ViewModelBase
    {

        public InspectorVM Inspector { get; set; }

        public RelayCommand EditCommand { get; set; }

        public RelayCommand AddCommand { get; set; }

        public RelayCommand<Window> DiscardCommand { get; set; }

        // prop ContactVM

        private CustomFSContext _Context;

        public EditInspectorViewModel(InspectorVM SelectedInspector)
        {
            _Context = new CustomFSContext();
            //EditCommand = new RelayCommand(ModifyInspector);
            AddCommand = new RelayCommand(AddInspector);
            DiscardCommand = new RelayCommand<Window>(Discard);

            // try catch
            if (SelectedInspector != null)
            {
                Inspector = SelectedInspector;
                // contact van deze customer
            }
            else
            {
                Inspector = new InspectorVM();
                // Contact aanmaken
            }
        }

        private void AddInspector()
        {
            // not tested yet
            _Context.InspectorCrud.GetAllInspectors().Add(Inspector);
            //_Context.InspectorCrud.Add(Inspector);
        }

        // Not tested yet
        //private void ModifyInspector() => _Context.InspectorCrud.Modify(Inspector);


        // Not tested yet
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
