using Crolow.Designer.Core.Geometry;
using Crolow.Designer.Core.Geometry.Paths;
using Crolow.Designer.Core.Geometry.Paths.Definitions;
using Crolow.Designer.Core.Geometry.Radius;
using Crolow.Designer.Core.Scene.Nodes;
using Crolow.Designer.Core.Scene.Nodes.Objects;
using Crolow.Designer.Core.Transforms;
using Crolow.Designer.Graphics.Core.Attributes;
using Crolow.Designer.Graphics.Core.Extensions;
using Crolow.Designer.Graphics.Core.Providers.BaseProviders;

namespace Crolow.Designer.Graphics.Core.BaseProviders
{

    [ShapeProviderMapping(typeof(RectangleShape))]
    public class RecangleShapeProvider : SceneNodeProvider, ISceneNodeProvider
    {
        public override RectangleShape Create(SceneNode parentNode, Rect2D bounds)
        {
            return new RectangleShape
            {
                Name = "Rectangle",
                ParentId = parentNode.Id,
                ParentNode = parentNode,
                Canvas = bounds
            };
        }
        public override void BuildPath(SceneNode node)
        {
            RectangleShape shape = node as RectangleShape;

            if (shape.UseDefaultCornerRadiusValue && shape.DefaultCornerRadiusValue.Value == 0)
            {
                BuildSimplePath(shape);
            }
            else
            {
                BuildPathWithRoundedCorners(shape);
            }
        }

        private void BuildPathWithRoundedCorners(RectangleShape shape)
        {
            Rect2D rect = shape.Canvas;

            CornerRadiusValue value = shape.UseDefaultCornerRadiusValue
                ? shape.DefaultCornerRadiusValue
                : shape.CornerRadiusValues[0];

            float rx;
            float ry;

            if (value.Unit == CornerRadiusUnit.Percentage)
            {
                rx = rect.Width * value.Value / 100f;
                ry = rect.Height * value.Value / 100f;
            }
            else
            {
                rx = value.Value;
                ry = value.Value;
            }

            // Clamp
            rx = MathF.Min(rx, rect.Width * 0.5f);
            ry = MathF.Min(ry, rect.Height * 0.5f);

            // Coefficient used to position the control points of a cubic Bézier so that it
            // accurately approximates a circular arc for the specified angle.
            // For a 90° corner, this evaluates to approximately 0.55228475.
            float k = (MathF.PI / 2f).BezierCoefficient();  // should be 0.552284749831f;

            PathFigure figure = new()
            {
                StartPoint = new Point2D(rect.X + rx, rect.Y),
                Closed = true
            };

            // ----- Top -----

            figure.Segments.Add(
                new LineSegment(
                    new Point2D(rect.Right - rx, rect.Y)));

            // Top Right

            figure.Segments.Add(
                new CubicBezierSegment(
                    new Point2D(rect.Right - rx + rx * k, rect.Y),
                    new Point2D(rect.Right, rect.Y + ry - ry * k),
                    new Point2D(rect.Right, rect.Y + ry)));

            // ----- Right -----

            figure.Segments.Add(
                new LineSegment(
                    new Point2D(rect.Right, rect.Bottom - ry)));

            // Bottom Right

            figure.Segments.Add(
                new CubicBezierSegment(
                    new Point2D(rect.Right, rect.Bottom - ry + ry * k),
                    new Point2D(rect.Right - rx + rx * k, rect.Bottom),
                    new Point2D(rect.Right - rx, rect.Bottom)));

            // ----- Bottom -----

            figure.Segments.Add(
                new LineSegment(
                    new Point2D(rect.X + rx, rect.Bottom)));

            // Bottom Left

            figure.Segments.Add(
                new CubicBezierSegment(
                    new Point2D(rect.X + rx - rx * k, rect.Bottom),
                    new Point2D(rect.X, rect.Bottom - ry + ry * k),
                    new Point2D(rect.X, rect.Bottom - ry)));

            // ----- Left -----

            figure.Segments.Add(
                new LineSegment(
                    new Point2D(rect.X, rect.Y + ry)));

            // Top Left

            figure.Segments.Add(
                new CubicBezierSegment(
                    new Point2D(rect.X, rect.Y + ry - ry * k),
                    new Point2D(rect.X + rx - rx * k, rect.Y),
                    new Point2D(rect.X + rx, rect.Y)));

            shape.BasicPath = new PathGeometry();
            shape.BasicPath.Figures.Add(figure);
        }

        private void BuildSimplePath(RectangleShape shape)
        {
            Rect2D rect = shape.Canvas;
            PathFigure figure = new()
            {
                StartPoint = new Point2D(rect.X, rect.Y),
                Closed = true
            };

            figure.Segments.Add(
                new LineSegment(new Point2D(rect.Right, rect.Y)));

            figure.Segments.Add(
                new LineSegment(new Point2D(rect.Right, rect.Bottom)));

            figure.Segments.Add(
                new LineSegment(new Point2D(rect.X, rect.Bottom)));

            shape.BasicPath = new PathGeometry();
            shape.BasicPath.Figures.Add(figure);

        }
        public void ApplyTransform(SceneNode node, TransformContent transformation, bool doNotUseContextCenter)
        {
            base.ApplyTransform(node, transformation, doNotUseContextCenter);
        }

        public Rect2D GetBounds(SceneNode node)
        {
            return this.GetBounds(node);
        }

        public bool IsInBounds(SceneNode node, Rect2D selection)
        {
            return base.IsInBounds(node, selection);
        }

        public bool HitTest(SceneNode node, Point2D point)
        {
            return base.HitTest(node, point);
        }

        public void Render(SceneNode node)
        {
            base.Render(node);
        }
    }
}
