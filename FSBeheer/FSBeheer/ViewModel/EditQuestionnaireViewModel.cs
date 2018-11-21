using FSBeheer.VM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSBeheer.ViewModel
{
    public class EditQuestionnaireViewModel
    {
        private QuestionnaireVM _questionnaireVM;

        public EditQuestionnaireViewModel(QuestionnaireVM questionnaireVM)
        {
            _questionnaireVM = questionnaireVM;
        }
    }
}
