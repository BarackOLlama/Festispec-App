using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace FSBeheer.View
{
    public class BaseView : Window
    {
        public BaseView()
        {
            double DefaultFontSize = 12; //Default "small" FontSize value

            var InterfaceButtonFactory = new FrameworkElementFactory(typeof(Button));
            InterfaceButtonFactory.SetValue(ContentProperty, new TemplateBindingExtension(ContentProperty));
            InterfaceButtonFactory.SetValue(Button.CommandProperty, new TemplateBindingExtension(Button.CommandProperty));
            InterfaceButtonFactory.SetValue(Button.CommandParameterProperty, new TemplateBindingExtension(Button.CommandParameterProperty));
            InterfaceButtonFactory.SetValue(FontSizeProperty, DefaultFontSize);
            InterfaceButtonFactory.SetValue(MarginProperty, new Thickness(5));
            InterfaceButtonFactory.SetValue(PaddingProperty, new Thickness(3));

            var SearchTextBoxFactory = new FrameworkElementFactory(typeof(FilterTextBox));
            SearchTextBoxFactory.SetValue(FontSizeProperty, DefaultFontSize);
            SearchTextBoxFactory.SetValue(WidthProperty, (double)300);
            SearchTextBoxFactory.SetValue(FilterTextBox.TextProperty, new TemplateBindingExtension(FilterTextBox.TextProperty));

            var ItemsPanelFactory = new FrameworkElementFactory(typeof(VirtualizingStackPanel));
            ItemsPanelFactory.SetValue(VirtualizingStackPanel.VirtualizationModeProperty, VirtualizationMode.Recycling);

            var FilteredComboBoxFactory = new FrameworkElementFactory(typeof(ComboBox));
            FilteredComboBoxFactory.SetValue(ComboBox.ItemsSourceProperty, new TemplateBindingExtension(ComboBox.ItemsSourceProperty));
            FilteredComboBoxFactory.SetValue(ComboBox.SelectedItemProperty, new TemplateBindingExtension(ComboBox.SelectedItemProperty));
            FilteredComboBoxFactory.SetValue(ComboBox.SelectedIndexProperty, new TemplateBindingExtension(ComboBox.SelectedIndexProperty));
            FilteredComboBoxFactory.SetValue(ComboBox.DisplayMemberPathProperty, new TemplateBindingExtension(ComboBox.DisplayMemberPathProperty));
            FilteredComboBoxFactory.SetValue(ComboBox.IsEditableProperty, true);
            FilteredComboBoxFactory.SetValue(ComboBox.IsTextSearchEnabledProperty, true);
            FilteredComboBoxFactory.SetValue(ComboBox.StaysOpenOnEditProperty, true);
            FilteredComboBoxFactory.SetValue(ComboBox.WidthProperty, (double)250);
            FilteredComboBoxFactory.SetValue(ComboBox.FontSizeProperty, DefaultFontSize);

            var CreateEditLabelFactory = new FrameworkElementFactory(typeof(Label));
            CreateEditLabelFactory.SetValue(Label.ContentProperty, new TemplateBindingExtension(Label.ContentProperty));
            CreateEditLabelFactory.SetValue(Label.HorizontalContentAlignmentProperty, HorizontalAlignment.Right);
            CreateEditLabelFactory.SetValue(Label.VerticalContentAlignmentProperty, VerticalAlignment.Center);
            CreateEditLabelFactory.SetValue(Label.FontSizeProperty, DefaultFontSize);
            CreateEditLabelFactory.SetValue(Label.WidthProperty, (double)75);

            var CreateEditTextBoxFactory = new FrameworkElementFactory(typeof(TextBox));
            CreateEditTextBoxFactory.SetValue(TextBox.TextProperty, new TemplateBindingExtension(TextBox.TextProperty));
            CreateEditTextBoxFactory.SetValue(TextBox.HorizontalAlignmentProperty, HorizontalAlignment.Left);
            CreateEditTextBoxFactory.SetValue(TextBox.PaddingProperty, new Thickness(3));
            CreateEditTextBoxFactory.SetValue(TextBox.FontSizeProperty, DefaultFontSize);
            CreateEditTextBoxFactory.SetValue(TextBox.WidthProperty, (double)250);


            this.Resources = new ResourceDictionary
            {
                {
                    "InterfaceButtonTemplate",
                    new ControlTemplate{ VisualTree = InterfaceButtonFactory }
                },
                {
                    "SearchTextBoxTemplate",
                    new ControlTemplate{ VisualTree = SearchTextBoxFactory }
                },
                {
                    "FilteredComboBoxTemplate",
                    new ControlTemplate{ VisualTree = FilteredComboBoxFactory }
                },
                {
                    "CreateEditLabelTemplate",
                    new ControlTemplate{ VisualTree = CreateEditLabelFactory }
                },
                {
                    "CreateEditTextBoxTemplate",
                    new ControlTemplate{ VisualTree = CreateEditTextBoxFactory }
                }
            };
        }
    }

    public class FilterTextBox : TextBox
    {
        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            Filter(Text);
            base.OnTextChanged(e);
        }

        private void Filter(string f)
        {
            var method = DataContext.GetType().GetMethod("FilterList"); // find method FilterList in your xaml's viewmodel
            if (method != null)
            { // if method exists
                method.Invoke(DataContext, new object[] { f }); // run it with parameter f
            }
        }
    }
}
