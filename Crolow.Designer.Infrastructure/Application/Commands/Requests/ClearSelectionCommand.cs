using Crolow.Designer.Abstractions;
using Crolow.Designer.Runtime.Application.Commands.Results;
using Crolow.Designer.Runtime.Commands;

namespace Crolow.Designer.Runtime.Application.Commands.Requests;

public sealed record ClearSelectionCommand<T>(
    ISelectionOwner<T> Owner)
    : ICommand<SelectionResult<T>>;
