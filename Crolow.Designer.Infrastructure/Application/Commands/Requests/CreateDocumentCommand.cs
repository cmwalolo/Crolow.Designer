using Crolow.Designer.Core.Document;
using Crolow.Designer.Runtime.Commands;

namespace Crolow.Designer.Runtime.Application.Commands.Requests;
#region Results

#endregion


public sealed record CreateDocumentCommand(
    string Name)
    : ICommand<DesignDocument>;
