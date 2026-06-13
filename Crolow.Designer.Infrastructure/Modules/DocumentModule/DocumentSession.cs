using Crolow.Designer.Core.Document;
using Crolow.Designer.Runtime.Application.Sessions.Selections;

namespace Crolow.Designer.Runtime.Modules.DocumentModule
{
    public class DocumentSession
    {
        public DesignDocument Document { get; set; }

        public SelectionRegistry Selections { get; set; } = new SelectionRegistry();
    }
}
