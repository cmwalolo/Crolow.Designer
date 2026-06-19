using Crolow.Designer.Common.Constants;

namespace Crolow.Designer.Common.Extensions;

public static class GuidExtensions
{
    public static Guid GenerateGuid(this GuidSources source)
    {
        return new Guid((uint)source, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
    }
}
