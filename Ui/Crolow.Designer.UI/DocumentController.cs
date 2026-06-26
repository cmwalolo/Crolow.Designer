using Crolow.Designer.Common.Constants;
using Crolow.Designer.Core.Scene.Nodes;
using Crolow.Designer.Runtime.Application;
using Crolow.Designer.Runtime.Application.Sessions.Selections;
using Crolow.Designer.Runtime.Modules.DocumentModule;
using Crolow.Designer.Runtime.Modules.DocumentModule.Commands.Documents.Events;
using Crolow.Designer.Runtime.Modules.DocumentModule.Commands.GroupNodes.Events;
using Crolow.Designer.UI.Enumerations;
using Crolow.Designer.UI.Utils;

namespace Crolow.Designer.UI
{
    public class DocumentController
    {
        protected DesignerRuntime runtime;
        protected DocumentsController manager;
        private readonly IDisposable documentSubscription;

        public DocumentSession Session { get; set; }
        public SelectionRegistry Selections { get; set; } = new SelectionRegistry();
        public PageNode ActivePage { get; set; }
        public LayerNode? ActiveLayer { get; set; }
        public SceneNode? ActiveNode { get; set; }
        public ToolboxTool CurrentToolboxTool { get; set; }

        public DocumentController(DocumentSession session)
        {
            this.Session = session;
            this.ActivePage = session.Document.Pages.FirstOrDefault();
            this.ActiveLayer = this.ActivePage.Children.FirstOrDefault() as LayerNode;
            this.ActiveNode = this.ActiveLayer;

            documentSubscription = RuntimeController.Runtime.Events
                .Subscribe<SceneNodeEvent>(this.Session.Document.Id, OnSceneNodeEvent);
        }

        private async Task OnSceneNodeEvent(SceneNodeEvent e)
        {
            switch (e.EventAction)
            {
                case EventAction.ObjectCreated:
                    if (e.Target.Any())
                    {
                        foreach (var doc in e.Target)
                        {
                            ActiveNode = e.Target.FirstOrDefault();
                        }
                        await runtime.Events.PublishAsync(Session.Document.Id, new NodeActivatedEvent(this, true, ActiveNode));
                    }
                    break;

                case EventAction.ObjectDeleted:
                    Console.WriteLine("We are closing a document");
                    break;

                case EventAction.ObjectUpdated:
                    Console.WriteLine("We are updating a document");
                    break;
            }
        }

        public void CreateRectangle(DesignDocumentSettings settings)
        {
            GroupNode pNode = ActiveNode is GroupNode ? ActiveNode as GroupNode : ActiveLayer;
            Session.CreateRectangle(pNode, settings.CurrentSelectionArea);
        }
    }
}
