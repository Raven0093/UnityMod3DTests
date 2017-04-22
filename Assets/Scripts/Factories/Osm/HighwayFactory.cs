using Assets.Scripts.Enums;
using Assets.Scripts.Factories.Helpers;
using Assets.Scripts.Factories.Unity;
using OSMtoSharp.Enums.Keys;
using OSMtoSharp.Enums.Values;
using OSMtoSharp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Factories.Osm
{
    public class HighwayFactory
    {
        private static class HighwaysConstants
        {
            public const float motorwayWidth = 1f;
            public const float trunkWidth = 0.9f;
            public const float primaryWidth = 0.8f;
            public const float secondaryWidth = 0.7f;
            public const float tertiaryWidth = 0.5f;
            public const float residentialWidth = 0.3f;
            public const float serviceWidth = 0.2f;
            public const float proposedWidth = 0.01f;
            public const float underConstruction = 0.01f;
            public const float cycleway = 0.005f;
            public const float defaultWidth = 0.1f;


            public static Color motorwayColor = new Color(232 / 255, 146f / 255f, 162f / 255f, 1);
            public static Color trunkColor = new Color(251f / 255f, 177f / 255f, 153f / 255f, 1);
            public static Color primaryColor = new Color(254f / 255f, 214f / 255f, 161f / 255f, 1);
            public static Color secondaryColor = new Color(246f / 255f, 250f / 255f, 186f / 255f, 1);
            public static Color cycleColor = new Color(150f / 255f, 150f / 255f, 150f / 255f, 1);
            public static Color defaultColor = new Color(255f / 255f, 255f / 255f, 255f / 255f, 1);

        }

        private static float GetHighwayWidth(HighwayTypeEnum type)
        {
            float result = 0;
            switch (type)
            {
                case HighwayTypeEnum.Motorway: result = HighwaysConstants.motorwayWidth; break;
                case HighwayTypeEnum.Trunk: result = HighwaysConstants.trunkWidth; break;
                case HighwayTypeEnum.Primary: result = HighwaysConstants.primaryWidth; break;
                case HighwayTypeEnum.Secondary: result = HighwaysConstants.secondaryWidth; break;
                case HighwayTypeEnum.Tertiary: result = HighwaysConstants.tertiaryWidth; break;
                case HighwayTypeEnum.Residential: result = HighwaysConstants.residentialWidth; break;
                case HighwayTypeEnum.Service: result = HighwaysConstants.serviceWidth; break;
                case HighwayTypeEnum.Construction: result = HighwaysConstants.proposedWidth; break;
                case HighwayTypeEnum.Proposed: result = HighwaysConstants.underConstruction; break;
                case HighwayTypeEnum.Cycleway: result = HighwaysConstants.cycleway; break;
                default: result = HighwaysConstants.defaultWidth; break;
            }
            return result * Assets.Scripts.Constants.Constants.Scale;

        }

        private static Color GetHighwayColor(HighwayTypeEnum type)
        {
            switch (type)
            {
                case HighwayTypeEnum.Motorway: return HighwaysConstants.motorwayColor;
                case HighwayTypeEnum.Trunk: return HighwaysConstants.trunkColor;
                case HighwayTypeEnum.Primary: return HighwaysConstants.primaryColor;
                case HighwayTypeEnum.Secondary: return HighwaysConstants.secondaryColor;
                case HighwayTypeEnum.Cycleway: return HighwaysConstants.cycleColor;
                default: return HighwaysConstants.defaultColor;
            }

        }

        public static GameObject CreateHighway(OsmWay highwayData, OsmBounds bounds, Transform parent)
        {
            Vector3[] linePoints = new Vector3[highwayData.Nodes.Count];

            for (int i = 0; i < highwayData.Nodes.Count; i++)
            {
                linePoints[i] = OsmToUnityConverter.GetPointFromUnityPointVec3(highwayData.Nodes[i].Point, bounds);
            }

            HighwayTypeEnum type = OSMtoSharp.Enums.Helpers.EnumExtensions.
                                                       GetTagKeyEnum<HighwayTypeEnum>
                                                       (highwayData.Tags[TagKeyEnum.Highway]);

            float width = GetHighwayWidth(type);
            Color color = GetHighwayColor(type);

            GameObject result = LineFactory.CreateLine(linePoints, width, color, new Material(Shader.Find("Sprites/Default")));
            if (highwayData.Tags.ContainsKey(TagKeyEnum.Name))
            {
                result.name = highwayData.Tags[TagKeyEnum.Name];
            }
            else
            {
                result.name = "<highway>";
            }

            result.transform.parent = parent;

            return result;
        }
    }
}