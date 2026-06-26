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
        private IDisposable documentSubscription, nodesSubscription;

        DocumentController documentController;

        public DocumentTreeview()
        {

            InitializeComponent();

            documentSubscription = RuntimeController.Runtime.Events
                        .Subscribe<DocumentActivatedEvent>(GuidSources.Documents.GenerateGuid(), OnActivateDocumentEvent);

        }

        private async Task OnSceneNodeEvent(NodeEvent args)
        {
            switch (args.EventAction)
            {
                case Common.Constants.EventAction.ObjectCreated:
                    foreach (var node in args.Target)
                    {
                        AddNodesInPlace(node);
                        DocumentTree.Refresh();
                    }
                    break;
            }
        }

        private async Task OnActivateDocumentEvent(DocumentActivatedEvent doc)
        {
            documentController = doc.Target.FirstOrDefault();
            switch (doc.EventAction)
            {
                case EventAction.ObjectActivated:
                    nodesSubscription?.Dispose();
                    nodesSubscription = RuntimeController.Runtime.Events
                            .Subscribe<NodeEvent>(documentController.Session.Document.Id, OnSceneNodeEvent);

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

        private void AddNodesInPlace(data.IDataObject node)
        {
            var parentNode = FindNode(node.ParentId);
            if (parentNode != null)
            {
                var newNode = new TreeNode { Text = node.Name, DataObject = node, IsVisible = true };
                parentNode.Children.Add(newNode);
                newNode.Parent = parentNode;
            }
        }

        private TreeNode FindNode(Guid parentId, TreeNode parent = null)
        {
            List<TreeNode> nodes = new();

            if (parent == null)
            {
                nodes = DocumentTree.Nodes.ToList(); ;
            }
            else
            {
                nodes = parent.Children.ToList();
            }

            var foundNode = nodes.Find(p => p.DataObject.Id == parentId);
            if (foundNode == null)
            {
                foreach (var node in nodes)
                {
                    foundNode = FindNode(parentId, node);
                    if (foundNode != null)
                    {
                        return foundNode;
                    }

                }
            }

            return foundNode;
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
            nodesSubscription?.Dispose();
        }
    }
}
