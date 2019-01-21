﻿using FSBeheer.View;
using FSBeheer.VM;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Caching;
using System.Runtime.InteropServices;
using System.Windows;

namespace FSBeheer.ViewModel
{
    public class ScheduleManagementViewModel : ViewModelBase
    {
        private CustomFSContext _Context;

        public ObservableCollection<InspectorVM> Inspectors { get; set; }
        public InspectorVM SelectedInspector { get; set; }

        public ObservableCollection<ScheduleItemVM> ScheduleItems { get; set; }
        public ObservableCollection<ScheduleItemVM> SelectedScheduleItems { get; set; }

        public RelayCommand<Window> BackHomeCommand { get; set; }
        public RelayCommand GetScheduleItemsCommand { get; set; }
        public RelayCommand NewScheduleItemCommand { get; set; }
        public RelayCommand DeleteScheduleItemsCommand { get; set; }
        public RelayCommand CanExecuteChangedCommand { get; set; }
        public RelayCommand<object> DatagridSelectionChangedCommand { get; set; }

        [DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(out int description, int reservedValue);
        public static bool IsInternetConnected()
        {
            return InternetGetConnectedState(out int description, 0);
        }

        public ScheduleManagementViewModel()
        {
            Init();
            Messenger.Default.Register<bool>(this, "UpdateSchedule", e => Init());

            BackHomeCommand = new RelayCommand<Window>(CloseAction);
            GetScheduleItemsCommand = new RelayCommand(GetScheduleItems);
            DeleteScheduleItemsCommand = new RelayCommand(DeleteScheduleItems, ScheduleItemSelected);
            NewScheduleItemCommand = new RelayCommand(NewScheduleItem, InspectorSelected);
            CanExecuteChangedCommand = new RelayCommand(CanExecuteChanged);
            DatagridSelectionChangedCommand = new RelayCommand<object>(DataGridSelectionChanged);
        }

        public void Init()
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
                Inspectors = _Context.InspectorCrud.GetAllInspectors();
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

        public void GetScheduleItems()
        {
            if (IsInternetConnected())
            {
                if (SelectedInspector != null)
                {
                    ScheduleItems = _Context.ScheduleItemCrud.GetAllScheduleItemsByInspector(SelectedInspector.Id);
                }
                RaisePropertyChanged(nameof(ScheduleItems));
            }
            else
                MessageBox.Show("U bent niet verbonden met het internet. Probeer het later opnieuw.");
        }

        private void DataGridSelectionChanged(object dataset)
        {
            if(dataset != null)
                SelectedScheduleItems = new ObservableCollection<ScheduleItemVM>((dataset as IEnumerable).Cast<ScheduleItemVM>());
        }

        private void CanExecuteChanged()
        {
            DeleteScheduleItemsCommand.RaiseCanExecuteChanged();
            NewScheduleItemCommand.RaiseCanExecuteChanged();

            RaisePropertyChanged(nameof(SelectedInspector));
            RaisePropertyChanged(nameof(SelectedScheduleItems));
        }

        private bool InspectorSelected()
        {
            return SelectedInspector != null;
        }

        private bool ScheduleItemSelected()
        {
            return SelectedScheduleItems != null && SelectedScheduleItems.Count != 0;
        }

        private void CloseAction(Window window)
        {
            window.Close();
        }

        private void DeleteScheduleItems()
        {
            if (IsInternetConnected())
            {
                var result = MessageBox.Show("Weet u zeker dat u deze roosteritems wilt verwijderen?\nLet op: Alleen roosteritems van het type Verlof kunnen hier verwijderd worden.", "Bevestigen", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    foreach(var item in SelectedScheduleItems)
                    {

                    }
                }
            }
            else
                MessageBox.Show("U bent niet verbonden met het internet. Probeer het later opnieuw.");
        }

        private void NewScheduleItem()
        {
            if (IsInternetConnected())
            {
                
            }
            else
                MessageBox.Show("U bent niet verbonden met het internet. Probeer het later opnieuw.");
        }
    }
}