using OSMtoSharp;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailwaysGenerator
{
    public static class RailwaysConstants
    {


    }



    public static GameObject CreateRailway(UnityRailway railway, OsmBounds bounds)
    {
        GameObject result = new GameObject();
        result.name = "<railway>";

        if (railway.HighwayPoints.Count > 1)
        {

            Vector3[] linePoints = new Vector3[railway.HighwayPoints.Count];

            for (int i = 0; i < railway.HighwayPoints.Count; i++)
            {
                linePoints[i] = UnityDataConverters.GetPointFromUnityPoint(railway.HighwayPoints[i], bounds);
            }
            LineRenderer lineRender = result.AddComponent<LineRenderer>();
            lineRender.numPositions = railway.HighwayPoints.Count;

            lineRender.material = new Material(Shader.Find("Sprites/Default"));

            float width = 0.2f;
            Color color = Color.black;

            lineRender.startWidth = width;
            lineRender.endWidth = width;

            lineRender.startColor = color;
            lineRender.endColor = color;

            lineRender.SetPositions(linePoints);
        }
        return result;
    }

    public static void AddRailwayToGameObject(IEnumerable<IUnityModel> railwaysList, Transform parent, OsmBounds bounds)
    {
        foreach (UnityRailway railway in railwaysList)
        {
            if (railway != null && railway.HighwayPoints.Count > 1)
            {
                GameObject newHighway = RailwaysGenerator.CreateRailway(railway, bounds);
                newHighway.transform.SetParent(parent);
            }
        }
    }
}
