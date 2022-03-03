namespace LidarPointCloudSubdivision.Models
{
    public class Octree
    {
        public double MinCoordinate { get; set; }
        public double MaxCoordinate { get; set; }
        public Octan[,,] Octans { get; set; } = new Octan[2, 2, 2];
    }
}
