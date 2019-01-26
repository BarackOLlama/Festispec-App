using FSBeheer.Model;

namespace FSBeheer.VM
{
    public class AnswerVM
    {
        public Answer _answer;
        public AnswerVM(Answer a)
        {
            _answer = a;
        }

        public int Id
        {
            get { return _answer.Id; }
        }

        public string Content
        {
            get { return _answer.Content; }
        }

        public int? QuestionId
        {
            get { return _answer.QuestionId; }
        }

        public InspectorVM Inspector
        {
            get
            {
                return new InspectorVM(_answer.Inspector);
            }
        }

        public string OptionIndex
        {
            get { return _answer.Content.Split('|')[0]; }
        }

        internal Answer ToModel()
        {
            return _answer;
        }
    }
}