using OSMtoSharp;
using OSMtoSharp.FileManagers;
using OSMtoSharp.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using OSMtoSharp.Enums.Keys;
using System.IO;

public class DataManager : MonoBehaviour
{

    OsmData osmData;

    private object osmDataLock;

    private bool dataManagerFinished;
    private bool dataParserStarted;
    private bool dataParserFinished;


    string fileName = "C:\\Users\\Public\\Documents\\Unity Projects\\Tests\\Files\\Gliwice2.osm";
    double minLon = -1000;
    double minLat = -1000;
    double maxLon = 1000;
    double maxLat = 1000;
    public Text debugConsoleText;

    private string DebugConsoleText
    {
        get
        {
            return debugConsoleText.text;
        }
        set
        {
            debugConsoleText.text = value;
            Canvas.ForceUpdateCanvases();
        }
    }

    private object dataParserLock;

    void Start()
    {
        osmDataLock = new object();
        dataParserLock = new object();

        dataParserStarted = false;
        dataParserFinished = false;

        dataManagerFinished = false;

        osmData = null;
        if (!LoadConfigFile())
        {
            dataManagerFinished = true;
        }
    }

    void Update()
    {
        if (!dataManagerFinished)
        {
            if (!dataParserFinished)
            {
                if (!dataParserStarted)
                {
                    dataParserStarted = true;

                    DebugConsoleText += String.Format("Data loading - {0} \r\n", DateTime.Now.TimeOfDay);

                    GetOsmData();
                }
                else
                {
                    lock (dataParserLock)
                    {
                        if (osmData != null)
                        {
                            DebugConsoleText += String.Format("Data loaded - {0} \r\n", DateTime.Now.TimeOfDay);

                            osmData.FillWaysNode();
                            osmData.RemoveNodesWithoutTags();
                            osmData.RemoveRelationsWithoutMembers();
                            osmData.RemoveWaysWithoutNodes();

                            dataParserFinished = true;
                        }
                    }
                }
            }
            else
            {
                AddDataToScene();
                dataManagerFinished = true;
            }
        }
    }


    public bool LoadConfigFile()
    {
        bool fileNameBool = false;
        bool minLonBool = false;
        bool minLatBool = false;
        bool maxLonBool = false;
        bool maxLatBool = false;

        if (!File.Exists(Assets.Scripts.Constants.Constants.ConfigFile))
        {
            DebugConsoleText += String.Format("Config file not exists - {0} \r\n", DateTime.Now.TimeOfDay);
            return false;
        }
        using (StreamReader inputile = new StreamReader(Assets.Scripts.Constants.Constants.ConfigFile))
        {
            try
            {
                while (!inputile.EndOfStream)
                {

                    var a = inputile.ReadLine();
                    var b = a.Split('=');
                    if (b[0] == "Path")
                    {
                        fileName = b[1];
                        fileNameBool = true;
                    }
                    else if (b[0] == "MinLat")
                    {
                        minLat = double.Parse(b[1]);
                        minLatBool = true;
                    }
                    else if (b[0] == "MinLon")
                    {
                        minLon = double.Parse(b[1]);
                        minLonBool = true;
                    }
                    else if (b[0] == "MaxLat")
                    {
                        maxLat = double.Parse(b[1]);
                        maxLatBool = true;
                    }
                    else if (b[0] == "MaxLon")
                    {
                        maxLon = double.Parse(b[1]);
                        maxLonBool = true;
                    }
                    else
                    {

                    }
                }
            }
            catch (Exception)
            {
                DebugConsoleText += String.Format("Config file is empty - {0} \r\n", DateTime.Now.TimeOfDay);
                return false;
            }


            if (fileNameBool && minLonBool && minLatBool && maxLonBool && maxLatBool)
            {
                DebugConsoleText += String.Format("Load config data - {0} \r\n", DateTime.Now.TimeOfDay);
                DebugConsoleText += String.Format("File path - {0} \r\n", fileName);
                DebugConsoleText += String.Format("MinLat - {0} \r\n", minLat);
                DebugConsoleText += String.Format("MinLon - {0} \r\n", minLon);
                DebugConsoleText += String.Format("MaxLat - {0} \r\n", maxLat);
                DebugConsoleText += String.Format("MaxLon - {0} \r\n", maxLon);
                return true;
            }
            else
            {
                DebugConsoleText += String.Format("Config file is empty - {0} \r\n", DateTime.Now.TimeOfDay);
                return false;
            }

        }
    }
    private void AddDataToScene()
    {
        GameObject highways = new GameObject();
        highways.name = "highways";
        highways.transform.parent = transform;
        GameObject powerLines = new GameObject();
        powerLines.name = "powerLines";
        powerLines.transform.parent = transform;
        GameObject railways = new GameObject();
        railways.name = "railways";
        railways.transform.parent = transform;
        GameObject buildings = new GameObject();
        buildings.name = "buildings";
        buildings.transform.parent = transform;
        GameObject powerTowers = new GameObject();
        powerTowers.name = "powerTowers";
        powerTowers.transform.parent = transform;
        GameObject flatArea = new GameObject();
        flatArea.name = "landuses";
        flatArea.transform.parent = transform;


        foreach (var nodeDic in osmData.Nodes)
        {
            if (nodeDic.Value.Tags.ContainsKey(TagKeyEnum.Power))
            {
                //TODO - Add Power tower
                Assets.Scripts.Factories.Osm.PowerFactory.CreatePower(nodeDic.Value, osmData.bounds, powerTowers.transform);
                continue;
            }

        }

        foreach (var wayDic in osmData.Ways)
        {
            if (wayDic.Value.Tags.ContainsKey(TagKeyEnum.Power))
            {
                //TODO - Add Power line
                Assets.Scripts.Factories.Osm.PowerFactory.CreatePower(wayDic.Value, osmData.bounds, powerLines.transform);
                continue;
            }
            if (wayDic.Value.Tags.ContainsKey(TagKeyEnum.Building))
            {
                //TODO - Add Building line
                //OSMtoSharp.Enums.Values.BuildingsTypeEnum type = OSMtoSharp.Enums.Helpers.EnumExtensions.
                //                                                   GetTagKeyEnum<OSMtoSharp.Enums.Values.BuildingsTypeEnum>
                //                                                   (wayDic.Value.Tags[TagKeyEnum.Building]);
                //if (type == OSMtoSharp.Enums.Values.BuildingsTypeEnum.Residential)
                Assets.Scripts.Factories.Osm.BuildingFactory.CreateBuilding(wayDic.Value, osmData.bounds, buildings.transform);
                continue;
            }
            if (wayDic.Value.Tags.ContainsKey(TagKeyEnum.Highway))
            {
                //TODO - Add HighWay line
                Assets.Scripts.Factories.Osm.HighwayFactory.CreateHighway(wayDic.Value, osmData.bounds, highways.transform);
                continue;
            }
            if (wayDic.Value.Tags.ContainsKey(TagKeyEnum.Railway))
            {
                //TODO - Add Railway line
                Assets.Scripts.Factories.Osm.RailWaysFactory.CreateRailway(wayDic.Value, osmData.bounds, railways.transform);
                continue;
            }
            if (wayDic.Value.Tags.ContainsKey(TagKeyEnum.Landuse) || wayDic.Value.Tags.ContainsKey(TagKeyEnum.Leisure) || wayDic.Value.Tags.ContainsKey(TagKeyEnum.Amenity))
            {
                //TODO - Add Railway line
                Assets.Scripts.Factories.Osm.FlatAreaFactory.CreateArea(wayDic.Value, osmData.bounds, flatArea.transform);
                continue;
            }
        }
    }


    private void GetOsmData()
    {
        new Thread(delegate ()
        {
            OsmData tmpOsmData = OsmParser.Parse(fileName, minLon, minLat, maxLon, maxLat);

            lock (osmDataLock)
            {
                osmData = tmpOsmData;
            }
        }).Start();
    }
}