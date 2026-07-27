using Crolow.Designer.Core.Geometry;
using Crolow.Designer.Core.Scene.Nodes;
using Crolow.Designer.Core.Transforms;
using Crolow.Designer.Graphics.Core.Extensions;

namespace Crolow.Designer.Graphics.Core.Providers.BaseProviders
{
    public interface ISceneNodeProvider
    {
        SceneNode Create(SceneNode parentNode, Rect2D bounds);
        void ApplyTransform(SceneNode node, TransformContent transformation, bool doNotUseContextCenter);
        void BuildPath(SceneNode node);
        Rect2D GetBounds(SceneNode node);
        bool HitTest(SceneNode node, Point2D point);
        bool IsInBounds(SceneNode node, Rect2D selection);
        void Render(SceneNode node);
    }

    public abstract class SceneNodeProvider
    {
        public virtual SceneNode Create(SceneNode parentNode, Rect2D bounds)
        {
            return null;
        }

        public virtual void BuildPath(SceneNode node)
        {

        }

        public virtual void ApplyTransform(SceneNode node, TransformContent transformation, bool doNotUseContextCenter)
        {
            float scaleX = transformation.Scale.X;
            float scaleY = transformation.Scale.Y;
            float newWidth = node.Canvas.Width * scaleX;
            float newHeight = node.Canvas.Height * scaleY;

            if (doNotUseContextCenter)
            {
                var center = node.Canvas.Center();
                center.X += transformation.Offset.X;
                center.Y += transformation.Offset.Y;

                node.Canvas = new Rect2D(
                    center.X - newWidth * 0.5f,
                    center.Y - newHeight * 0.5f,
                    newWidth,
                    newHeight);

                node.Rotation = transformation.Rotation;
            }
            else
            {

                float angle = MathF.PI * transformation.Rotation / 180.0f;
                float cos = MathF.Cos(angle);
                float sin = MathF.Sin(angle);

                float width = node.Canvas.Width;
                float height = node.Canvas.Height;

                float objectCenterX = node.Canvas.X + width * 0.5f;
                float objectCenterY = node.Canvas.Y + height * 0.5f;

                float dx = objectCenterX - transformation.InitCenter.X;
                float dy = objectCenterY - transformation.InitCenter.Y;

                // Scale
                dx *= scaleX;
                dy *= scaleY;

                // Rotate around selection center
                float rx = dx * cos - dy * sin;
                float ry = dx * sin + dy * cos;

                float newCenterX = transformation.Center.X + rx;
                float newCenterY = transformation.Center.Y + ry;

                node.Canvas = new Rect2D(
                    newCenterX - newWidth * 0.5f,
                    newCenterY - newHeight * 0.5f,
                    newWidth,
                    newHeight);

                node.Rotation += transformation.Rotation;
            }
        }

        public virtual Rect2D GetBounds(SceneNode node)
        {
            return node.Canvas;
        }

        /// <summary>
        /// This will be replaced with a bitmap created on each canvas invalidation
        /// </summary>
        /// <param name="node"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public virtual bool HitTest(SceneNode node, Point2D point)
        {
            return (point.X >= node.Canvas.X && point.X <= node.Canvas.Right
                && point.Y >= node.Canvas.Y && point.Y <= node.Canvas.Bottom);
        }
        public virtual bool IsInBounds(SceneNode node, Rect2D selection)
        {
            return (selection.X <= node.Canvas.X && selection.Right >= node.Canvas.Right
                && selection.Y <= node.Canvas.Y && selection.Bottom >= node.Canvas.Bottom);
        }


        public virtual void Render(SceneNode node)
        {

        }
    }
}
