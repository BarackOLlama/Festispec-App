using FSBeheer.ViewModel;
using FSBeheer.VM;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for CreateEditInspectionView.xaml
    /// </summary>
    public partial class CreateEditInspectionView : Window
    {
        public CreateEditInspectionView(InspectionVM inspection = null)
        {
            InitializeComponent();
            (DataContext as CreateEditInspectionViewModel).SetInspection(inspection);
        }
    }
}
