# Optimize Path

This endpoint solves the Vehicle Routing Problem (VRP) for multiple UAVs or the Traveling Salesman Problem (TSP) if only one UAV is specified. The distances are provided in meters.

## Request

* Method: POST
* URL: {base_url}/api/optimize_path

### Request body

The request body should be a JSON object with the following properties:

```json
{
  "uavCount": 3,
  "coordinates": [
    {
      "latitude": 50.04306419473103,
      "longitude": 36.288142204284675
    },
    {
      "latitude": 50.04261633070681,
      "longitude": 36.28449440002442
    },
    // ... coordinates for other waypoints
  ]
}

```

* '**uavCount**' (integer, required): The number of UAVs involved in the optimization.
* '**coordinates**' (array, required): An array of objects representing the coordinates of waypoints. Each waypoint should have latitude and longitude properties.
    * '**latitude**' (number, required): The latitude of the waypoint.
    * '**longitude**' (number, required): The longitude of the waypoint.

## Response

The response will be a JSON object with the following properties:

```json
{
  "uavPaths": [
    {
      "uavId": 0,
      "path": [
        {
          "latitude": 50.04306419473103,
          "longitude": 36.288142204284675
        },
        // ... coordinates for the optimized path
      ],
      "distance": 1015.72
    },
    {
      "uavId": 1,
      "path": [
        {
          "latitude": 50.04306419473103,
          "longitude": 36.288142204284675
        },
        // ... coordinates for the optimized path
      ],
      "distance": 936.93
    },
    // ... paths for other UAVs
  ]
}
```
* '**uavPaths**' (array): An array of objects representing the optimized paths for each UAV.
  * '**uavId**' (integer): The ID of the UAV.
  * '**path**' (array): An array of objects representing the coordinates of the optimized path for the UAV.
    * '**latitude**' (number): The latitude of the waypoint.
    * '**longitude**' (number): The longitude of the waypoint.
  * '**distance**' (number): The total distance of the optimized path for the UAV in meters.