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
    /// Interaction logic for CreateEditContactView.xaml
    /// </summary>
    public partial class CreateEditContactView : BaseView
    {
        public CreateEditContactView(CustomerVM customer = null)
        {
            InitializeComponent();
            (DataContext as CreateEditContactViewModel).SetContact(customer);
        }
    }
}
