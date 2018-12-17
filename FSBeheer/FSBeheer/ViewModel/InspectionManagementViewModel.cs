using FSBeheer.VM;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.Windows;
using FSBeheer.View;
using System.Runtime.InteropServices;
using System.Runtime.Caching;
using System;

namespace FSBeheer.ViewModel
{
    public class InspectionManagementViewModel : ViewModelBase
    {
        private CustomFSContext _Context;
        public ObservableCollection<InspectionVM> Inspections { get; set; }
        private InspectionVM _SelectedInspection { get; set; }
        public InspectionVM SelectedInspection {
            get
            {
                return _SelectedInspection;
            }
            set
            {
                _SelectedInspection = value;
                base.RaisePropertyChanged(nameof(SelectedInspection));
            }
        }

        public RelayCommand ShowEditInspectionViewCommand { get; set; }
        public RelayCommand ShowCreateInspectionViewCommand { get; set; }
        //public RelayCommand<Window> BackHomeCommand { get; set; }

        [DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(out int description, int reservedValue);
        public static bool IsInternetConnected()
        {
            return InternetGetConnectedState(out int description, 0);
        }

        public InspectionManagementViewModel()
        {
            Messenger.Default.Register<bool>(this, "UpdateInspectionList", il => Init());
            Init();

            ShowEditInspectionViewCommand = new RelayCommand(ShowEditInspectionView);
            ShowCreateInspectionViewCommand = new RelayCommand(ShowCreateInspectionView);
            //BackHomeCommand = new RelayCommand<Window>(CloseAction);
        }

        internal void Init()
        {
            _Context = new CustomFSContext();
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
                Inspections = _Context.InspectionCrud.GetAllInspectionVMs();
                cache.Set("inspections", Inspections, policy);
            }
            else
            {
                Inspections = cache["inspections"] as ObservableCollection<InspectionVM>;
                if (Inspections == null)
                {
                    Inspections = new ObservableCollection<InspectionVM>();
                }
            }
            RaisePropertyChanged(nameof(Inspections));
        }

        private void CloseAction(Window window)
        {
            window.Close();
        }

        private void ShowEditInspectionView()
        {
            if (IsInternetConnected())
            {
                if (_SelectedInspection == null)
                {
                    MessageBox.Show("Er is geen inspectie geselecteerd. Kies een inspectie en kies daarna de optie 'Wijzig'.");
                }
                else
                {
                    new CreateEditInspectionView(_SelectedInspection).Show();
                }
            }
            else
            {
                MessageBox.Show("U bent niet verbonden met het internet. Probeer het later opnieuw.");
            }
        }

        private void ShowCreateInspectionView()
        {
            if(IsInternetConnected())
                new CreateEditInspectionView().Show();   
            else
                MessageBox.Show("U bent niet verbonden met het internet. Probeer het later opnieuw.");
        }
    }
}
