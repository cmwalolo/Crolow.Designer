namespace Crolow.Designer.Core.Variables;

public sealed class SceneVariable
{
    public string Name { get; set; } = "";

    public Type VariableType { get; set; } = typeof(string);

    public object? DefaultValue { get; set; }
}
