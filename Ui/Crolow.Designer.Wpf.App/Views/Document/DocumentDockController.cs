using AvalonDock.Layout;
using Crolow.Designer.Common.Constants;
using Crolow.Designer.Common.Extensions;
using Crolow.Designer.Runtime.Modules.DocumentModule.Commands.Documents.Events;
using Crolow.Designer.UI;
using Crolow.Designer.Wpf.App.Views.Document;

public sealed class DocumentDockController : IDisposable
{
    private IDisposable documentSubscription;
    private readonly LayoutDocumentPane layoutDocumentPane;

    public DocumentDockController(LayoutDocumentPane pane)
    {
        layoutDocumentPane = pane;
        documentSubscription = RuntimeController.Runtime.Events
            .Subscribe<DocumentActivatedEvent>(GuidSources.Documents.GenerateGuid(), OnActivateDocumentEvent);
    }

    private async Task OnActivateDocumentEvent(DocumentActivatedEvent docEvent)
    {
        switch (docEvent.EventAction)
        {
            case EventAction.ObjectActivated:
                foreach (var doc in docEvent.Target)
                {
                    if (docEvent.NewlyCreated)
                    {
                        var canvas = new DocumentTreeview();
                        var layoutDocument = new LayoutDocument
                        {
                            Title = doc.Session.Document.Name,
                            Content = canvas,
                            IsActive = true,
                            ContentId = doc.Session.Document.Id.ToString()
                        };
                        layoutDocumentPane.Children.Add(layoutDocument);
                    }
                    else
                    {
                        foreach (var p in layoutDocumentPane.Children.Where(p => p.ContentId == doc.Session.Document.Id.ToString()))
                        {
                            p.IsActive = true;
                        }
                    }
                }
                break;
        }
    }

    public void Dispose()
    {
        documentSubscription.Dispose();
    }
}
