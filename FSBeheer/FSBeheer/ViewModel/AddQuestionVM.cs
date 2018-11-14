using FSBeheer.Model;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSBeheer.ViewModel
{
    public class AddQuestionVM : ViewModelBase
    {
        private Question _question;

        public AddQuestionVM()
        {
            _question = new Question();
        }

        public Question Question
        {
            get
            {
                return _question;
            }
            set
            {
                _question = value;
                base.RaisePropertyChanged("Question");
            }
        }



    }
}
