using FSBeheer.VM;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSBeheer.ViewModel
{
    public class CreateQuestionnaireViewModel : ViewModelBase
    {
        private QuestionnaireVM _questionnaire;
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

        private CustomFSContext _Context;
        public ObservableCollection<QuestionnaireVM> Questionnaires { get; }

        public RelayCommand CreateQuestionnaireCommand { get; set; }





    }
}
