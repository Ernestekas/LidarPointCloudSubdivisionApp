using LidarPointCloudSubdivision.Repositories;
using LidarPointCloudSubdivision.Services;
using Microsoft.Extensions.DependencyInjection;

namespace LidarPointCloudSubdivision
{
    public static class DependencyInjection
    {
        public static void ConfigureServices(this IServiceCollection services)
        {
            services.AddTransient<ConsoleService>();
            services.AddTransient<LasReaderService>();
            services.AddTransient<OctreeService>();
            services.AddTransient<OctreeRepository>();
            services.AddTransient<OctreeElipsoidService>();
        }
    }
}
