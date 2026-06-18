using Crolow.Designer.Abstractions;
using Crolow.Designer.Core.Document;
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
        public SelectionRegistry Selections { get; set; }

        private readonly IDisposable documentSubscription;
        public DocumentsController(RuntimeController runtimeController)
        {
            RuntimeController = runtimeController;
            Controller = this;
            Selections = new SelectionRegistry();

            documentSubscription = RuntimeController.Runtime.Events
                .Subscribe<DocumentEvent>(GuidSources.Documents.GenerateGuid(), OnDocumentEvent);
        }
        private Task OnDocumentEvent(DocumentEvent e)
        {
            switch (e.EventAction)
            {
                case EventAction.ObjectCreated:
                    Console.WriteLine("We need a new Document");
                    break;

                case EventAction.ObjectDeleted:
                    Console.WriteLine("We are closing a document");
                    break;

                case EventAction.ObjectUpdated:
                    Console.WriteLine("We are updating a document");
                    break;
            }

            return Task.CompletedTask;
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
