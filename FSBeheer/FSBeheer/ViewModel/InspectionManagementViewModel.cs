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

        public RelayCommand ShowGenerateReportViewCommand { get; set; }
        public RelayCommand ShowEditInspectionViewCommand { get; set; }
        public RelayCommand ShowCreateInspectionViewCommand { get; set; }

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

            ShowGenerateReportViewCommand = new RelayCommand(ShowGenerateReportView);
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
                Inspections = _Context.InspectionCrud.GetAllInspections();
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

        private void ShowGenerateReportView()
        {
            if (IsInternetConnected())
            {
                if (_SelectedInspection == null)
                {
                    MessageBox.Show("Er is geen inspectie geselecteerd. Kies een inspectie en kies daarna de optie 'Genereer rapport'.");
                }
                else if (_Context.QuestionnaireCrud.GetQuestionnaireByInspectionId(_SelectedInspection.Id) == null)
                {
                    MessageBox.Show("De gekozen inspectie heeft geen vragenlijst. Kies een andere inspectie.");
                }
                else
                {
                    new GenerateReportView(_SelectedInspection.Id).Show();
                }
            }
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
                    new CreateEditInspectionView(_SelectedInspection.Id).Show();
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

        public void FilterList(string filter)
        {
            if (string.IsNullOrEmpty(filter))
            {
                Inspections = _Context.InspectionCrud.GetAllInspections();
            }
            else
            {
                Inspections = _Context.InspectionCrud.GetAllInspectionsFiltered(filter);
            }
            RaisePropertyChanged(nameof(Inspections));
        }
    }
}
