namespace LidarPointCloudSubdivision.Models
{
    public class Octan
    {
        public Point MinCoordinate { get; set; }
        public Point MaxCoordinate { get; set; }
        public Octan[,,] Octans { get; set; } = new Octan[2, 2, 2];
    }
}
