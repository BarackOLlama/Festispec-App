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

        public int? InspectorId
        {
            get
            {
                return _answer.InspectorId;
            }
        }

        public string OptionIndex
        {
            get { return _answer.Content.Split('|')[0]; }
        }
    }
}