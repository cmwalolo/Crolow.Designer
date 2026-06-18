namespace Crolow.Designer.Runtime.Modules.DocumentModule.Commands.Documents.Events;

public static class GuidExtensions
{
    public static Guid GenerateGuid(this GuidSources source)
    {
        return new Guid((uint)source, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
    }
}
