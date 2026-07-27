namespace Crolow.Designer.Graphics.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ShapeProviderMappingAttribute : Attribute
    {
        public ShapeProviderMappingAttribute(Type mappedType)
        {
            MappedType = mappedType;
        }

        public Type MappedType { get; set; }
    }
}
