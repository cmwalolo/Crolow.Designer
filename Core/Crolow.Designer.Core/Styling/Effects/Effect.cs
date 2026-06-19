using Crolow.Designer.Common.Data;

namespace Crolow.Designer.Core.Styling.Effects;

public abstract class Effect : DataObject
{
    public bool Enabled { get; set; } = true;
}
