using ControlzEx.Theming;
using Crolow.Designer.UI;

namespace Crolow.Designer.Wpf.App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Fluent.RibbonWindow
    {
        public MainWindow(RuntimeController runtime, DocumentsController documents)
        {
            InitializeComponent();
            dockManager.Theme = new AvalonDock.Themes.ArcDarkTheme();

            ThemeManager.Current.ChangeTheme(
                System.Windows.Application.Current,
                "Dark.Red");
        }
    }
}