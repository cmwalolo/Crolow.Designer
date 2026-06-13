using System.Runtime.InteropServices.ComTypes;

namespace Crolow.Designer.Runtime.Application.Sessions.Selections
{

    public sealed class SelectionManager
    {
        public void Clear(SelectionContainer node)
        {
        }

        public void Set(SelectionContainer node, IDataObject child)
        {
        }

        public void Set(SelectionContainer node, IEnumerable<IDataObject> children)
        {
        }

        public void Add(SelectionContainer node, IDataObject child)
        {
        }

        public void Add(SelectionContainer node, IEnumerable<SelectionContainer> children)
        {
        }

        public void Remove(SelectionContainer node, SelectionContainer child)
        {
        }

        public bool IsSelected(SelectionContainer node)
        {
            return true;
        }
    }
}
