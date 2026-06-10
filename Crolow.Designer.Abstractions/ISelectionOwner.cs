namespace Crolow.Designer.Abstractions;

public interface ISelectionOwner<T>
{
    SelectionState<T> Selection { get; }
}