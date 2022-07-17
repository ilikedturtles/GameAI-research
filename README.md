# CS380 Research Project
This Project is the combined effort of @ilikedturtles and @MarkKouris.  The goal of the project is outlined in the pitch document below.

[Pitch Document](./docs/CS380ResearchProjectPitch.pdf)

## Project Information
```
unity: 2021.3.5f1
platforms: Windows, Linux, MacOS
```

# Controls
The left panel provides tools for configuring variables in the algorithm being run on the map.  Each Algorithm can enabled or disabled by clicking the respective checkbox.

The remaining screen shows the map and agent.  The map is made of tiles and will be updated in real time as you adjust the algorithm paramenters.  An `enabled` tile is Green, and a disabled tile is Black.

## Map Gen Sys
### Width and Height
The dimensions of the map being generated. The camera will move to fit the map.
Default: `10`, Range : `[5, 50]`
### Threshold
The float value used to convert to booleans after filtering has been applied to the random data.
Default: `0.0`, Range: `[-1.0f, 1.0f]`

## Camera
### Y Adjust
Increasing this value moves the camera away from the map, so you can see more.
Default: `0.0`, Range: `[0.0f, 50.0f]`

## Biggest Island
This algorithm identifies and singles out the largest floodfill area on the map.

## Propogation
Works similar to Project3 Propogation.  Each iteration enables neighbors of the currently enabled nodes.
### Iterations
Number of iterations to run.
Default: `1`, Range: `[0, 5]`

## Erosion
Works as an inverted version of the Propogation Algorithm.  Neighbors of Disabled nodes will also be disabled.
### Iterations
Number of iterations to run.
Default: `1`, Range: `[0, 5]`

## Invert
Inverts the enabled status of each tile.

## Biggest Island 2
A second instance of the Biggest Island algorithm, useful for cleaning up the output of an eroded map.

## Hole Fill
Useful for Patching "holes" in the map. Enables all disabled tiles with the specified number of enabled neighbors.
### Iterations
Number of iterations to run.
Default: `1`, Range: `[0, 5]`
### Required Neighbors
Number of enabled neighbors a disabled tile must have to become enabled.
Default: `4`, Range: `[2, 4]`

## Seed
Displays the current seed.
### New Seed
Clicking this will generate a new map based on the same paramenters.
