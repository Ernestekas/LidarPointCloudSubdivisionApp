using laszip.net;
using LidarPointCloudSubdivision.Models;
using System;
using System.Diagnostics;

namespace LidarPointCloudSubdivision
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Begin time:" + DateTime.Now);
            var lasReader = new laszip_dll();
            var compressed = true;

            lasReader.laszip_open_reader("2743_1234.las", ref compressed);

            var min = new Point()
            {
                X = lasReader.header.min_x,
                Y = lasReader.header.min_y,
                Z = lasReader.header.min_z
            };

            var max = new Point()
            {
                X = lasReader.header.max_x,
                Y = lasReader.header.max_y,
                Z = lasReader.header.max_z
            };

            // First octans.
            var main = new Octan()
            {
                MinCoordinate = min,
                MaxCoordinate = max
            };

            for(int z = 0; z < 2; z++)
            {
                for(int y = 0; y < 2; y++)
                {
                    for(int x = 0; x < 2; x++)
                    {
                        var octan = new Octan();
                        octan.MinCoordinate = new Point()
                        {
                            X = main.MinCoordinate.X + main.MaxCoordinate.X / 2 * x,
                            Y = main.MinCoordinate.Y + main.MaxCoordinate.Y / 2 * y,
                            Z = main.MinCoordinate.Z + main.MaxCoordinate.Z / 2 * z
                        };

                        octan.MaxCoordinate = new Point()
                        {
                            X = (main.MaxCoordinate.X + main.MaxCoordinate.X * x) / 2,
                            Y = (main.MaxCoordinate.Y + main.MaxCoordinate.Y * y) / 2,
                            Z = (main.MaxCoordinate.Z + main.MaxCoordinate.Z * z) / 2
                        };

                        main.Octans[z, y, x] = octan;
                    }
                }
            }

            //for (int pointIndex = 0; pointIndex < 10; pointIndex++)
            //{
            //    var coordArray = new double[3];
            //    lasReader.laszip_read_point();
            //    lasReader.laszip_get_coordinates(coordArray);
            //    Console.WriteLine($"Point: . X: {coordArray[0]} Y: {coordArray[1]} Z: {coordArray[2]}");
            //}

            lasReader.laszip_close_reader();
        }
    }
}
