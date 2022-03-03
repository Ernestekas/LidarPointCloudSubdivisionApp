using LidarPointCloudSubdivision.Models;
using Newtonsoft.Json;
using System.IO;

namespace LidarPointCloudSubdivision.Repositories
{
    public class OctreeRepository
    {
        private readonly string _octreeDataFile = "Data/octree.json";

        public OctreeRepository()
        {
            if (!File.Exists(_octreeDataFile))
            {
                File.WriteAllText(_octreeDataFile, "[]");
            }
        }

        public void OctreeToJson(Octan mainOctan)
        {
            string json = JsonConvert.SerializeObject(mainOctan);
            File.WriteAllText(_octreeDataFile, json);
        }
    }
}
