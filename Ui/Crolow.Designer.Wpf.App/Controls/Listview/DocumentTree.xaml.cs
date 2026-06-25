using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Input;

namespace Crolow.Designer.Wpf.App.Controls.Listview
{
    public partial class DocumentTree : UserControl
    {
        private readonly List<TreeNode> selectedNodes = new();
        private TreeNode? selectionParent;
        private TreeNode? selectionAnchor;

        public bool PropagateSelection { get; set; }

        public ObservableCollection<TreeNode> Nodes { get; }
            = new();

        public DocumentTree()
        {
            InitializeComponent();
        }

        private void AddNode(TreeNode node, Panel parent)
        {
            parent.Children.Add(new DocumentTreeNode(this, node, 1));
        }

        public void Refresh()
        {
            Root.Children.Clear();
            foreach (TreeNode node in Nodes)
            {
                Root.Children.Add(
                    new DocumentTreeNode(this, node, 1));
            }
        }

        public void Clear()
        {
            Root.Children.Clear();
        }

        private void ClearSelection()
        {
            foreach (TreeNode node in selectedNodes)
                node.IsSelected = false;

            selectedNodes.Clear();
        }

        public void Select(TreeNode node, ModifierKeys modifiers)
        {
            if ((modifiers & ModifierKeys.Control) != 0)
                SelectCtrl(node);
            else if ((modifiers & ModifierKeys.Shift) != 0)
                SelectShift(node);
            else
                SelectSingle(node);
        }

        private void SelectSingle(TreeNode node)
        {
            ClearSelection();
            node.IsSelected = true;
            selectedNodes.Add(node);
            selectionParent = node.Parent;
            selectionAnchor = node;
        }

        private void SelectCtrl(TreeNode node)
        {
            if (selectionParent != node.Parent)
            {
                SelectSingle(node);
                return;
            }

            if (node.IsSelected)
            {
                node.IsSelected = false;
                selectedNodes.Remove(node);
            }
            else
            {
                node.IsSelected = true;
                selectedNodes.Add(node);
            }

            selectionAnchor = node;
        }

        private void SelectShift(TreeNode node)
        {
            if (selectionAnchor == null)
            {
                SelectSingle(node);
                return;
            }

            if (selectionAnchor.Parent != node.Parent)
            {
                SelectSingle(node);
                return;
            }

            ClearSelection();

            var siblings = node.Parent.Children;

            int first = siblings.IndexOf(selectionAnchor);
            int last = siblings.IndexOf(node);

            if (first > last)
                (first, last) = (last, first);

            for (int i = first; i <= last; i++)
            {
                siblings[i].IsSelected = true;
                selectedNodes.Add(siblings[i]);
            }
        }
    }
}