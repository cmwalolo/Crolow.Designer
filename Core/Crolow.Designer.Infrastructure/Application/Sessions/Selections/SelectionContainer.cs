
using Crolow.Designer.Common.Data;

namespace Crolow.Designer.Runtime.Application.Sessions.Selections
{
    public record SelectionContainer()
    {
        public Dictionary<Guid, IDataObject> Objects { get; set; } = new();
    }
}
