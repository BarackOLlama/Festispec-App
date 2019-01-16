using FSBeheer.VM;
using LiveCharts;
using LiveCharts.Wpf;
using LiveCharts.Wpf.Charts.Base;
using PdfSharp.Drawing;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
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

        public ChartGenerator(QuestionVM SelectedQuestion, string ChartType, int width, int height)
        {
            _context = new CustomFSContext();

            Answers = _context.AnswerCrud.GetAllAnswersByQuestionId(SelectedQuestion.Id);
            Options = SelectedQuestion.OptionsDictionary;

            bool disableAnim = true;
            int w = width;
            int h = height;

            if (ChartType == "Pie")
            {
                PieChart = new PieChart
                {
                    DisableAnimations = disableAnim,
                    Width = w,
                    Height = h
                };

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
                BarChart = new CartesianChart
                {
                    DisableAnimations = disableAnim,
                    Width = w,
                    Height = h
                };

                BarChart.AxisY.Add(new Axis
                {
                    Title = "Aantal keer gegeven",
                    MinValue = 0,
                    MaxValue = Options.Count,
                    Separator = new LiveCharts.Wpf.Separator
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
            }
        }

        public XImage GetImageFromChart()
        {
            Chart chart;
            if (PieChart != null) chart = PieChart;
            else if (BarChart != null) chart = BarChart;
            else return null;

            var viewbox = new Viewbox
            {
                Child = chart
            };
            viewbox.Measure(chart.RenderSize);
            viewbox.Arrange(new Rect(new Point(0, 0), chart.RenderSize));
            chart.Update(true, true); //force chart redraw
            viewbox.UpdateLayout();

            var rtb = new RenderTargetBitmap((int)chart.ActualWidth, (int)chart.ActualHeight, 96, 96, PixelFormats.Pbgra32);
            rtb.Render(chart);

            return XImage.FromBitmapSource(rtb);
        }
    }
}
