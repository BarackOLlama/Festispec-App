using FSBeheer.View;
using FSBeheer.VM;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Caching;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FSBeheer.ViewModel
{
    public class InspectorManagementViewModel : ViewModelBase
    {

        private CustomFSContext _customFSContext;
        private InspectorVM _selectedInspector;
        public ObservableCollection<InspectorVM> Inspectors { get; set; }
        public RelayCommand<Window> BackHomeCommand { get; set; }
        public RelayCommand ShowEditInspectorViewCommand { get; set; }
        public RelayCommand ShowCreateInspectorViewCommand { get; set; }
        public RelayCommand DeleteInspectorCommand { get; set; }

        [DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(out int description, int reservedValue);
        public static bool IsInternetConnected()
        {
            return InternetGetConnectedState(out int description, 0);
        }

        public InspectorManagementViewModel()
        {
            Messenger.Default.Register<bool>(this, "UpdateInspectorList", il => Init()); // registratie, ontvangt (recipient is dit zelf) Observable Collection van CustomerVM en token is CustomerList, en voeren uiteindelijk init() uit, stap I

            Init();
            
            BackHomeCommand = new RelayCommand<Window>(CloseAction);
            ShowEditInspectorViewCommand = new RelayCommand(ShowEditInspectorView);
            ShowCreateInspectorViewCommand = new RelayCommand(ShowCreateInspectorView);
            DeleteInspectorCommand = new RelayCommand(DeleteInspector);
        }

        private void Init()
        {
            _customFSContext = new CustomFSContext();
            GetData();
        }

        private void GetData()
        {
            ObjectCache cache = MemoryCache.Default;
            CacheItemPolicy policy = new CacheItemPolicy
            {
                AbsoluteExpiration = DateTimeOffset.Now.AddDays(1)
            };
            if (IsInternetConnected())
            {
                Inspectors = _customFSContext.InspectorCrud.GetAllInspectors();
                cache.Set("inspectors", Inspectors, policy);
            }
            else
            {
                Inspectors = cache["inspectors"] as ObservableCollection<InspectorVM>;
                if (Inspectors == null)
                {
                    Inspectors = new ObservableCollection<InspectorVM>();
                }
            }
            RaisePropertyChanged(nameof(Inspectors));
        }

        public InspectorVM SelectedInspector
        {
            get
            {
                return _selectedInspector;
            }
            set
            {
                _selectedInspector = value;
                base.RaisePropertyChanged("SelectedInspector");
            }
        }

        private void ShowCreateInspectorView()
        {
            if(IsInternetConnected())
                new CreateEditInspectorView().Show();
            else
                MessageBox.Show("U bent niet verbonden met het internet. Probeer het later opnieuw.");
        }

        private void ShowEditInspectorView()
        {
            if(IsInternetConnected())
                new CreateEditInspectorView(SelectedInspector).Show();
            else
                MessageBox.Show("U bent niet verbonden met het internet. Probeer het later opnieuw.");
        }

        private void CloseAction(Window window)
        {
            window.Close();
        }

        private void DeleteInspector()
        {
            if (IsInternetConnected())
            {
                MessageBoxResult result = MessageBox.Show("Geselecteerde inspecteur verwijderen?", "Bevestiging verwijdering", MessageBoxButton.OKCancel);
                if (result == MessageBoxResult.OK)
                {
                    SelectedInspector.IsDeleted = true;
                    _customFSContext.SaveChanges();


                    Messenger.Default.Send(true, "UpdateInspectorList");
                }
            }
            else
            {
                MessageBox.Show("U bent niet verbonden met het internet. Probeer het later opnieuw.");
            }
        }

        public void FilterList(string filter)
        {
            if (string.IsNullOrEmpty(filter))
            {
                Inspectors = _customFSContext.InspectorCrud.GetAllInspectors();
            }
            else
            {
                Inspectors = _customFSContext.InspectorCrud.GetAllInspectorsFiltered(filter);
            }
            RaisePropertyChanged(nameof(Inspectors));
        }
    }
}
