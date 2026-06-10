namespace Crolow.Designer.Abstractions;

public sealed class SelectionState<T>
{
    private readonly HashSet<T> _items = [];

    public IReadOnlyCollection<T> Items => _items;

    public bool Select(T item)
        => _items.Add(item);

    public bool Unselect(T item)
        => _items.Remove(item);

    public void Clear()
        => _items.Clear();
}