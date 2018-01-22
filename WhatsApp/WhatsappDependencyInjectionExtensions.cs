using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using System;
using WhatsApp;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class WhatsappDependencyInjectionExtensions
    {
        public static IServiceCollection AddWhatsApp<T>(this IServiceCollection services, Action<WhatsAppOptions> options = null)
            where T: IWhatsAppEventExecutor
        {

            services.AddOptions();
            if (options != null)
            {
                services.Configure(options);
            }
            
            services.TryAddSingleton<IHostedService, WhatsAppHostedService>();
            services.TryAddSingleton<IWhatsAppConnector, WhatsAppConnector>();
            services.TryAddSingleton(typeof(IWhatsAppEventExecutor), typeof(T));

            return services;
        }

      
    }
}