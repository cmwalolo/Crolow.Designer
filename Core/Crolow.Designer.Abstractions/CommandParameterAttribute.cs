namespace Crolow.Designer.Abstractions
{
    public class CommandParameterAttribute : Attribute
    {
        public CommandParameterAttribute(Type type)
        {
            Type = type;
        }
        public Type Type { get; }
    }
}
