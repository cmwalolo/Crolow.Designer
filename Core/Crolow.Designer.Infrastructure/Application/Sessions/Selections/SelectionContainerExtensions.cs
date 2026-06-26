
using Crolow.Designer.Common.Data;

namespace Crolow.Designer.Runtime.Application.Sessions.Selections
{

    public static class SelectionContainerExtensions
    {
        public static void Clear(this SelectionContainer container)
        {
            container.Objects.Clear();
        }

        public static void Set(this SelectionContainer container, IDataObject child)
        {
            container.Objects.Clear();
            container.Objects.TryAdd(child.Id, child);
        }

        public static void Set(this SelectionContainer container, IEnumerable<IDataObject> children)
        {
            container.Objects.Clear();
            container.Add(children);
        }

        public static void Add(this SelectionContainer container, IDataObject child)
        {
            container.Objects.TryAdd(child.Id, child);
        }

        public static void Add(this SelectionContainer container, IEnumerable<IDataObject> children)
        {
            foreach (var i in children)
                container.Objects.TryAdd(i.Id, i);
        }

        public static void Remove(this SelectionContainer container, IDataObject child)
        {
            if (container.IsSelected(child))
            {
                container.Objects.Remove(child.Id);
            }
        }

        public static bool IsSelected(this SelectionContainer container, IDataObject child)
        {
            return container.Objects.ContainsKey(child.Id);
        }
    }
}
