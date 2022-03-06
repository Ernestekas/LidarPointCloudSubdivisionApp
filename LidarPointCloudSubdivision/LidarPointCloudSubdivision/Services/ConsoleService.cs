using LidarPointCloudSubdivision.Models;
using LidarPointCloudSubdivision.Repositories;
using System;
using System.Collections.Generic;

namespace LidarPointCloudSubdivision.Services
{
    public class ConsoleService
    {
        private readonly LasReaderService _lasReaderService;
        private readonly OctreeService _octreeService;
        private readonly OctreeRepository _octreeRepository;
        private readonly OctreeElipsoidService _octreeElipsoidService;

        public ConsoleService(LasReaderService lasReaderService, OctreeService octreeService, OctreeRepository octreeRepository, OctreeElipsoidService octreeElipsoidService)
        {
            _lasReaderService = lasReaderService;
            _octreeService = octreeService;
            _octreeRepository = octreeRepository;
            _octreeElipsoidService = octreeElipsoidService;
        }

        public void Run()
        {
            bool run = true;
            while (run)
            {
                Console.Clear();
                Console.WriteLine("Subdivide point cloud using octree.");
                Console.WriteLine("This app will take some time, be patient.");
                Console.WriteLine();
                Console.WriteLine("Select an action by entering option number:");
                Console.WriteLine("1 - Begin reading points from default .las file.");
                Console.WriteLine("2 - Exit an application.");
                string action = Console.ReadLine();

                switch (action)
                {
                    case "1":
                        Console.Clear();
                        List<Point> points = RunStageOne_ReadPoints();
                        run = !PreStageTwo_CheckUserAction(points);

                        break;
                    case "2":
                        run = false;
                        break;
                    default:
                        Console.WriteLine("Invalid command.");
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                }
            }
        }

        private List<Point> RunStageOne_ReadPoints()
        {
            var startTime = DateTime.Now;
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("Stage 1: BEGIN...");
            Console.WriteLine("Reading points from file...");
            Console.WriteLine();

            List<Point> points = _lasReaderService.GetAllPoints();

            Console.WriteLine();
            Console.WriteLine("Stage 1: FINISHED...");
            Console.WriteLine($"Stage 1: Time spent reading points: {DateTime.Now.Subtract(startTime)}");
            Console.WriteLine($"Total points: {points.Count}");
            Console.WriteLine();
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();

            return points;
        }

        private Octan RunStageTwo_Subdivide(List<Point> points, bool filterByEllipsoid = false)
        {
            var startTime = DateTime.Now;
            var octan = new Octan();

            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("Stage 2: BEGIN...");
            Console.WriteLine("Subdividing Point Cloud...");

            if (filterByEllipsoid)
            {
                octan = _octreeElipsoidService.SubdividePointCloud(points);
            }
            else
            {
                octan = _octreeService.SubdividePointCloud(points);
            }
            
            Console.WriteLine("Stage 2: FINISHED...");
            Console.WriteLine($"Stage 2 proccess time: {DateTime.Now.Subtract(startTime)}");
            Console.WriteLine();
            return octan;
        }

        private bool PreStageTwo_CheckUserAction(List<Point> points)
        {
            Console.Clear();
            bool endApplication = false;
            bool run = true;
            while (run)
            {
                Console.WriteLine("Select an action by entering option number:");
                Console.WriteLine("1 - Begin Subdivision proccess.");
                Console.WriteLine("2 - Begin Subdivision proccess (Point check in octan ellipsoid)");
                Console.WriteLine("3 - Exit an application.");

                string action = Console.ReadLine();
                switch (action)
                {
                    case "1":
                        var octan = RunStageTwo_Subdivide(points);

                        Console.WriteLine("Press any key to continue writing octree to file...");
                        Console.ReadKey();

                        StageThree_WriteOctreeToJson(octan);
                        run = false;
                        endApplication = true;
                        break;
                    case "2":
                        var octanEll = RunStageTwo_Subdivide(points, true);

                        Console.WriteLine("Press any key to continue writing octree to file...");
                        Console.ReadKey();

                        StageThree_WriteOctreeToJson(octanEll);
                        run = false;
                        endApplication = true;
                        break;
                    case "3":
                        endApplication = true;
                        break;
                    default:
                        Console.WriteLine("Invalid command.");
                        break;
                }
            }

            return endApplication;
        }

        private void StageThree_WriteOctreeToJson(Octan octan)
        {
            Console.Clear();
            var startTime = DateTime.Now;
            Console.WriteLine("Stage 3: BEGIN...");
            Console.WriteLine("Writing octree into a file...");

            _octreeRepository.OctreeToJson(octan);

            Console.WriteLine("Stage 3: FINISHED...");
            Console.WriteLine($"Stage 3 proccess time: {DateTime.Now.Subtract(startTime)}");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}
