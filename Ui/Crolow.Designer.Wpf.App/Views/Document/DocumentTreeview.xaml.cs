using Crolow.Designer.Common.Constants;
using Crolow.Designer.Common.Extensions;
using Crolow.Designer.Runtime.Modules.DocumentModule.Commands.Documents.Events;
using Crolow.Designer.UI;
using Crolow.Designer.Wpf.App.Controls;
using System.Windows;
using System.Windows.Controls;

namespace Crolow.Designer.Wpf.App.Views.Document
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class DocumentTreeview : UserControl
    {
        public DocumentTreeview()
        {
            InitializeComponent();

            var document = new TreeNode { Text = "Document" };
            var page1 = new TreeNode { Parent = document, Text = "Page 1" };
            page1.Children.Add(new TreeNode { Parent = page1, Text = "Image" });
            page1.Children.Add(new TreeNode { Parent = page1, Text = "Text" });

            var page2 = new TreeNode { Parent = document, Text = "Page 2" };
            page2.Children.Add(new TreeNode { Text = "Rectangle" });

            document.Children.Add(page1);
            document.Children.Add(page2);

            DocumentTree.Nodes.Add(document);
            DocumentTree.Refresh();

            var documentSubscription = RuntimeController.Runtime.Events
                        .Subscribe<DocumentActivateEvent>(GuidSources.Documents.GenerateGuid(), OnActivateDocumentEvent);
        }

        private async Task OnActivateDocumentEvent(DocumentActivateEvent doc)
        {
            switch (doc.EventAction)
            {
                case EventAction.ObjectActivated:
                    DocumentTree.Clear();
                    break;
            }
        }

        private void EditDocument_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
