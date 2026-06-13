using System.Runtime.InteropServices.ComTypes;

namespace Crolow.Designer.Runtime.Application.Sessions.Selections
{
    public record SelectionContainer(IDataObject Node)
    {
        public List<IDataObject> Children { get; set; } = new List<IDataObject>();
    }
}
