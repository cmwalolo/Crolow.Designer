using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Crolow.Designer.Wpf.App.Controls.Listview
{
    /// <summary>
    /// Interaction logic for DocumentTreeNode.xaml
    /// </summary>
    public partial class DocumentTreeNode : UserControl, IDisposable
    {
        public TreeNode Node { get; }
        public DocumentTree Tree { get; set; }
        public int Level { get; set; }

        public DocumentTreeNode(DocumentTree tree, TreeNode node, int level)
        {
            InitializeComponent();
            Tree = tree;
            Node = node;
            Level = level;

            GridRow.MouseLeftButtonUp += GridRow_MouseLeftButtonUp;
            Node.PropertyChanged += Node_PropertyChanged;

            PanelRow.Margin = new Thickness(5 * (level - 1), 0, 0, 0);

            Caption.Text = node.Text;
            Chevron.Text = node.HasChildren ? (node.IsExpanded ? "▼" : "▶") : "";
            Chevron.MouseLeftButtonUp += Chevron_Click;

            VisibilityToggle.Background = node.IsVisible ? Brushes.SeaGreen : Brushes.DarkRed;

            if (node.IsExpanded)
            {
                foreach (TreeNode child in node.Children)
                {
                    ChildrenPanel.Children.Add(
                        new DocumentTreeNode(Tree, child, level + 1));
                }
            }
        }

        private void Node_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(TreeNode.IsSelected):
                    UpdateSelection();
                    break;
            }
        }

        private void UpdateSelection()
        {
            GridRow.Background = Node.IsSelected
                ? Brushes.DodgerBlue
                : Brushes.Transparent;
        }

        private void GridRow_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            Tree.Select(Node, Keyboard.Modifiers);
        }

        private void Chevron_Click(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            if (!Node.HasChildren)
                return;

            if (Node.IsExpanded)
                Collapse();
            else
                Expand();
        }

        private void Expand()
        {
            Node.IsExpanded = true;
            Chevron.Text = "▼";
            foreach (TreeNode child in Node.Children)
            {
                ChildrenPanel.Children.Add(
                    new DocumentTreeNode(Tree, child, Level + 1));
            }
            ChildrenPanel.Visibility = Visibility.Visible;
        }

        private void Collapse()
        {
            Node.IsExpanded = false;
            Chevron.Text = "▶";
            ChildrenPanel.Visibility = Visibility.Collapsed;
            ChildrenPanel.Children.Clear();
        }

        private void VisibilityToggle_Click(object sender, RoutedEventArgs e)
        {
            Node.IsVisible = !Node.IsVisible;
            VisibilityToggle.Background = Node.IsVisible ? Brushes.SeaGreen : Brushes.DarkRed;
            e.Handled = true;
        }

        private void EditNode_Click(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
        }

        public void Dispose()
        {
            Node.PropertyChanged -= Node_PropertyChanged;
        }
    }
}
