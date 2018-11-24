using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;

namespace FSBeheer.View
{
    public partial class BaseView : Window
    {
        public BaseView()
        {
            var InterfaceButtonFactory = new FrameworkElementFactory(typeof(Button));
            InterfaceButtonFactory.SetValue(ContentProperty, new TemplateBindingExtension(ContentProperty));
            InterfaceButtonFactory.SetValue(Button.CommandProperty, new TemplateBindingExtension(Button.CommandProperty));
            InterfaceButtonFactory.SetValue(Button.CommandParameterProperty, new TemplateBindingExtension(Button.CommandParameterProperty));
            InterfaceButtonFactory.SetValue(FontSizeProperty, (double)15);
            InterfaceButtonFactory.SetValue(MarginProperty, new Thickness(5));

            var SearchTextBoxFactory = new FrameworkElementFactory(typeof(TextBox));
            SearchTextBoxFactory.SetValue(FontSizeProperty, (double)15);
            SearchTextBoxFactory.SetValue(WidthProperty, (double)300);
            SearchTextBoxFactory.SetValue(TextBox.TextProperty, new TemplateBindingExtension(TextBox.TextProperty));

            var SearchButtonFactory = new FrameworkElementFactory(typeof(Button));
            SearchButtonFactory.SetValue(ContentProperty, new TemplateBindingExtension(ContentProperty));

            this.Resources = new ResourceDictionary
            {
                {
                    "InterfaceButton",
                    new ControlTemplate{ VisualTree = InterfaceButtonFactory }
                }
            };
        }
    }
}
