using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Helpers;
using UnityEngine;
using OSMtoSharp;
using OSMtoSharp.Model;
using OSMtoSharp.Enums.Values;
using OSMtoSharp.Enums.Keys;
using System.Globalization;
using Assets.Scripts.Factories.Helpers;
using Assets.Scripts.Enums;

namespace Assets.Scripts.Factories.Osm
{
    public class BuildingFactory : MonoBehaviour
    {
        private static class BuildingsConstants
        {
            public const float wallHeight = 2f;
            public const float wallWidth = 0.0001f;
        }

        public static float GetWallHeight(BuildingsTypeEnum type, int level)
        {
            return BuildingsConstants.wallHeight * Assets.Scripts.Constants.Constants.Scale * level;
        }

        public static void CreateRoof(List<OsmNode> nodes, OsmBounds bounds, float height, Transform parent)
        {
            var buildingCorners = new List<Vector3>();


            for (int i = 0; i < nodes.Count - 1; i++)
            {
                buildingCorners.Add(OsmToUnityConverter.GetPointFromUnityPointVec3(nodes[i].Point, bounds));
            }

            GameObject roof = new GameObject();
            roof.AddComponent<MeshRenderer>();
            roof.AddComponent<MeshFilter>().mesh = MeshFactory.CreateMesh(buildingCorners);
            roof.transform.localPosition = new Vector3(roof.transform.localPosition.x, height, roof.transform.localPosition.z);
            roof.name = "roof";

            roof.transform.parent = parent;
        }

        public static GameObject CreateWall(Vector3 pointA, Vector3 pointB, float width, float height)
        {
            GameObject newWall = GameObject.CreatePrimitive(PrimitiveType.Cube);

            newWall.name = "Wall";

            Vector3 between = pointB - pointA;
            float distance = between.magnitude;

            newWall.transform.localScale = new Vector3(width, height, distance);
            newWall.transform.position = pointA + (between / 2.0f);
            newWall.transform.LookAt(pointB);
            newWall.transform.position = new Vector3(newWall.transform.position.x, height / 2, newWall.transform.position.z);

            return newWall;
        }

        public static void CreateWalls(List<OsmNode> nodes, OsmBounds bounds, float width, float levels, float height, float minHeight, Transform parent)
        {
            for (int i = 1; i < nodes.Count; i++)
            {
                Vector2 pointA = OsmToUnityConverter.GetPointFromUnityPointVec2(nodes[i].Point, bounds);
                Vector2 pointB = OsmToUnityConverter.GetPointFromUnityPointVec2(nodes[i - 1].Point, bounds);

                GameObject wall = CreateWall(new Vector3(pointA.x, minHeight, pointA.y),
                                               new Vector3(pointB.x, minHeight, pointB.y),
                                               width, height);


                Material material = new Material(Resources.Load("Wall", typeof(Material)) as Material);

                material.mainTextureScale = new Vector2((int)wall.transform.localScale.z, levels);

                wall.GetComponent<Renderer>().material = material;

                wall.transform.SetParent(parent);
            }

        }

        public static void CreateBuilding(OsmWay buildingData, OsmBounds bounds, Transform parent)
        {
            if (buildingData.Nodes.Count < 3)
            {
                return;
            }
            GameObject result = new GameObject();

            float levels = 0;
            float height = 0;
            float minHeight = 0;
            if (buildingData.Tags.ContainsKey(TagKeyEnum.Name))
            {
                result.name = buildingData.Tags[TagKeyEnum.Name];
            }
            else
            {
                result.name = "<building>";
            }

            if (buildingData.Tags.ContainsKey(TagKeyEnum.BuildingLevels))
            {
                levels += float.Parse(buildingData.Tags[TagKeyEnum.BuildingLevels], CultureInfo.InvariantCulture);
                height = levels * BuildingsConstants.wallHeight;
            }
            if (buildingData.Tags.ContainsKey(TagKeyEnum.Min_level))
            {
                levels -= float.Parse(buildingData.Tags[TagKeyEnum.BuildingLevels], CultureInfo.InvariantCulture);
                minHeight = BuildingsConstants.wallHeight;
            }

            if (buildingData.Tags.ContainsKey(TagKeyEnum.Height))
            {
                height = float.Parse(buildingData.Tags[TagKeyEnum.Height].Replace(" ", "").Replace("m", ""), CultureInfo.InvariantCulture);
            }

            if (buildingData.Tags.ContainsKey(TagKeyEnum.MinHeight))
            {
                minHeight = float.Parse(buildingData.Tags[TagKeyEnum.MinHeight], CultureInfo.InvariantCulture);
            }
            if (levels == 0)
            {
                levels = 1;
                height = BuildingsConstants.wallHeight;
            }

            BuildingsTypeEnum type = OSMtoSharp.Enums.Helpers.EnumExtensions.
                                                                   GetTagKeyEnum<BuildingsTypeEnum>
                                                                   (buildingData.Tags[TagKeyEnum.Building]);

            CreateWalls(buildingData.Nodes, bounds, BuildingsConstants.wallWidth, levels, height, minHeight, result.transform);
            CreateRoof(buildingData.Nodes, bounds, height, result.transform);

            result.transform.parent = parent;
        }
    }
}
