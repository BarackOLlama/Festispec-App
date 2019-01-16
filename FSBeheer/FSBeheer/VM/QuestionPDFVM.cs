using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSBeheer.VM
{
    class QuestionPDFVM
    {
        private QuestionVM _questionVM;

        public QuestionPDFVM(QuestionVM questionVM)
        {
            _questionVM = questionVM;
        }

        public string Content
        {
            get
            {
                return _questionVM.Content;
            }
            set
            {

            }
        }

        private bool _doNotShow;
        public bool DoNotShow
        {
            get
            {
                return _doNotShow;
            }
            set
            {
                _doNotShow = value;
                PieChart = BarChart = false;
            }
        }

        private bool _pieChart;
        public bool PieChart
        {
            get
            {
                return _pieChart;
            }
            set
            {
                _pieChart = value;
                DoNotShow = BarChart = false;
            }
        }

        private bool _barChart;
        public bool BarChart
        {
            get
            {
                return _barChart;
            }
            set
            {
                _barChart = value;
                DoNotShow = PieChart = false;
            }
        }
    }
}
