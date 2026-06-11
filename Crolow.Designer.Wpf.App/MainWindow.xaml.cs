using ControlzEx.Theming;

namespace Crolow.Designer.Wpf.App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Fluent.RibbonWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            dockManager.Theme = new AvalonDock.Themes.ArcDarkTheme();

            ThemeManager.Current.ChangeTheme(
                System.Windows.Application.Current,
                "Dark.Cobalt");
        }
    }
}