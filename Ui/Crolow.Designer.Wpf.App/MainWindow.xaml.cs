using ControlzEx.Theming;
using Crolow.Designer.UI;

namespace Crolow.Designer.Wpf.App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Fluent.RibbonWindow, IDisposable
    {
        private DocumentDockController dockController;
        public MainWindow(RuntimeController runtime, DocumentsController documents)
        {
            InitializeComponent();
            dockManager.Theme = new AvalonDock.Themes.ArcDarkTheme();

            ThemeManager.Current.ChangeTheme(
                System.Windows.Application.Current,
                "Dark.Red");

            dockController = new DocumentDockController(DocumentPane);

        }

        public void Dispose()
        {
            dockController.Dispose();
        }
    }
}