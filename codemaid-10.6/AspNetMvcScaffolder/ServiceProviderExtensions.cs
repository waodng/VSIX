using System;
namespace AspNetMvcScaffolder
{
    internal static class ServiceProviderExtensions
    {
        public static T GetService<T>(this IServiceProvider serviceProvider)
        {
            if (serviceProvider == null)
            {
                throw new ArgumentNullException("serviceProvider");
            }

            return (T)serviceProvider.GetService(typeof(T));
        }

        public static TInterface GetService<TInterface, TService>(this IServiceProvider serviceProvider)
            where TInterface : class
            where TService : class
        {
            if (serviceProvider == null)
            {
                throw new ArgumentNullException("serviceProvider");
            }

            return (TInterface)serviceProvider.GetService(typeof(TService));
        }
    }
}
