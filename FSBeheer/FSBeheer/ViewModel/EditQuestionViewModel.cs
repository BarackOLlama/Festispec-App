﻿using FSBeheer.VM;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSBeheer.ViewModel
{
    //vragen aanpassen (de content zelf)
    //antwoorden toevoegen
    //antwoorden wijzigen (content)
    public class EditQuestionViewModel :ViewModelBase
    {
        private QuestionVM _question;
        public QuestionVM Question { get
            {
                return _question;
            }
            set
            {
                _question = value;
                base.RaisePropertyChanged("Question");
            }
        }
        private QuestionTypeVM _questionType;
        public QuestionTypeVM SelectedQuestionType
        {
            get
            {
                return _questionType;
            }
            set
            {
                _questionType = value;
                base.RaisePropertyChanged("QuestionType");
            }
        }

        private CustomFSContext _context;
        public ObservableCollection<QuestionTypeVM> QuestionTypes { get; set; }

        public RelayCommand SaveQuestionChangesCommand { get; set; }
        public RelayCommand DiscardQuestionChangesCommand { get; set; }
        public EditQuestionViewModel(QuestionVM question)
        {
            _context = new CustomFSContext();
            _question = question;
            var temp = _context.QuestionTypes.ToList().Select(e => new QuestionTypeVM(e));
            QuestionTypes = new ObservableCollection<QuestionTypeVM>(temp);
            SaveQuestionChangesCommand = new RelayCommand(SaveQuestionChanges);
            DiscardQuestionChangesCommand = new RelayCommand(DiscardQuestionChanges);
            SelectedQuestionType = QuestionTypes.FirstOrDefault();
        }

        public void SaveQuestionChanges()
        {
            //_context.QuestionCrud.GetAllQuestionVMs().Add(_question);
            _context.ChangeTracker.DetectChanges();
            _context.SaveChanges();
        }

        public void DiscardQuestionChanges()
        {
            throw new NotImplementedException();
        }
    }
}
