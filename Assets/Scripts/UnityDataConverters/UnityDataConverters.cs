using OSMtoSharp;
using UnityEngine;

public class UnityDataConverters
{
    public static Vector3 GetPointFromUnityPoint(UnityPoint unityPoint, OsmBounds bounds)
    {
        float x = ((unityPoint.Lat - (float)bounds.MinLat) * Assets.Scripts.Constants.Constants.multipler);
        float y = 0;
        float z = -((unityPoint.Lon - (float)bounds.MinLon) * Assets.Scripts.Constants.Constants.multipler);

        return new Vector3(x, y, z);
    }
}
