using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LidarPointCloudSubdivision.Models
{
    public class Octree
    {
        public double MinCoordinate { get; set; }
        public double MaxCoordinate { get; set; }
        public Octan[,,] octans { get; set; } = new Octan[2, 2, 2];
    }
}
