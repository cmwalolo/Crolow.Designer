using Crolow.Designer.Core.Scene.Nodes;

namespace Crolow.Designer.Core.Extensions
{
    public static class SceneNodeExtensions
    {
        public static void ApplyParents(this GroupNode node)
        {
            int position = 1;
            foreach (var child in node.Children)
            {
                child.Position = position++;
                child.ParentNode = node;
                child.ParentId = node.Id;
                if (child is GroupNode groupNode)
                {
                    groupNode.ApplyParents();
                }
            }
        }

        public static void ApplyParents(this List<GroupNode> nodes)
        {
            foreach (var child in nodes)
            {
                if (child is GroupNode groupNode)
                {
                    groupNode.ApplyParents();
                }
            }
        }

        public static void ApplyParents(this List<PageNode> nodes)
        {
            foreach (var child in nodes)
            {
                if (child is GroupNode groupNode)
                {
                    groupNode.ApplyParents();
                }
            }
        }

    }
}
