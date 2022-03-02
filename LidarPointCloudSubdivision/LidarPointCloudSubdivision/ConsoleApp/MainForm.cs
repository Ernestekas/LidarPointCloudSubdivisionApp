using LidarPointCloudSubdivision.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LidarPointCloudSubdivision.ConsoleApp
{
    public class MainForm
    {
        private readonly LasReaderService _lasReaderService;

        public MainForm(LasReaderService lasReaderService)
        {
            _lasReaderService = lasReaderService;
        }

        public void Run()
        {
            bool run = true;
            while (run)
            {
                Console.WriteLine("Subdivide point cloud using octree.");
                Console.WriteLine("This app will take some time, be patient.");
                Console.WriteLine("Press any key to begin reading default .las file...");
                Console.ReadKey();
                Console.WriteLine("Stage 1: Reading points from file...");
                
            }
        }
    }
}
