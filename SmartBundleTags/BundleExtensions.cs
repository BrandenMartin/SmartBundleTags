using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace SmartBundleTags.BundleTagHelpers {

    public static class BundleExtensions {

        public static IServiceCollection AddBundles(this IServiceCollection services) {
            return AddBundles(services, null);
        }

        public static IServiceCollection AddBundles(this IServiceCollection services, Action<BundleOptions> configure) {

            services.AddSingleton<IBundleProvider, BundleProvider>();
            services.AddTransient(serviceProvider =>
            {
                var env = serviceProvider.GetService<IHostingEnvironment>();

                var options = new BundleOptions(env);
                configure?.Invoke(options);

                return options;
            });

            return services;
        }
    }
}