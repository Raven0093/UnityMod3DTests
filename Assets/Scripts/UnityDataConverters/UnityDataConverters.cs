using OSMtoSharp;
using UnityEngine;

public class UnityDataConverters
{
    public static Vector3 GetPointFromUnityPoint(UnityPoint unityPoint, OsmBounds bounds)
    {
        float multipler = 10000;
        float x = (unityPoint.Lat - (float)bounds.MinLat) * multipler;
        float y = 0;
        float z = (unityPoint.Lon - (float)bounds.MinLon) * multipler;

        return new Vector3(x, y, z);
    }
}
