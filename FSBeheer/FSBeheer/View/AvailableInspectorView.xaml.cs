using FSBeheer.ViewModel;
using FSBeheer.VM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FSBeheer.View
{
    /// <summary>
    /// Interaction logic for AvailableInspectorsView.xaml
    /// </summary>
    public partial class AvailableInspectorView : BaseView
    {
        public AvailableInspectorView(CustomFSContext context, InspectionVM inspection, ObservableCollection<InspectorVM> availableInspectors)
        {
            InitializeComponent();
            (DataContext as AvailableInspectorViewModel).SetContextInspectionId(context, inspection, availableInspectors);
        }
    }
}
