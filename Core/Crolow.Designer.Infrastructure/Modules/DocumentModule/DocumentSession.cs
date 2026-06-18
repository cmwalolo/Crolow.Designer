using Crolow.Designer.Core.Document;
using Crolow.Designer.Runtime.Application;

namespace Crolow.Designer.Runtime.Modules.DocumentModule
{
    public class DocumentSession
    {
        public DocumentSessionManager Manager { get; }
        public DesignerRuntime Runtime { get; }

        public DesignDocument Document { get; }

        public DocumentSession(DocumentSessionManager manager, DesignerRuntime runtime, DesignDocument document)
        {
            Manager = manager;
            Runtime = runtime;
            Document = document;
        }
    }
}
