using Crolow.Designer.UI;
using System.Windows;
using System.Windows.Controls;

namespace Crolow.Designer.Wpf.App.Views.Document
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class DocumentCanvas : UserControl, IDisposable
    {
        private DocumentController documentController;
        public DocumentCanvas(DocumentController documentController)
        {
            InitializeComponent();

            this.documentController = documentController;

            //var document = new TreeNode { Text = "Document" };
            //var page1 = new TreeNode { Parent = document, Text = "Page 1" };
            //page1.Children.Add(new TreeNode { Parent = page1, Text = "Image" });
            //page1.Children.Add(new TreeNode { Parent = page1, Text = "Text" });

            //var page2 = new TreeNode { Parent = document, Text = "Page 2" };
            //page2.Children.Add(new TreeNode { Text = "Rectangle" });

            //document.Children.Add(page1);
            //document.Children.Add(page2);

            //DocumentTree.Nodes.Add(document);
            DocumentTree.Refresh();
        }


        private void EditDocument_Click(object sender, RoutedEventArgs e)
        {

        }

        public void Dispose()
        {
        }
    }
}
