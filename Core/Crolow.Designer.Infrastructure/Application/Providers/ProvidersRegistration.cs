using Crolow.Designer.Graphics.Core.Attributes;
using Crolow.Designer.Graphics.Core.Providers.BaseProviders;
using System.Reflection;

namespace Crolow.Designer.Runtime.Application.Commands;

public sealed class ProvidersRegistration
{
    public readonly Dictionary<Type, ISceneNodeProvider> shapeProviders = [];

    private readonly DesignerRuntime runtime;

    public ProvidersRegistration(DesignerRuntime runtime)
    {
        this.runtime = runtime;
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        RegisterShapeProviders(assemblies);
    }

    public ISceneNodeProvider GetProvider(Type shapeType)
    {
        return shapeProviders[shapeType];
    }

    #region Private 
    private void RegisterShapeProviders(Assembly[] assemblies)
    {
        foreach (var assembly in assemblies)
        {
            var results = RegisterAssemblyByAttribute<ShapeProviderMappingAttribute>(assembly);
            foreach (var result in results)
            {
                var instance = Activator.CreateInstance(result.Value) as ISceneNodeProvider;
                shapeProviders.Add(result.Key.MappedType, instance);
            }
        }
    }

    private Dictionary<T, Type> RegisterAssemblyByAttribute<T>(Assembly assembly) where T : Attribute
    {
        Dictionary<T, Type> results = new();

        foreach (var type in assembly.GetTypes())
        {
            var att = type.GetCustomAttribute<T>();
            if (att != null)
            {
                results.Add(att, type);
            }
        }

        return results;
    }
    #endregion 
}
