using Crolow.Designer.Core.Document;
using Crolow.Designer.Core.Geometry;
using Crolow.Designer.Core.Scene.Nodes;
using Crolow.Designer.Core.Scene.Nodes.Objects;
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

        public async Task CreateRectangle(GroupNode activeNode, Rect2D currentSelectionArea)
        {
            var rectangle = new RectangleShape
            {
                Name = "new Rectangle",
                ParentId = activeNode.Id,
                ParentNode = activeNode,
                Canvas = currentSelectionArea
            };

            var command = new CreateSceneNodeCommand(Document.Id, activeNode, rectangle);
            var result = await Runtime.Commands.ExecuteAsync(command);

        }
    }
}
