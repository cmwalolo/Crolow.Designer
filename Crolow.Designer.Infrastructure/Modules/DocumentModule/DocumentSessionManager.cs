using Crolow.Designer.Runtime.Application.Sessions.Selections;
using Crolow.Designer.Runtime.Modules.DocumentModule;

public sealed class DocumentSessionManager
{
    public IList<DocumentSession> Documents { get; } = new List<DocumentSession>();
    public SelectionRegistry Selections { get; set; } = new SelectionRegistry();

}