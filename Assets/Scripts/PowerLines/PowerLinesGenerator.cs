using OSMtoSharp;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerLinesGenerator
{
    private static class PowerLinesConstants
    {
        public const float defaultWidth = 0.1f;
        public static Color defaultColor = new Color(1 / 255f, 1 / 255f, 1 / 255f, 1);
        public const float defaultHeight = 4f;
    }

    private static float GetPowerLineWidth(PowerLineTypeEnum type)
    {
        return PowerLinesConstants.defaultWidth * Assets.Scripts.Constants.Constants.Scale;
    }

    private static float GetPowerLineHeight(PowerLineTypeEnum type)
    {
        return PowerLinesConstants.defaultHeight * Assets.Scripts.Constants.Constants.Scale;
    }

    private static Color GetPowerLineColor(PowerLineTypeEnum type)
    {
        return PowerLinesConstants.defaultColor;
    }
    public static GameObject CreatePowerLine(UnityPowerLine powerline, OsmBounds bounds)
    {
        GameObject result = new GameObject();
        result.name = "<powerline>";


        Vector3[] linePoints = new Vector3[powerline.PowerLinePoints.Count];

        for (int i = 0; i < powerline.PowerLinePoints.Count; i++)
        {
            linePoints[i] = UnityDataConverters.GetPointFromUnityPoint(powerline.PowerLinePoints[i], bounds);
            linePoints[i].y = GetPowerLineHeight(powerline.PowerLineType);
        }
        LineRenderer lineRender = result.AddComponent<LineRenderer>();
        lineRender.positionCount = powerline.PowerLinePoints.Count;

        lineRender.material = new Material(Shader.Find("Sprites/Default"));

        float width = GetPowerLineWidth(powerline.PowerLineType);
        Color color = GetPowerLineColor(powerline.PowerLineType);

        lineRender.startWidth = width;
        lineRender.endWidth = width;

        lineRender.startColor = color;
        lineRender.endColor = color;

        lineRender.SetPositions(linePoints);

        return result;
    }

    public static void AddPowerLinesToGameObject(IEnumerable<IUnityModel> powerLineList, Transform parent, OsmBounds bounds)
    {
        foreach (UnityPowerLine powerLine in powerLineList)
        {
            if (powerLine != null && powerLine.PowerLinePoints.Count > 1)
            {
                GameObject newHighway = CreatePowerLine(powerLine, bounds);
                newHighway.transform.SetParent(parent);
            }
        }
    }
}
