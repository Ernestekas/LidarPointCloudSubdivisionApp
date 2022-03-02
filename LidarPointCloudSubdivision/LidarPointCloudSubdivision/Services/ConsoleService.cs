using LidarPointCloudSubdivision.ConsoleApp;
using LidarPointCloudSubdivision.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LidarPointCloudSubdivision.Services
{
    public class ConsoleService
    {
        private readonly LasReaderService _lasReaderService;

        public ConsoleService(LasReaderService lasReaderService)
        {
            _lasReaderService = lasReaderService;
        }

        public void Run()
        {
            bool run = true;
            while (run)
            {
                List<Point> points = RunStageOne_ReadPoints();
            }
        }

        private List<Point> RunStageOne_ReadPoints()
        {
            var startTime = DateTime.Now;
            Console.WriteLine("Subdivide point cloud using octree.");
            Console.WriteLine("This app will take some time, be patient.");
            Console.WriteLine("Press any key to begin reading default .las file...");
            Console.ReadKey();
            Console.Clear();

            Console.WriteLine("Stage 1: BEGIN...");
            Console.WriteLine("Reading points from file...");

            List<Point> points = _lasReaderService.GetAllPoints();

            Console.WriteLine("Stage 1: FINISHED...");
            Console.WriteLine($"Stage 1: Time spent reading points: {DateTime.Now.Subtract(startTime)}");
            Console.WriteLine($"Total points: {points.Count}");

            return points;
        }

        private void StageTwo_Subdivide(List<Point> points)
        {

        }
    }
}
