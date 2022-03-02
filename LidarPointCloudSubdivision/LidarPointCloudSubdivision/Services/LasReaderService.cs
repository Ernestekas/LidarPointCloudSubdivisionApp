using laszip.net;
using LidarPointCloudSubdivision.Models;
using System.Collections.Generic;

namespace LidarPointCloudSubdivision.Services
{
    public class LasReaderService
    {
        private readonly string _defaultFile;
        private readonly laszip_dll _reader;
        private bool _compressed;

        public LasReaderService()
        {
            _defaultFile = "2743_1234.las";
            _reader = new laszip_dll();
            _compressed = true;
        }

        public List<Point> GetAllPoints()
        {
            var points = new List<Point>();
            
            _reader.laszip_open_reader(_defaultFile, ref _compressed);

            var pointsCount = _reader.header.number_of_point_records;

            for(int i = 0; i < pointsCount; i++)
            {
                var coordArray = new double[3];
                _reader.laszip_read_point();
                _reader.laszip_get_coordinates(coordArray);

                var point = new Point()
                {
                    X = coordArray[0],
                    Y = coordArray[1],
                    Z = coordArray[2]
                };

                points.Add(point);
            }

            _reader.laszip_close_reader();
            return points;
        }

        public Point[] GetMinMaxCoordinates()
        {
            _reader.laszip_open_reader(_defaultFile, ref _compressed);
            var minPoint = new Point()
            {
                X = _reader.header.min_x,
                Y = _reader.header.min_y,
                Z = _reader.header.min_z
            };

            var maxPoint = new Point()
            {
                X = _reader.header.max_x,
                Y = _reader.header.max_y,
                Z = _reader.header.max_z
            };

            _reader.laszip_close_reader();

            return new Point[] 
            { 
                minPoint, 
                maxPoint 
            };
        }
    }
}
