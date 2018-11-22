using FSBeheer.Model;
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

namespace FSBeheer.ViewModel
{
    public class EditQuestionnaireViewModel : ViewModelBase
    {
        private QuestionnaireVM _questionnaire;
        private CustomFSContext _context;
        public QuestionnaireVM Questionnaire
        {
            get
            {
                return _questionnaire;
            }
            set
            {
                _questionnaire = value;
                base.RaisePropertyChanged("Questionnaire");
            }
        }
        public ObservableCollection<int?> InspectionNumbers { get; set; }
        private ObservableCollection<QuestionnaireVM> Questionnaires { get; set; }
        public ObservableCollection<QuestionVM> Questions { get; set; }
        private int? _selectedInspectioNumber;
        public int? SelectedInspectionNumber
        {
            get
            {
                return _selectedInspectioNumber;
            }
            set
            {
                _selectedInspectioNumber = value;
                base.RaisePropertyChanged("SelectedInspectionNumber");
            }
        }

        public RelayCommand OpenCreateQuestionViewCommand { get; set; }
        public RelayCommand SaveQuestionnaireChangesCommand { get; set; }

        public EditQuestionnaireViewModel(QuestionnaireVM questionnaire, ObservableCollection<QuestionnaireVM> questionnaires)
        {
            _questionnaire = questionnaire;
            Questionnaires = questionnaires;
            _context = new CustomFSContext();
            var questions = _context.Questions
                .Include("QuestionType")
                .ToList()
                .Where(e => e.QuestionnaireId == _questionnaire.Id)
                .Select(e => new QuestionVM(e));
            Questions = new ObservableCollection<QuestionVM>(questions);

            var inspectionNumbers = _context.Inspections
                .ToList()
                .Where(e => !e.IsDeleted)
                .Select(e => (int?)e.Id);
            InspectionNumbers = new ObservableCollection<int?>(inspectionNumbers);
            _selectedInspectioNumber = inspectionNumbers.FirstOrDefault();

            OpenCreateQuestionViewCommand = new RelayCommand(OpenCreateQuestionView);
            SaveQuestionnaireChangesCommand = new RelayCommand(SaveQuestionnaireChanges);

        }

        private void SaveQuestionnaireChanges()
        {
            //TODO
            _context.SaveChanges();
        }

        public void OpenCreateQuestionView()
        {
            new CreateQuestionView().ShowDialog();
        }
    }
}