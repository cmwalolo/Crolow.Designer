using Crolow.Designer.Core.Geometry;
using Crolow.Designer.Core.Scene.Nodes;
using Crolow.Designer.Core.Transforms;

namespace Crolow.Designer.Graphics.Core.BaseProviders
{
    public class SceneNodeProvider
    {
        public void BuildPath(SceneNode node)
        {

        }

        public void ApplyTransform(SceneNode node, TransformContent transformation, bool doNotUseContextCenter)
        {
            float scaleX = transformation.Scale.X;
            float scaleY = transformation.Scale.Y;
            float newWidth = node.Canvas.Width * scaleX;
            float newHeight = node.Canvas.Height * scaleY;

            if (doNotUseContextCenter)
            {
                float centerX = node.Canvas.X + node.Canvas.Width * 0.5f;
                float centerY = node.Canvas.Y + node.Canvas.Height * 0.5f;

                node.Canvas = new Rect2D(
                    centerX - newWidth * 0.5f,
                    centerY - newHeight * 0.5f,
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

        public Rect2D GetBounds(SceneNode node)
        {
            return new Rect2D();
        }

        public bool HitTest(SceneNode node, Point2D point)
        {
            return true;
        }

        public void Render(SceneNode node)
        {

        }
    }
}
