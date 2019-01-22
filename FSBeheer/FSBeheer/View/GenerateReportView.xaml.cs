using FSBeheer.ViewModel;


namespace FSBeheer.View
{
    /// <summary>
    /// Interaction logic for GenerateReportView.xaml
    /// </summary>
    public partial class GenerateReportView : BaseView
    {
        public GenerateReportView(int inspectionId = -1)
        {
            InitializeComponent();
            (DataContext as GenerateReportViewModel).SetInspection(inspectionId);
        }
    }
}
