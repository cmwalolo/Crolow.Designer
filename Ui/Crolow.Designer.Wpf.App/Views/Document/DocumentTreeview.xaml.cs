using Crolow.Designer.Common.Constants;
using Crolow.Designer.Common.Extensions;
using Crolow.Designer.Core.Scene.Nodes;
using Crolow.Designer.Runtime.Modules.DocumentModule.Commands.Documents.Events;
using Crolow.Designer.UI;
using Crolow.Designer.Wpf.App.Controls;
using System.Windows;
using System.Windows.Controls;
using data = Crolow.Designer.Common.Data;

namespace Crolow.Designer.Wpf.App.Views.Document
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class DocumentTreeview : UserControl, IDisposable
    {
        private IDisposable documentSubscription;

        DocumentController documentController;

        public DocumentTreeview()
        {
            InitializeComponent();

            //var document = new TreeNode { Text = "Document" };
            //var page1 = new TreeNode { Parent = document, Text = "Page 1" };
            //page1.Children.Add(new TreeNode { Parent = page1, Text = "Image" });
            //page1.Children.Add(new TreeNode { Parent = page1, Text = "Text" });

            //var page2 = new TreeNode { Parent = document, Text = "Page 2" };
            //page2.Children.Add(new TreeNode { Text = "Rectangle" });

            //document.Children.Add(page1);
            //document.Children.Add(page2);

            //DocumentTree.Nodes.Add(document);
            //DocumentTree.Refresh();

            documentSubscription = RuntimeController.Runtime.Events
                        .Subscribe<DocumentActivatedEvent>(GuidSources.Documents.GenerateGuid(), OnActivateDocumentEvent);
        }



        private async Task OnActivateDocumentEvent(DocumentActivatedEvent doc)
        {
            documentController = doc.Target.FirstOrDefault();
            switch (doc.EventAction)
            {
                case EventAction.ObjectActivated:
                    DocumentTree.Clear();
                    DocumentTree.Nodes.Clear();
                    foreach (var node in documentController.Session.Document.Pages)
                    {
                        DocumentTree.Nodes.Add(AddNodes(node, null));
                    }
                    DocumentTree.Refresh();
                    break;
            }
        }

        private TreeNode AddNodes(data.IDataObject node, TreeNode parent)
        {
            var newNode = new TreeNode { Text = node.Name };

            if (node is GroupNode groupNode)
            {
                foreach (var child in groupNode.Children)
                {
                    AddNodes(child, newNode);
                }
            }

            return newNode;
        }

        private void EditDocument_Click(object sender, RoutedEventArgs e)
        {

        }

        public void Dispose()
        {
            documentSubscription?.Dispose();
        }
    }
}
