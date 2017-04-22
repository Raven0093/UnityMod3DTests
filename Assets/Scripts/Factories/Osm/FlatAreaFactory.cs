﻿using OSMtoSharp.Enums.Keys;
using OSMtoSharp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Factories.Osm
{
    public class FlatAreaFactory
    {
        public static void CreateArea(OsmWay areaData, OsmBounds bounds, Transform parent)
        {
            Material material = null;
            if (areaData.Tags.ContainsKey(TagKeyEnum.Landuse))
            {
                Enums.LanduseTypeEnum type = OSMtoSharp.Enums.Helpers.EnumExtensions.GetTagKeyEnum<Enums.LanduseTypeEnum>(areaData.Tags[TagKeyEnum.Landuse]);
                if (type != Enums.LanduseTypeEnum.None)
                {
                    string materiaName = string.Format("{0}_{1}",
                        OSMtoSharp.Enums.Helpers.EnumExtensions.GetEnumAttributeValue(TagKeyEnum.Landuse),
                        OSMtoSharp.Enums.Helpers.EnumExtensions.GetEnumAttributeValue(type));

                    material = Resources.Load(materiaName, typeof(Material)) as Material;
                }
            }
            if (areaData.Tags.ContainsKey(TagKeyEnum.Leisure))
            {
                Enums.LeisureTypeEnum type = OSMtoSharp.Enums.Helpers.EnumExtensions.GetTagKeyEnum<Enums.LeisureTypeEnum>(areaData.Tags[TagKeyEnum.Leisure]);
                if (type != Enums.LeisureTypeEnum.None)
                {
                    string materiaName = string.Format("{0}_{1}",
                        OSMtoSharp.Enums.Helpers.EnumExtensions.GetEnumAttributeValue(TagKeyEnum.Leisure),
                        OSMtoSharp.Enums.Helpers.EnumExtensions.GetEnumAttributeValue(type));

                    material = Resources.Load(materiaName, typeof(Material)) as Material;
                }
            }

            if (material != null)
            {
                GameObject result = MeshFactory.CreateMesh(areaData, bounds);
                if (areaData.Tags.ContainsKey(TagKeyEnum.Name))
                {
                    result.name = areaData.Tags[TagKeyEnum.Name];
                }
                else
                {
                    result.name = "landuse";
                }

                result.transform.parent = parent;
            }


        }
    }
}
