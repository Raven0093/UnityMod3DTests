using OSMtoSharp;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailwaysGenerator
{
    private static class RailwaysConstants
    {
        public const float defaultWidth = 0.2f;
        public static Color defaultColor = new Color(16 / 255f, 17 / 255f, 17 / 255f, 1);
    }

    private static float GetHighwayWidth(RailwayTypeEnum type)
    {
        return RailwaysConstants.defaultWidth * Assets.Scripts.Constants.Constants.Scale;
    }

    private static Color GetHighwayColor(RailwayTypeEnum type)
    {
        return RailwaysConstants.defaultColor;
    }
    public static GameObject CreateRailway(UnityRailway railway, OsmBounds bounds)
    {
        GameObject result = new GameObject();
        result.name = "<railway>";


        Vector3[] linePoints = new Vector3[railway.RailwayPoints.Count];

        for (int i = 0; i < railway.RailwayPoints.Count; i++)
        {
            linePoints[i] = UnityDataConverters.GetPointFromUnityPoint(railway.RailwayPoints[i], bounds);
        }
        LineRenderer lineRender = result.AddComponent<LineRenderer>();
        lineRender.positionCount = railway.RailwayPoints.Count;

        lineRender.material = new Material(Shader.Find("Sprites/Default"));

        float width = GetHighwayWidth(railway.RailwayType);
        Color color = GetHighwayColor(railway.RailwayType);

        lineRender.startWidth = width;
        lineRender.endWidth = width;

        lineRender.startColor = color;
        lineRender.endColor = color;

        lineRender.SetPositions(linePoints);

        return result;
    }

    public static void AddRailwayToGameObject(IEnumerable<IUnityModel> railwaysList, Transform parent, OsmBounds bounds)
    {
        foreach (UnityRailway railway in railwaysList)
        {
            if (railway != null && railway.RailwayPoints.Count > 1)
            {
                GameObject newHighway = CreateRailway(railway, bounds);
                newHighway.transform.SetParent(parent);
            }
        }
    }
}
