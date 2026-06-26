using Crolow.Designer.Core.Document;
using Crolow.Designer.Core.Geometry;
using Crolow.Designer.Core.Scene.Nodes;
using System.Numerics;

namespace Crolow.Designer.Core.Extensions
{
    public static class SceneNodeExtensions
    {
        public static Rect2D AddSelection(this Rect2D sourceRect, Rect2D rect, Matrix3x2 transform = new())
        {
            Rect2D bounds = GetBoundingRect(rect, transform);

            // Premier rectangle
            if (sourceRect.Width == 0 && sourceRect.Height == 0)
                return bounds;

            float left = MathF.Min(sourceRect.X, bounds.X);
            float top = MathF.Min(sourceRect.Y, bounds.Y);
            float right = MathF.Max(sourceRect.X + sourceRect.Width, bounds.X + bounds.Width);
            float bottom = MathF.Max(sourceRect.Y + sourceRect.Height, bounds.Y + bounds.Height);

            return new Rect2D(
                left,
                top,
                right - left,
                bottom - top);
        }

        private static Rect2D GetBoundingRect(Rect2D rect, Matrix3x2 transform)
        {
            Vector2 p1 = Vector2.Transform(new Vector2(rect.X, rect.Y), transform);
            Vector2 p2 = Vector2.Transform(new Vector2(rect.X + rect.Width, rect.Y), transform);
            Vector2 p3 = Vector2.Transform(new Vector2(rect.X + rect.Width, rect.Y + rect.Height), transform);
            Vector2 p4 = Vector2.Transform(new Vector2(rect.X, rect.Y + rect.Height), transform);

            float minX = MathF.Min(MathF.Min(p1.X, p2.X), MathF.Min(p3.X, p4.X));
            float minY = MathF.Min(MathF.Min(p1.Y, p2.Y), MathF.Min(p3.Y, p4.Y));
            float maxX = MathF.Max(MathF.Max(p1.X, p2.X), MathF.Max(p3.X, p4.X));
            float maxY = MathF.Max(MathF.Max(p1.Y, p2.Y), MathF.Max(p3.Y, p4.Y));

            return new Rect2D(
                minX,
                minY,
                maxX - minX,
                maxY - minY);
        }


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

        public static void ApplyParents(this DesignDocument document)
        {
            int position = 1;
            foreach (var child in document.Pages)
            {
                child.Position = position++;
                child.ParentNode = document;
                child.ParentId = document.Id;

                if (child is GroupNode groupNode)
                {
                    groupNode.ApplyParents();
                }
            }
        }

    }
}
