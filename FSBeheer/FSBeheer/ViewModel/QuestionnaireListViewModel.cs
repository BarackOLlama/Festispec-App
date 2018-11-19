using FSBeheer.Model;
using FSBeheer.View;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FSBeheer.ViewModel
{
    public class QuestionnaireListViewModel : ViewModelBase
    {
        //PLURAL FORM
        public ObservableCollection<QuestionnaireViewModel> Questionnaires { get; set; }
        private QuestionnaireViewModel _selectedQuestionnaire;

        private QuestionnaireEditView _questionnaireEditView;

        public ICommand ViewQuestionnaireCommand { get; set; }

        public QuestionnaireViewModel SelectedQuestionnaire
        {
            get
            {
                return _selectedQuestionnaire;
            }
            set
            {
                _selectedQuestionnaire = value;
                base.RaisePropertyChanged("SelectedQuestionnaire");
            }
        }

        public QuestionnaireListViewModel()
        {
            using (var context = new FSContext())
            {
                var result = context.Questionnaires.Include("Questions").ToList().Select(e => new QuestionnaireViewModel(e));
                Questionnaires = new ObservableCollection<QuestionnaireViewModel>(result);
            }
            SelectedQuestionnaire = Questionnaires?.First();
            ViewQuestionnaireCommand = new RelayCommand(OpenCreateQuestionWindow);
        }


        public void OpenCreateQuestionWindow()
        {
            new QuestionnaireEditView().ShowDialog();
        }
    }
}