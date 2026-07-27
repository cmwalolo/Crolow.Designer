using Crolow.Designer.Core.Document;
using Crolow.Designer.Core.Geometry;
using Crolow.Designer.Core.Scene.Nodes;
using Crolow.Designer.Runtime.Application;
using Crolow.Designer.Runtime.Modules.DocumentModule.Commands.Shapes.Requests;

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

        public async Task CreateSceneNode(GroupNode activeNode, Rect2D currentSelectionArea, Type targetType)
        {
            var provider = Runtime.Providers.GetProvider(targetType);
            var shape = provider.Create(activeNode, currentSelectionArea);
            var command = new CreateSceneNodeCommand(Document.Id, activeNode, shape);
            var result = await Runtime.Commands.ExecuteAsync(command);

        }
    }
}
