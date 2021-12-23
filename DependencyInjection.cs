namespace ClickTrack
{
    using System;
    using ClickTrack.Repository;
    using Microsoft.Extensions.DependencyInjection;

    public class DependencyInjection
    {
        public static void SetDependencies(IServiceCollection services)
        {
            services.AddSingleton<IClickRepository, ClickRepository>();
        }

        public static void FinalizeServices(IServiceProvider applicationServices)
        {
            (applicationServices.GetService<IClickRepository>() as IDisposable)?.Dispose();
        }
    }
}
