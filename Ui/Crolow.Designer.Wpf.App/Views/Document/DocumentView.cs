using AvalonDock.Layout;
using Crolow.Designer.UI;
using Crolow.Designer.Wpf.App.Views.Document;

public class DocumentView
{
    public LayoutDocument LayoutDocument { get; }
    public DocumentCanvas Canvas { get; }

    public DocumentView(DocumentController controller)
    {
        Canvas = new DocumentCanvas(controller);

        LayoutDocument = new LayoutDocument
        {
            ContentId = controller.Session.Document.Id.ToString(),
            Title = controller.Session.Document.Name,
            Content = Canvas
        };
    }
}