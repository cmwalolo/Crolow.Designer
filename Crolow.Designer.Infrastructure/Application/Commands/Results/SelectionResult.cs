using Crolow.Designer.Abstractions;

namespace Crolow.Designer.Runtime.Application.Commands.Results;
#region Runtime

#endregion


public sealed class SelectionResult<T>
{
    public required ISelectionOwner<T> Owner
    {
        get;
        init;
    }

    public required IReadOnlyCollection<T> Items
    {
        get;
        init;
    }
}
