﻿using FSBeheer.Model;
using FSBeheer.View;
using FSBeheer.VM;
using GalaSoft.MvvmLight.Command;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using System;
using GalaSoft.MvvmLight;

namespace FSBeheer.VM
{
    public class QuestionnaireVM : ViewModelBase
    {
        private Questionnaire _questionnaire;
        public ObservableCollection<QuestionVM> Questions { get; set; }

        public QuestionnaireVM(Questionnaire questionnaire)
        {
            _questionnaire = questionnaire;
        }

        public QuestionnaireVM()
        {
            _questionnaire = new Questionnaire();
        }

        public int Id
        {
            get { return _questionnaire.Id; }
        }

        public string Name
        {
            get
            {
                return _questionnaire.Name;
            }
            set
            {
                _questionnaire.Name = value;
                base.RaisePropertyChanged("Name");
            }
        }

        public string Instructions
        {
            get
            {
                return _questionnaire.Instructions;
            }
            set
            {
                _questionnaire.Instructions = value;
                base.RaisePropertyChanged("Instructions");
            }
        }

        public string Comments
        {
            get
            {
                return _questionnaire.Comments;
            }
            set
            {
                _questionnaire.Comments = value;
                base.RaisePropertyChanged("Comments");
            }
        }

        public int? QuestionCount
        {
            get
            {
                int? amount = 0;
                amount = _questionnaire?.Questions?.Count();
                return amount;
            }
        }
        
        public Event Event
        {
            get { return _questionnaire.Inspection.Event; }
        }

        //for QuestionnaireManagementViewModel
        public string InspectionNumber
        {
            get
            {
                if (_questionnaire.InspectionId == null)
                {
                    return "unknown";
                }
                return _questionnaire.InspectionId.ToString();
            }
        }


        public bool IsDeleted
        {
            get { return _questionnaire.IsDeleted; }
            set { _questionnaire.IsDeleted = value; }
        }

        internal Questionnaire ToModel()
        {
            return _questionnaire;
        }
    }
}