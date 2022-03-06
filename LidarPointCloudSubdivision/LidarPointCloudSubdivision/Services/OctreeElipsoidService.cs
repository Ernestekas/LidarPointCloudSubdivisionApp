using LidarPointCloudSubdivision.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LidarPointCloudSubdivision.Services
{
    public class OctreeElipsoidService
    {
        private const int _maxDepth = 5;
        private readonly LasReaderService _lasReaderService;

        public OctreeElipsoidService(LasReaderService lasReaderService)
        {
            _lasReaderService = lasReaderService;
        }

        public Octan SubdividePointCloud(List<Point> allPoints)
        {
            Point[] spaceMinMax = _lasReaderService.GetMinMaxCoordinates();

            var octan = new Octan
            {
                MinCoordinate = spaceMinMax[0],
                MaxCoordinate = spaceMinMax[1]
            };
            octan.InscribedEllipsoid = CreateEllipsoid(octan.MinCoordinate, octan.MaxCoordinate);
            List<Point> pointsInEllipsoid = allPoints.Where(p => PointIsInEllipsoid(p, octan.InscribedEllipsoid)).ToList();

            return Create(pointsInEllipsoid, octan);
        }

        private Octan Create(List<Point> parentEllipsoidPoints, Octan parentOctan, int currentDepthLevel = 0)
        {
            int level = currentDepthLevel + 1;

            if(level <= _maxDepth && parentEllipsoidPoints.Count > 1)
            {
                for(int z = 0; z < 2; z++)
                {
                    for(int y = 0; y < 2; y++)
                    {
                        for(int x = 0; x < 2; x++)
                        {
                            var childOctan = FormChildOctan(z, y, x, parentOctan);

                            List<Point> childEllipsoidPoints = parentEllipsoidPoints
                                                                    .Where(p => PointIsInEllipsoid(p, childOctan.InscribedEllipsoid))
                                                                    .ToList();

                            parentOctan.Octans[z, y, x] = Create(childEllipsoidPoints, childOctan, currentDepthLevel);
                        }
                    }
                }
            }

            return parentOctan;
        }

        private Octan FormChildOctan(int z, int y, int x, Octan parent)
        {
            var octan = new Octan
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
                    Y = parent.MinCoordinate.Y + (parent.MaxCoordinate.Y - parent.MinCoordinate.Y) / 2 + (parent.MaxCoordinate.Y - parent.MinCoordinate.Y) / 2 * y,
                    Z = parent.MaxCoordinate.Z + (parent.MaxCoordinate.Z - parent.MinCoordinate.Z) / 2 + (parent.MaxCoordinate.Z - parent.MinCoordinate.Z) / 2 * z
                }
            };
            octan.InscribedEllipsoid = CreateEllipsoid(octan.MinCoordinate, octan.MaxCoordinate);

            return octan;
        }

        private static Ellipsoid CreateEllipsoid(Point minCoordinates, Point maxCoordinates)
        {
            var ellipsoid = new Ellipsoid
            {
                Center = new Point
                {
                    X = minCoordinates.X + (maxCoordinates.X - minCoordinates.X) / 2,
                    Y = minCoordinates.Y + (maxCoordinates.Y - minCoordinates.Y) / 2,
                    Z = minCoordinates.Z + (maxCoordinates.Z - minCoordinates.Z) / 2
                },
                XAxisLength = maxCoordinates.X - minCoordinates.X,
                YAxisLength = maxCoordinates.Y - minCoordinates.Y,
                ZAxisLength = maxCoordinates.Z - minCoordinates.Z
            };

            return ellipsoid;
        }

        private bool PointIsInEllipsoid(Point p, Ellipsoid ellipsoid)
        {
            if(
                (Math.Pow((p.X - ellipsoid.Center.X), 2) / Math.Pow((ellipsoid.XAxisLength / 2),2)) +
                (Math.Pow((p.Y - ellipsoid.Center.Y), 2) / Math.Pow((ellipsoid.YAxisLength / 2),2)) +
                (Math.Pow((p.Z - ellipsoid.Center.Z), 2) / Math.Pow((ellipsoid.ZAxisLength / 2),2)) <= 1)
            {
                return true;
            }
            return false;
        }
    }
}
