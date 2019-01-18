using FSBeheer.VM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSBeheer.Crud
{
    public class QuestionPDFCrud : AbstractCrud
    {
        public QuestionPDFCrud(CustomFSContext customFSContext) : base(customFSContext)
        {
        }

        public ObservableCollection<QuestionPDFVM> ConvertQuestionVMToQuestionPDFVM(ObservableCollection<QuestionVM> questions)
        {
            var questionPDFs = new ObservableCollection<QuestionPDFVM>();
            foreach (QuestionVM questionVM in questions)
            {
                questionPDFs.Add(new QuestionPDFVM(questionVM));
            }
            return questionPDFs;
        }
    }
}
