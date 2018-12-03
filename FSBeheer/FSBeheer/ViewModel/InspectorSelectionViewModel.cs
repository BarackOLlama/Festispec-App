using FSBeheer.VM;
using GalaSoft.MvvmLight;
using System.Collections.ObjectModel;

namespace FSBeheer.ViewModel
{
    public partial class InspectorSelectionViewModel : ViewModelBase
    {
        private CustomFSContext _Context;

        private ObservableCollection<InspectorVM> _InspectorVM;

        public InspectorSelectionViewModel()
        {
            _Context = new CustomFSContext();
            _InspectorVM = _Context.InspectorCrud.GetAllInspectors();
        }
    }
}
