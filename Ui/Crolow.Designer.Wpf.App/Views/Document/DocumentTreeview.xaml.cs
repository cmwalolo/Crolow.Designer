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
            var newNode = new TreeNode { Text = node.Name, DataObject = node, IsVisible = true };
            if (parent != null)
            {
                parent.Children.Add(newNode);
                newNode.Parent = parent;
            }

            if (node is SceneNode sceneNode)
            {
                newNode.IsVisible = sceneNode.IsVisible;
            }

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
