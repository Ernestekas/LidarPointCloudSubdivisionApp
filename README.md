# LidarPointCloudSubdivisionApp

Descritpion
--------------------
This application read point cloud file (.las format), then subdivides space using octree and returns octree.json with octree object. After stage 3 octree will be saved into a octree.json which will be located in:
project folder > LidarPointCloudSubdivision > bin > Debug > net5.0 > Data folder.

Requirements
--------------------
! You must:
1. Using Visual Studio, put your yourFileName.las into a project Data folder and rename it to input.las. 
2. Right click input.las file and select properties.
3. Change property 'Copy to Output Directory' value to 'Copy always'.
