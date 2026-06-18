using Crolow.Designer.UI;
using System.Windows.Controls;

namespace Crolow.Designer.Wpf.App.Views
{
    /// <summary>
    /// Interaction logic for MainPageRibbon.xaml
    /// </summary>
    public partial class MainPageRibbon : UserControl
    {
        public MainPageRibbon()
        {
            InitializeComponent();
        }

        private void NewDocument_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            DocumentsController.Controller.NewDocument(new Core.Document.DesignDocument());
        }
    }
}
