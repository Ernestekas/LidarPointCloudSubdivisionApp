using LidarPointCloudSubdivision.Models;
using System.Collections.Generic;
using System.Linq;

namespace LidarPointCloudSubdivision.Services
{
    public class OctreeService
    {
        private readonly int _maxDepth = 5;
        private readonly LasReaderService _lasReaderService;

        public OctreeService(LasReaderService lasReaderService)
        {
            _lasReaderService = lasReaderService;
        }

        public Octan SubdividePointCloud(List<Point> points)
        {
            Point[] spaceMinMax = _lasReaderService.GetMinMaxCoordinates();

            var octan = new Octan
            {
                MinCoordinate = spaceMinMax[0],
                MaxCoordinate = spaceMinMax[1]
            };

            return Create(points, octan);
        }

        public Octan Create(List<Point> points, Octan parentOctan, int currentDepthLevel = 0)
        {
            int level = currentDepthLevel + 1;

            if(level <= _maxDepth && GetPointCountInOctan(parentOctan, points) > 1)
            {
                for (int z = 0; z < 2; z++)
                {
                    for (int y = 0; y < 2; y++)
                    {
                        for (int x = 0; x < 2; x++)
                        {
                            Octan child = FormChildOctan(z, y, x, parentOctan);

                            parentOctan.Octans[z, y, x] = Create(points, child, level);
                        }
                    }
                }
            }

            return parentOctan;
        }

        private Octan FormChildOctan(int z, int y, int x, Octan parent)
        {
            return new Octan
            {
                MinCoordinate = new Point
                {
                    X = parent.MinCoordinate.X + (parent.MaxCoordinate.X - parent.MinCoordinate.X) / 2 * x,
                    Y = parent.MinCoordinate.Y + (parent.MaxCoordinate.Y - parent.MinCoordinate.Y) / 2 * y,
                    Z = parent.MinCoordinate.Z + (parent.MaxCoordinate.Z - parent.MinCoordinate.Z) / 2 * z
                },
                MaxCoordinate = new Point
                {
                    X = parent.MinCoordinate.X + (parent.MaxCoordinate.X - parent.MinCoordinate.X) / 2 + (parent.MaxCoordinate.X - parent.MinCoordinate.X) / 2 * x,
                    Y = parent.MinCoordinate.Y + (parent.MaxCoordinate.X - parent.MaxCoordinate.Y) / 2 + (parent.MaxCoordinate.Y - parent.MinCoordinate.Y) / 2 * y,
                    Z = parent.MaxCoordinate.Z + (parent.MaxCoordinate.Z - parent.MaxCoordinate.Z) / 2 + (parent.MaxCoordinate.Z - parent.MinCoordinate.Z) / 2 * z
                }
            };
        }

        private int GetPointCountInOctan(Octan octan, List<Point> points)
        {
            List<Point> pointsInOctan = points
                .Where(p => 
                            p.X >= octan.MinCoordinate.X &&
                            p.Y >= octan.MinCoordinate.Y &&
                            p.Z >= octan.MinCoordinate.Z &&
                            p.X < octan.MaxCoordinate.X &&
                            p.Y < octan.MaxCoordinate.Y &&
                            p.Z < octan.MaxCoordinate.Z)
                .ToList();

            return pointsInOctan.Count;
        }
    }
}
