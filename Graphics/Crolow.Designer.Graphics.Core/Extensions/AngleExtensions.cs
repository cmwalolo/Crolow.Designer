namespace Crolow.Designer.Graphics.Core.Extensions
{
    public static class AngleExtensions
    {
        extension(float angleRadians)
        {
            public float BezierCoefficient()
            {
                return 4f / 3f * MathF.Tan(angleRadians / 4f);
            }
        }
    }
}
