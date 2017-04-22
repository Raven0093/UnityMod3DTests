using OSMtoSharp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Factories.Helpers
{
    public class OsmToUnityConverter
    {
        public static Vector2 GetPointFromUnityPointVec2(Point point, OsmBounds bounds)
        {
            float x = (((float)point.Lat - (float)bounds.MinLat) * Assets.Scripts.Constants.Constants.multipler);
            float y = -(((float)point.Lon - (float)bounds.MinLon) * Assets.Scripts.Constants.Constants.multipler);

            return new Vector2(x, y);
        }

        public static Vector3 GetPointFromUnityPointVec3(Point point, OsmBounds bounds)
        {
            float x = (((float)point.Lat - (float)bounds.MinLat) * Assets.Scripts.Constants.Constants.multipler);
            float y = 0f;
            float z = -(((float)point.Lon - (float)bounds.MinLon) * Assets.Scripts.Constants.Constants.multipler);

            return new Vector3(x, y, z);
        }
    }
}
