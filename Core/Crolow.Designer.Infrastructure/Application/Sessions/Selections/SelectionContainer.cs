
using Crolow.Designer.Common.Data;

namespace Crolow.Designer.Runtime.Application.Sessions.Selections
{
    public record SelectionContainer(IDataObject Node)
    {
        public IDataObject Parent { get; set; }
        public List<IDataObject> Children { get; set; } = new List<IDataObject>();
    }
}
