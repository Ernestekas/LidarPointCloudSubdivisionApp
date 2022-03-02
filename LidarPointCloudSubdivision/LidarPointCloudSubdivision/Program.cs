using laszip.net;
using LidarPointCloudSubdivision.Models;
using LidarPointCloudSubdivision.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics;

namespace LidarPointCloudSubdivision
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var services = new ServiceCollection();
            services.ConfigureServices();

            var serviceProvider = services.BuildServiceProvider();

            var consoleService = serviceProvider.GetService<ConsoleService>();
            consoleService.Run();
        }
    }
}
