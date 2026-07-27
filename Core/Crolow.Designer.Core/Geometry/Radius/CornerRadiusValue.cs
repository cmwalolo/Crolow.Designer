namespace Crolow.Designer.Core.Geometry.Radius
{
    public struct CornerRadiusValue
    {
        public CornerRadiusValue(float value, CornerRadiusUnit unit)
        {
            Value = value;
            Unit = unit;
        }

        public float Value { get; }

        public CornerRadiusUnit Unit { get; }

        public bool IsPercentage => Unit == CornerRadiusUnit.Percentage;
    }
}
