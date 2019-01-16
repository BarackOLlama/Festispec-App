using FSBeheer.VM;
using LiveCharts;
using LiveCharts.Wpf;
using LiveCharts.Wpf.Charts.Base;
using PdfSharp.Drawing;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace FSBeheer.PDF
{
    public class ChartGenerator
    {
        private CustomFSContext _context;
        public PieChart PieChart { get; set; }
        public CartesianChart BarChart { get; set; }

        public ObservableCollection<Option> Options { get; set; }
        public ObservableCollection<AnswerVM> Answers { get; set; }

        public ChartGenerator(QuestionVM SelectedQuestion, string ChartType)
        {
            _context = new CustomFSContext();

            Answers = _context.AnswerCrud.GetAllAnswersByQuestionId(SelectedQuestion.Id);
            Options = SelectedQuestion.OptionsDictionary;

            if (ChartType == "Pie")
            {
                PieChart = new PieChart();

                foreach (Option option in Options)
                {
                    //Determine how often this answer was given
                    int count = Answers.Where(a => a.Content == option.OptionString).Count();

                    if (count != 0)
                    {
                        PieChart.Series.Add(new PieSeries
                        {
                            Values = new ChartValues<int> { count },
                            DataLabels = true,
                            LabelPoint = chartpoint => string.Format("Antw. {0}, {1}x ({2:p})", option.Key, chartpoint.Y, chartpoint.Participation)
                        });
                    }
                }
            }
            else
            // bar
            {
                BarChart = new CartesianChart();

                BarChart.AxisY.Add(new Axis
                {
                    Title = "Aantal keer gegeven",
                    MinValue = 0,
                    MaxValue = Options.Count,
                    Separator = new Separator
                    {
                        Step = 1
                    }
                });
                BarChart.AxisX.Add(new Axis
                {
                    Title = "Opties",
                    ShowLabels = false
                });

                foreach (Option option in Options)
                {
                    //Determine how often this answer was given
                    int count = Answers.Where(a => a.Content == option.OptionString).Count();

                    if (count != 0)
                    {
                        BarChart.Series.Add(new ColumnSeries
                        {
                            Values = new ChartValues<int> { count },
                            DataLabels = true,
                            LabelPoint = point => string.Format("{0}: {1}x", option.Key, point.Y)
                        });
                    }
                }
                BarChart.MinHeight = 300;
            }
        }

        public XImage GetImageFromChart()
        {
            Chart chart;
            if (PieChart != null) chart = PieChart;
            else if (BarChart != null) chart = BarChart;
            else return null;

           

            chart.Width = 300;
            chart.Height = 300;

            chart.Update(true, true);

            var rtb = new RenderTargetBitmap((int)chart.ActualWidth, (int)chart.ActualHeight, 96, 96, PixelFormats.Pbgra32);
            rtb.Render(chart);

            return XImage.FromBitmapSource(rtb);
        }
    }
}
