using Crolow.Designer.Runtime.Application;
using Crolow.Designer.Runtime.Application.Sessions.Selections;
using Crolow.Designer.Runtime.Modules.DocumentModule;

namespace Crolow.Designer.UI
{
    public class DocumentController
    {
        protected DesignerRuntime runtime;
        protected DocumentsController manager;
        protected DocumentSession session;
        public SelectionRegistry Selections { get; set; } = new SelectionRegistry();

        public DocumentController(DocumentSession session)
        {
            this.session = session;
        }
    }
}
