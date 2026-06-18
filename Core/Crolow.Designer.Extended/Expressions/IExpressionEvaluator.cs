using Crolow.Designer.Core.Bindings;

namespace Crolow.Designer.Core.Expressions;

public interface IExpressionEvaluator
{
    object? Evaluate(
        string expression,
        BindingContext context);
}
