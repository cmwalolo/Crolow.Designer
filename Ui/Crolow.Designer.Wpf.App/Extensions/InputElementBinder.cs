using System.Windows;
using System.Windows.Controls;

namespace Crolow.Designer.Wpf.App.Extensions
{
    public static class InputElementBinder
    {
        public static void UpdateBindings(this IInputElement element)
        {
            if (element is not FrameworkElement fe)
                return;

            DependencyProperty? dp = fe switch
            {
                TextBox => TextBox.TextProperty,
                ComboBox => ComboBox.SelectedItemProperty,
                CheckBox => CheckBox.IsCheckedProperty,
                _ => null
            };

            if (dp is not null)
            {
                fe.GetBindingExpression(dp)?.UpdateSource();
            }
        }
    }
}
