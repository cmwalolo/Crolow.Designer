using Crolow.Designer.Common.Constants;
using Crolow.Designer.Common.Extensions;
using Crolow.Designer.Core.Document;
using Crolow.Designer.Runtime.Application;
using Crolow.Designer.Runtime.Application.Sessions.Selections;
using Crolow.Designer.Runtime.Modules.DocumentModule.Commands.Documents.Events;

namespace Crolow.Designer.UI
{
    public class DocumentsController : IDisposable
    {
        public static DocumentsController Controller { get; set; }
        public DocumentController ActiveDocument { get; set; }
        public List<DocumentController> OpenDocuments { get; set; } = new List<DocumentController>();
        public RuntimeController RuntimeController { get; set; }

        protected DesignerRuntime runtime { get; set; }
        public SelectionRegistry Selections { get; set; }

        private readonly IDisposable documentSubscription;

        public DocumentsController(RuntimeController runtimeController)
        {
            RuntimeController = runtimeController;
            Controller = this;
            Selections = new SelectionRegistry();

            runtime = RuntimeController.Runtime;
            documentSubscription = RuntimeController.Runtime.Events
                .Subscribe<DocumentEvent>(GuidSources.Documents.GenerateGuid(), OnDocumentEvent);
        }
        private async Task OnDocumentEvent(DocumentEvent e)
        {
            switch (e.EventAction)
            {
                case EventAction.ObjectCreated:
                    if (e.Target.Any())
                    {
                        foreach (var doc in e.Target)
                        {
                            ActiveDocument = new DocumentController(doc);
                            OpenDocuments.Add(ActiveDocument);
                        }
                        await runtime.Events.PublishAsync(GuidSources.Documents.GenerateGuid(), new DocumentActivatedEvent(this, true, ActiveDocument));
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

        public async void NewDocument(DesignDocument document)
        {
            var result = await RuntimeController.Runtime.Documents.CreateDocument(document);
        }

        public void Dispose()
        {
            documentSubscription.Dispose();
        }
    }
}
