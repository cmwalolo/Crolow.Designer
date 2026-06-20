using Crolow.Designer.Core.Document;
using Crolow.Designer.UI;
using Crolow.Designer.Wpf.App.Views.Document;
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

            var doc = new DesignDocument();
            var vm = new DesignDocumentViewModel(doc);

            var dialog = new DesignDocumentEditorDialog
            {
                Owner = System.Windows.Application.Current.MainWindow,
                DataContext = vm
            };

            vm.RequestClose += (result) =>
            {
                dialog.DialogResult = result;
                dialog.Close();
            };

            bool? result = dialog.ShowDialog();

            if (result == true)
            {
                DocumentsController.Controller.NewDocument(doc);
            }
        }
    }
}
