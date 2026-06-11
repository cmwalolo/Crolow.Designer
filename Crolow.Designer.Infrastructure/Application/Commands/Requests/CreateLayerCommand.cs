using Crolow.Designer.Core.Document;
using Crolow.Designer.Runtime.Commands;

namespace Crolow.Designer.Runtime.Application.Commands.Requests;

public sealed record CreateLayerCommand(
    DesignDocument Document,
    string Name)
    : ICommand<Layer>;
