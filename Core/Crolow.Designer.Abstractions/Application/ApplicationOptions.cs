namespace Crolow.Designer.Common.Application;

public sealed class ApplicationOptions
{
    public string Name { get; set; } = string.Empty;

    public string Version { get; set; } = string.Empty;

    public List<DatabaseSettings> DatabaseSettings { get; set; } = new();
}
