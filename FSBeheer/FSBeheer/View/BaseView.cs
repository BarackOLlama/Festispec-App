using System;
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

            var SearchButtonFactory = new FrameworkElementFactory(typeof(Button));
            SearchButtonFactory.SetValue(ContentProperty, new TemplateBindingExtension(ContentProperty));
            SearchButtonFactory.SetValue(Button.CommandProperty, new TemplateBindingExtension(Button.CommandProperty));
            SearchButtonFactory.SetValue(Button.CommandParameterProperty, new TemplateBindingExtension(Button.CommandParameterProperty));
            SearchButtonFactory.SetValue(FontSizeProperty, DefaultFontSize);

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
                    "SearchButton",
                    new ControlTemplate { VisualTree = SearchButtonFactory }
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
            base.OnKeyUp(e);
            if (!string.IsNullOrEmpty(Text))
            {
                Filter(Text);
            }
        }

        private void Filter(string f)
        {
            DataContext.GetType().GetMethod("FilterList").Invoke(DataContext, new object[] { f });
        }
    }
}
