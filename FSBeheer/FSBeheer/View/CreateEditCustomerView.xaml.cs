﻿using FSBeheer.ViewModel;
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
    /// Interaction logic for CreateEditCustomerView.xaml
    /// </summary>
    public partial class CreateEditCustomerView : BaseView
    {
        public CreateEditCustomerView(CustomerVM customer = null)
        {
            InitializeComponent();
            // Zijn datacontext is een create edit, hier kun je hem nog zetten, bij viewmodel hoef je niet meer bij te houden wat er allemaal gebeurt
            (DataContext as CreateEditCustomerViewModel).SetCustomer(customer);
        }

    }
}
