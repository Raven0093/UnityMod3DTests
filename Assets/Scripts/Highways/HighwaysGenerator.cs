using OSMtoSharp;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighwaysGenerator
{
    public static class HighwaysConstants
    {
        public const float motorwayWidth = 0.5f;
        public const float trunkWidth = 0.45f;
        public const float primaryWidth = 0.4f;
        public const float secondaryWidth = 0.3f;
        public const float tertiaryWidth = 0.2f;
        public const float residentialWidth = 0.15f;
        public const float serviceWidth = 0.1f;
        public const float defaultWidth = 0.05f;


        public static Color motorwayColor = new Color(232 / 255, 146f / 255f, 162f / 255f, 1);
        public static Color trunkColor = new Color(251f / 255f, 177f / 255f, 153f / 255f, 1);
        public static Color primaryColor = new Color(254f / 255f, 214f / 255f, 161f / 255f, 1);
        public static Color secondaryColor = new Color(246f / 255f, 250f / 255f, 186f / 255f, 1);
        public static Color defaultColor = new Color(255f / 255f, 255f / 255f, 255f / 255f, 1);

    }

    public static float GetHighwayWidth(HighwayTypeEnum type)
    {
        switch (type)
        {
            case HighwayTypeEnum.Motorway: return HighwaysConstants.motorwayWidth;
            case HighwayTypeEnum.Trunk: return HighwaysConstants.trunkWidth;
            case HighwayTypeEnum.Primary: return HighwaysConstants.primaryWidth;
            case HighwayTypeEnum.Secondary: return HighwaysConstants.secondaryWidth;
            case HighwayTypeEnum.Tertiary: return HighwaysConstants.tertiaryWidth;
            case HighwayTypeEnum.Residential: return HighwaysConstants.residentialWidth;
            case HighwayTypeEnum.Service: return HighwaysConstants.serviceWidth;
            default: return HighwaysConstants.defaultWidth;
        }

    }

    public static Color GetHighwayColor(HighwayTypeEnum type)
    {
        switch (type)
        {
            case HighwayTypeEnum.Motorway: return HighwaysConstants.motorwayColor;
            case HighwayTypeEnum.Trunk: return HighwaysConstants.trunkColor;
            case HighwayTypeEnum.Primary: return HighwaysConstants.primaryColor;
            case HighwayTypeEnum.Secondary: return HighwaysConstants.secondaryColor;
            default: return HighwaysConstants.defaultColor;
        }

    }

    public static GameObject CreateHighway(UnityHighway highway, OsmBounds bounds)
    {
        GameObject result = new GameObject();
        result.name = highway.Name;

        if (highway.HighwayPoints.Count > 1)
        {

            Vector3[] linePoints = new Vector3[highway.HighwayPoints.Count];

            for (int i = 0; i < highway.HighwayPoints.Count; i++)
            {
                linePoints[i] = UnityDataConverters.GetPointFromUnityPoint(highway.HighwayPoints[i], bounds);
            }
            LineRenderer lineRender = result.AddComponent<LineRenderer>();
            lineRender.numPositions = highway.HighwayPoints.Count;

            lineRender.material = new Material(Shader.Find("Sprites/Default"));

            float width = GetHighwayWidth(highway.HighwayType);
            Color color = GetHighwayColor(highway.HighwayType);

            lineRender.startWidth = width;
            lineRender.endWidth = width;

            lineRender.startColor = color;
            lineRender.endColor = color;

            lineRender.SetPositions(linePoints);
        }
        return result;
    }

    public static void AddHighwaysToGameObject(IEnumerable<IUnityModel> highwaysList, Transform parent, OsmBounds bounds)
    {
        foreach (UnityHighway highway in highwaysList)
        {
            if (highway != null && highway.HighwayPoints.Count > 1)
            {
                GameObject newHighway = HighwaysGenerator.CreateHighway(highway, bounds);
                newHighway.transform.SetParent(parent);
            }
        }
    }
}
