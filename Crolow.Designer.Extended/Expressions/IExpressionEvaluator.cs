namespace Crolow.Designer.Core.Expressions;

public interface IExpressionEvaluator
{
    object? Evaluate(
        string expression,
        BindingContext context);
}
