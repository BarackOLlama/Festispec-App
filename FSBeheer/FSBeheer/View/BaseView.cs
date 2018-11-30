﻿using System.Collections;
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
            double DefaultFontSize = 15; //Default "small" FontSize value

            var InterfaceButtonFactory = new FrameworkElementFactory(typeof(Button));
            InterfaceButtonFactory.SetValue(ContentProperty, new TemplateBindingExtension(ContentProperty));
            InterfaceButtonFactory.SetValue(Button.CommandProperty, new TemplateBindingExtension(Button.CommandProperty));
            InterfaceButtonFactory.SetValue(Button.CommandParameterProperty, new TemplateBindingExtension(Button.CommandParameterProperty));
            InterfaceButtonFactory.SetValue(FontSizeProperty, DefaultFontSize);
            InterfaceButtonFactory.SetValue(MarginProperty, new Thickness(5));

            var SearchTextBoxFactory = new FrameworkElementFactory(typeof(FilterTextBox));
            SearchTextBoxFactory.SetValue(FontSizeProperty, DefaultFontSize);
            SearchTextBoxFactory.SetValue(WidthProperty, (double)300);
            SearchTextBoxFactory.SetValue(FilterTextBox.TextProperty, new TemplateBindingExtension(FilterTextBox.TextProperty));

            var ItemsPanelFactory = new FrameworkElementFactory(typeof(VirtualizingStackPanel));
            ItemsPanelFactory.SetValue(VirtualizingStackPanel.VirtualizationModeProperty, VirtualizationMode.Recycling);

            var FilteredComboBoxFactory = new FrameworkElementFactory(typeof(FilteredComboBox));
            FilteredComboBoxFactory.SetValue(FilteredComboBox.ItemsSourceProperty, new TemplateBindingExtension(FilteredComboBox.ItemsSourceProperty));
            FilteredComboBoxFactory.SetValue(FilteredComboBox.SelectedItemProperty, new TemplateBindingExtension(FilteredComboBox.SelectedItemProperty));
            FilteredComboBoxFactory.SetValue(FilteredComboBox.DisplayMemberPathProperty, new TemplateBindingExtension(FilteredComboBox.DisplayMemberPathProperty));
            FilteredComboBoxFactory.SetValue(FilteredComboBox.IsEditableProperty, true);
            FilteredComboBoxFactory.SetValue(FilteredComboBox.IsTextSearchEnabledProperty, false);
            FilteredComboBoxFactory.SetValue(FilteredComboBox.StaysOpenOnEditProperty, true);
            FilteredComboBoxFactory.SetValue(FilteredComboBox.ItemsPanelProperty, new ItemsPanelTemplate() { VisualTree = ItemsPanelFactory });
            FilteredComboBoxFactory.SetValue(FilteredComboBox.WidthProperty, (double)400);
            FilteredComboBoxFactory.SetValue(FilteredComboBox.FontSizeProperty, (double)20);

            var CreateEditLabelFactory = new FrameworkElementFactory(typeof(Label));
            CreateEditLabelFactory.SetValue(Label.ContentProperty, new TemplateBindingExtension(Label.ContentProperty));
            CreateEditLabelFactory.SetValue(Label.HorizontalAlignmentProperty, HorizontalAlignment.Right);
            CreateEditLabelFactory.SetValue(Label.VerticalAlignmentProperty, VerticalAlignment.Center);
            CreateEditLabelFactory.SetValue(Label.FontSizeProperty, (double)12);

            var CreateEditTextBoxFactory = new FrameworkElementFactory(typeof(TextBox));
            CreateEditTextBoxFactory.SetValue(TextBox.TextProperty, new TemplateBindingExtension(TextBox.TextProperty));
            CreateEditTextBoxFactory.SetValue(TextBox.HorizontalAlignmentProperty, HorizontalAlignment.Left);
            CreateEditTextBoxFactory.SetValue(TextBox.PaddingProperty, new Thickness(3));
            CreateEditTextBoxFactory.SetValue(TextBox.FontSizeProperty, (double)12);
            CreateEditTextBoxFactory.SetValue(TextBox.WidthProperty, (double)250);


            this.Resources = new ResourceDictionary
            {
                {
                    "InterfaceButton",
                    new ControlTemplate{ VisualTree = InterfaceButtonFactory }
                },
                {
                    "SearchTextBox",
                    new ControlTemplate{ VisualTree = SearchTextBoxFactory }
                },
                {
                    "FilteredComboBox",
                    new ControlTemplate{ VisualTree = FilteredComboBoxFactory }
                },
                {
                    "CreateEditLabel",
                    new ControlTemplate{ VisualTree = CreateEditLabelFactory }
                },
                {
                    "CreateEditTextBox",
                    new ControlTemplate{ VisualTree = CreateEditTextBoxFactory }
                }
            };
        }
    }

    public class FilteredComboBox : ComboBox
    {
        private string oldFilter = string.Empty;

        private string currentFilter = string.Empty;

        protected TextBox EditableTextBox => GetTemplateChild("PART_EditableTextBox") as TextBox;

        protected override void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
        {
            if (newValue != null)
            {
                var view = CollectionViewSource.GetDefaultView(newValue);
                view.Filter += FilterItem;
            }

            if (oldValue != null)
            {
                var view = CollectionViewSource.GetDefaultView(oldValue);
                if (view != null) view.Filter -= FilterItem;
            }

            base.OnItemsSourceChanged(oldValue, newValue);
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Tab:
                case Key.Enter:
                    IsDropDownOpen = false;
                    break;
                case Key.Escape:
                    IsDropDownOpen = false;
                    SelectedIndex = -1;
                    Text = currentFilter;
                    break;
                default:
                    if (e.Key == Key.Down) IsDropDownOpen = true;

                    base.OnPreviewKeyDown(e);
                    break;
            }

            // Cache text
            oldFilter = Text;
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Up:
                case Key.Down:
                    break;
                case Key.Tab:
                case Key.Enter:

                    ClearFilter();
                    break;
                default:
                    if (Text != oldFilter)
                    {
                        RefreshFilter();
                        IsDropDownOpen = true;

                        EditableTextBox.SelectionStart = int.MaxValue;
                    }

                    base.OnKeyUp(e);
                    currentFilter = Text;
                    break;
            }
        }

        protected override void OnPreviewLostKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            ClearFilter();
            var temp = SelectedIndex;
            SelectedIndex = -1;
            Text = string.Empty;
            SelectedIndex = temp;
            base.OnPreviewLostKeyboardFocus(e);
        }

        private void RefreshFilter()
        {
            if (ItemsSource == null) return;

            var view = CollectionViewSource.GetDefaultView(ItemsSource);
            view.Refresh();
        }

        private void ClearFilter()
        {
            currentFilter = string.Empty;
            RefreshFilter();
        }

        private bool FilterItem(object value)
        {
            if (value == null) return false;
            if (Text.Length == 0) return true;

            return value.ToString().ToLower().Contains(Text.ToLower());
        }
    }

    public class FilterTextBox : TextBox
    {
        protected override void OnKeyUp(KeyEventArgs e)
        {
            Filter(Text);
            e.Handled = true;
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
