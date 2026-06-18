namespace Crolow.Designer.Core.Bindings;

public sealed class PropertyBinding
{
    public string PropertyPath { get; set; } = "";

    public string Expression { get; set; } = "";

    public bool Enabled { get; set; } = true;
}
