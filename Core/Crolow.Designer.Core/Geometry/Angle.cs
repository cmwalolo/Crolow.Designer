namespace Crolow.Designer.Core.Geometry
{
    public readonly struct Angle
    {
        public Angle(float radians)
        {
            Radians = radians;
        }
        public float Radians { get; }

        public static Angle FromDegrees(float degrees)
            => new(degrees * MathF.PI / 180f);

        public static Angle FromRadians(float radians)
            => new(radians);

        public float Degrees => Radians * 180f / MathF.PI;
    }
}
