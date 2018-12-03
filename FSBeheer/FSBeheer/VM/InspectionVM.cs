﻿using FSBeheer.Model;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSBeheer.VM
{
    public class InspectionVM : ViewModelBase
    {
        private Inspection _inspection;

        public InspectionVM(Inspection inspection)
        {
            _inspection = inspection;
        }

        public int Id
        {
            get { return _inspection.Id; }
        }

        public string Name
        {
            get { return _inspection.Name; }
        }

        public string Notes
        {
            get { return _inspection.Notes; }
        }

        public int? EventId
        {
            get { return _inspection.EventId; }
        }

        public Event Event
        {
            get { return _inspection.Event; }
            set { _inspection.Event = value; }
        }

        public int? StatusId
        {
            get { return _inspection.StatusId; }
        }

        public Status Status
        {
            get { return _inspection.Status; }
            set { _inspection.Status = value; }
        }

        public InspectionDate InspectionDate
        {
            get { return _inspection.InspectionDate; }
            set { _inspection.InspectionDate = value; }
        }

        public ObservableCollection<Inspector> Inspectors
        {
            get { return _inspection.Inspectors; }
        }

        public bool IsDeleted
        {
            get { return _inspection.IsDeleted; }
            set { _inspection.IsDeleted = value; RaisePropertyChanged(nameof(IsDeleted)); }
        }

        internal Inspection ToModel()
        {
            return _inspection;
        }
    }
}
