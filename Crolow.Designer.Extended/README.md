# Designer.Core Extensions

This package extends the base model architecture with:

- Property bindings
- Expression evaluation
- Scene variables
- Timeline animation

## Example Binding

```csharp
new PropertyBinding
{
    PropertyPath = "Text",
    Expression = "Customer.Name"
};
```

## Example Animation

```csharp
new Track
{
    PropertyPath = "Transform.Position.X"
};
```
