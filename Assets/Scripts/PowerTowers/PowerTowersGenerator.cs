using OSMtoSharp;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerTowersGenerator
{
    private static class PowerTowersConstants
    {
        public const float defaultWidth = 0.1f;
        public static Color defaultColor = new Color(1 / 255f, 1 / 255f, 1 / 255f, 1);
        public const float defaultHeight = 4f;
    }

    private static float GetPowerTowerWidth()
    {
        return PowerTowersConstants.defaultWidth * Assets.Scripts.Constants.Constants.Scale;
    }

    private static float GetPowerTowerHeight()
    {
        return PowerTowersConstants.defaultHeight * Assets.Scripts.Constants.Constants.Scale;
    }

    private static Color GetPowerTowerColor()
    {
        return PowerTowersConstants.defaultColor;
    }
    public static GameObject CreatePowerTower(UnityPowerTower powerTower, OsmBounds bounds)
    {
        GameObject result = new GameObject();
        result.name = "<powerline>";


        Vector3[] linePoints = new Vector3[2];


        linePoints[0] = UnityDataConverters.GetPointFromUnityPoint(powerTower.Point, bounds);
        linePoints[1] = linePoints[0];
        linePoints[1].y = GetPowerTowerHeight();

        LineRenderer lineRender = result.AddComponent<LineRenderer>();
        lineRender.positionCount = 2;

        lineRender.material = new Material(Shader.Find("Sprites/Default"));

        float width = GetPowerTowerWidth();
        Color color = GetPowerTowerColor();

        lineRender.startWidth = width;
        lineRender.endWidth = width;

        lineRender.startColor = color;
        lineRender.endColor = color;

        lineRender.SetPositions(linePoints);

        return result;
    }

    public static void AddPowerTowersToGameObject(IEnumerable<IUnityModel> powerTowerList, Transform parent, OsmBounds bounds)
    {
        foreach (UnityPowerTower powerline in powerTowerList)
        {
            if (powerTowerList != null)
            {
                GameObject newHighway = CreatePowerTower(powerline, bounds);
                newHighway.transform.SetParent(parent);
            }
        }
    }
}
