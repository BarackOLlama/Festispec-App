﻿using FSBeheer.Model;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSBeheer.VM
{
    class InspectionVM : ViewModelBase
    {
        private Inspection _inspection;

        public InspectionVM(Inspection inspection)
        {
            _inspection = inspection;
        }

        internal Inspection ToModel()
        {
            return _inspection;
        }

        public int Id
        {
            get { return _inspection.Id; }
        }

    }
}