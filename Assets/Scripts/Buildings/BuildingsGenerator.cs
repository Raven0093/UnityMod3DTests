using OSMtoSharp;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingsGenerator
{

    private static class BuildingsConstants
    {
        public const float wallHeight = 2f;
    }

    public static float GetWallHeight(BuildingsTypeEnum type, int level)
    {
        return BuildingsConstants.wallHeight * Assets.Scripts.Constants.Constants.Scale * level;
    }

    //private static class HighwaysConstants
    //{
    //    //public const float motorwayWidth = 0.5f;
    //    //public const float trunkWidth = 0.45f;
    //    //public const float primaryWidth = 0.4f;
    //    //public const float secondaryWidth = 0.3f;
    //    //public const float tertiaryWidth = 0.2f;
    //    //public const float residentialWidth = 0.15f;
    //    //public const float serviceWidth = 0.1f;
    //    //public const float defaultWidth = 0.05f;


    //    public const float motorwayWidth = 1f;
    //    public const float trunkWidth = 0.9f;
    //    public const float primaryWidth = 0.8f;
    //    public const float secondaryWidth = 0.7f;
    //    public const float tertiaryWidth = 0.5f;
    //    public const float residentialWidth = 0.3f;
    //    public const float serviceWidth = 0.2f;
    //    public const float proposedWidth = 0.01f;
    //    public const float underConstruction = 0.01f;
    //    public const float defaultWidth = 0.1f;


    //    public static Color motorwayColor = new Color(232 / 255, 146f / 255f, 162f / 255f, 1);
    //    public static Color trunkColor = new Color(251f / 255f, 177f / 255f, 153f / 255f, 1);
    //    public static Color primaryColor = new Color(254f / 255f, 214f / 255f, 161f / 255f, 1);
    //    public static Color secondaryColor = new Color(246f / 255f, 250f / 255f, 186f / 255f, 1);
    //    public static Color defaultColor = new Color(255f / 255f, 255f / 255f, 255f / 255f, 1);

    //}

    //private static float GetHighwayWidth(HighwayTypeEnum type)
    //{
    //    switch (type)
    //    {
    //        case HighwayTypeEnum.Motorway: return HighwaysConstants.motorwayWidth;
    //        case HighwayTypeEnum.Trunk: return HighwaysConstants.trunkWidth;
    //        case HighwayTypeEnum.Primary: return HighwaysConstants.primaryWidth;
    //        case HighwayTypeEnum.Secondary: return HighwaysConstants.secondaryWidth;
    //        case HighwayTypeEnum.Tertiary: return HighwaysConstants.tertiaryWidth;
    //        case HighwayTypeEnum.Residential: return HighwaysConstants.residentialWidth;
    //        case HighwayTypeEnum.Service: return HighwaysConstants.serviceWidth;
    //        case HighwayTypeEnum.Construction: return HighwaysConstants.proposedWidth;
    //        case HighwayTypeEnum.Proposed: return HighwaysConstants.underConstruction;
    //        default: return HighwaysConstants.defaultWidth;
    //    }

    //}

    //private static Color GetHighwayColor(HighwayTypeEnum type)
    //{
    //    switch (type)
    //    {
    //        case HighwayTypeEnum.Motorway: return HighwaysConstants.motorwayColor;
    //        case HighwayTypeEnum.Trunk: return HighwaysConstants.trunkColor;
    //        case HighwayTypeEnum.Primary: return HighwaysConstants.primaryColor;
    //        case HighwayTypeEnum.Secondary: return HighwaysConstants.secondaryColor;
    //        default: return HighwaysConstants.defaultColor;
    //    }

    //}

    public static GameObject CreateBuilding(UnityBuilding building, OsmBounds bounds)
    {
        GameObject result = new GameObject();
        result.name = building.Name;

        //Vector3[] linePoints = new Vector3[highway.HighwayPoints.Count];


        for (int i = 1; i < building.BuildingPoints.Count; i++)
        {
            GameObject newWall = GameObject.CreatePrimitive(PrimitiveType.Cube);
            newWall.transform.SetParent(newWall.transform);
            newWall.name = "Wall";

            Vector3 pA = UnityDataConverters.GetPointFromUnityPoint(building.BuildingPoints[i], bounds);
            Vector3 pB = UnityDataConverters.GetPointFromUnityPoint(building.BuildingPoints[i - 1], bounds);

            float height = GetWallHeight(building.BuildingsType, building.BuildingLevels);

            pA.y = height / 2;
            pB.y = height / 2;

            Vector3 between = pB - pA;
            float distance = between.magnitude;

            newWall.transform.localScale = new Vector3(0.0001f, height, distance); ;
            newWall.transform.position = pA + (between / 2.0f);
            newWall.transform.LookAt(pB);

            newWall.transform.SetParent(result.transform);
        }

        //for (int i = 0; i < highway.HighwayPoints.Count; i++)
        //{
        //    linePoints[i] = UnityDataConverters.GetPointFromUnityPoint(highway.HighwayPoints[i], bounds);
        //}
        //LineRenderer lineRender = result.AddComponent<LineRenderer>();
        //lineRender.positionCount = highway.HighwayPoints.Count;

        //lineRender.material = new Material(Shader.Find("Sprites/Default"));

        //float width = GetHighwayWidth(highway.HighwayType);
        //Color color = GetHighwayColor(highway.HighwayType);

        //lineRender.startWidth = width;
        //lineRender.endWidth = width;

        //lineRender.startColor = color;
        //lineRender.endColor = color;

        //lineRender.SetPositions(linePoints);

        return result;
    }

    public static void AddBuildingsToGameObject(IEnumerable<IUnityModel> buildingsList, Transform parent, OsmBounds bounds)
    {
        foreach (UnityBuilding building in buildingsList)
        {
            if (building != null && building.BuildingPoints.Count > 3)
            {
                GameObject newHighway = CreateBuilding(building, bounds);
                newHighway.transform.SetParent(parent);
            }
        }
    }
}
