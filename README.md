# LidarPointCloudSubdivisionApp

Descritpion
--------------------
This application read point cloud file (.las format), then subdivides space using octree and returns octree.json with octree object. 
Before stage 2 starts you will be able to select one of two octree subdivision settings.
1 - Subdivide by checking points in octan.
2 - Subdivide by checking points in octans ellipsoid. Points that are outside of octan ellipsoid are filtered out and deeper level subdivision will check only points that passed filter.

After stage 3 octree will be saved into a octree.json which will be located in:
project folder / LidarPointCloudSubdivision / bin / Debug / net5.0 / Data / octree.json

- To change octree subdivision depth in OctreeService.cs file change _maxDepth value to desired int number.

Requirements
--------------------
! You must:
1. Using Visual Studio, put your yourFileName.las into a project Data folder and rename it to input.las. 
2. Right click input.las file and select properties.
3. Change property 'Copy to Output Directory' value to 'Copy always'.
