using Crolow.Designer.Core.Scene.Nodes;
using Crolow.Designer.Runtime.Application;
using Crolow.Designer.Runtime.Application.Sessions.Selections;
using Crolow.Designer.Runtime.Modules.DocumentModule;

namespace Crolow.Designer.UI
{
    public class DocumentController
    {
        protected DesignerRuntime runtime;
        protected DocumentsController manager;

        public DocumentSession Session { get; set; }
        public SelectionRegistry Selections { get; set; } = new SelectionRegistry();
        public PageNode ActivePage { get; set; }
        public GroupNode ActiveGroup { get; set; }

        public DocumentController(DocumentSession session)
        {
            this.Session = session;
            this.ActivePage = session.Document.Pages.FirstOrDefault();
        }
    }
}
