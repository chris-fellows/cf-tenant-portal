using System.Reflection;

namespace CFTenantPortal.Extensions
{
    public static class ServiceExtensions
    {
        public static void RegisterAllTypes<T>(this IServiceCollection services, IEnumerable<Assembly> assemblies, ServiceLifetime lifetime = ServiceLifetime.Transient)
        {
            var typesFromAssemblies = assemblies.SelectMany(a => a.DefinedTypes.Where(x => x.GetInterfaces().Contains(typeof(T))));
            foreach (var type in typesFromAssemblies)
            {                
                services.Add(new ServiceDescriptor(typeof(T), type, lifetime));
            }
        }
    }
}
