using Crolow.Designer.Core.Document;
using Crolow.Designer.Core.Scene.Nodes.Objects;
using Crolow.Designer.Runtime.Commands;

namespace Crolow.Designer.Runtime.Application.Commands.Requests;

public sealed record CreateRectangleCommand(
    Layer Layer,
    string Name)
    : ICommand<RectangleShape>;
