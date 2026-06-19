using Crolow.Designer.Core.Scene.Nodes;

namespace Crolow.Designer.Core.Extensions
{
    public static class SceneNodeExtensions
    {
        public static void ApplyParents(this GroupNode node)
        {
            foreach (var child in node.Children)
            {
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

        public static void ApplyParents(this List<LayerNode> nodes)
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
