using OSMtoSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class DataManager : MonoBehaviour
{
    string fileName = "map.osm";
    double minLon = -100;
    double minLat = -100;
    double maxLon = 100;
    double maxLat = 100;
    public Text debugConsoleText;

    private bool dataManagerFinished;

    private bool dataParserStarted;
    private bool dataParserFinished;

    private bool gettinHighwayDataStarted;
    private bool gettinHighwayDataFinished;

    private bool gettinBuildingsDataStarted;
    private bool gettinBuildingsDataFinished;

    private bool gettinRailwayDataStarted;
    private bool gettinRailwayDataFinished;

    private object dataParserLock;
    private object highwayDataLock;
    private object railwayDataLock;
    private object buildingsDataLock;

    OsmBounds bounds;
    IEnumerable<IUnityModel> highwaysData;
    IEnumerable<IUnityModel> buildingsData;
    IEnumerable<IUnityModel> railwaysData;

    OsmDataManager osmDataManager;

    void Start()
    {
        dataParserLock = new object();
        highwayDataLock = new object();
        railwayDataLock = new object();
        buildingsDataLock = new object();


        dataParserStarted = false;
        dataParserFinished = false;

        gettinHighwayDataStarted = false;
        gettinHighwayDataFinished = false;

        gettinBuildingsDataStarted = false;
        gettinBuildingsDataFinished = false;

        gettinRailwayDataStarted = false;
        gettinRailwayDataFinished = false;
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

                    debugConsoleText.text += String.Format("Loading data - {0} \r\n", DateTime.Now.TimeOfDay);
                    Canvas.ForceUpdateCanvases();

                    GetOsmDataManager();
                }
                else
                {
                    lock (dataParserLock)
                    {
                        if (osmDataManager != null)
                        {
                            dataParserFinished = true;
                        }

                    }
                }
            }
            else if (!gettinHighwayDataFinished)
            {
                if (!gettinHighwayDataStarted)
                {
                    gettinHighwayDataStarted = true;

                    debugConsoleText.text += String.Format("gettinHighwayDataStarted - {0}\r\n", DateTime.Now.TimeOfDay);
                    Canvas.ForceUpdateCanvases();

                    GetHighwayData();
                }
                else
                {
                    lock (highwayDataLock)
                    {
                        if (highwaysData != null)
                        {
                            gettinHighwayDataFinished = true;
                        }

                    }
                }
            }
            else if (!gettinRailwayDataFinished)
            {
                if (!gettinRailwayDataStarted)
                {
                    gettinRailwayDataStarted = true;

                    debugConsoleText.text += String.Format("gettinRailwayDataStarted - {0}\r\n", DateTime.Now.TimeOfDay);
                    Canvas.ForceUpdateCanvases();

                    GetRailwayData();

                    AddHighwaysToScene();
                }
                else
                {
                    lock (railwayDataLock)
                    {
                        if (railwaysData != null)
                        {
                            gettinRailwayDataFinished = true;
                        }

                    }
                }
            }
            else if (!gettinBuildingsDataFinished)
            {
                if (!gettinBuildingsDataStarted)
                {
                    gettinBuildingsDataStarted = true;

                    debugConsoleText.text += String.Format("gettinBuildingsDataStarted - {0}\r\n", DateTime.Now.TimeOfDay);
                    Canvas.ForceUpdateCanvases();

                    GetBuildingsData();

                    AddRailwaysToScene();
                }
                else
                {
                    lock (buildingsDataLock)
                    {
                        if (buildingsData != null)
                        {
                            gettinBuildingsDataFinished = true;
                            AddBuildingsToScene();

                            dataManagerFinished = true;
                        }

                    }
                }
            }
        }
    }

    private void GetOsmDataManager()
    {
        new Thread(delegate ()
        {
            OsmData osmData = OsmParser.GetDataFromOSM(fileName, minLon, minLat, maxLon, maxLat);
            OsmDataManager tmpOsmDataManager = new OsmDataManager(osmData);

            lock (dataParserLock)
            {
                bounds = osmData.bounds;
                osmDataManager = tmpOsmDataManager;
            }
        }).Start();
    }

    private void GetHighwayData()
    {
        new Thread(delegate ()
        {
            IEnumerable<IUnityModel> tmpHighwaysData = osmDataManager.GetHighways(Enum.GetValues(typeof(HighwayTypeEnum)) as HighwayTypeEnum[]);

            lock (highwayDataLock)
            {
                highwaysData = tmpHighwaysData;
            }
        }).Start();
    }

    private void GetRailwayData()
    {
        new Thread(delegate ()
        {
            IEnumerable<IUnityModel> tmpRailwayData = osmDataManager.GetRailways();

            lock (railwayDataLock)
            {
                railwaysData = tmpRailwayData;
            }
        }).Start();
    }

    private void GetBuildingsData()
    {
        new Thread(delegate ()
        {
            IEnumerable<IUnityModel> tmpBuildingsData = osmDataManager.GetBuildings();

            lock (buildingsDataLock)
            {
                buildingsData = tmpBuildingsData;
            }
        }).Start();
    }

    private void AddHighwaysToScene()
    {
        HighwaysGenerator.AddHighwaysToGameObject(highwaysData, transform, bounds);
        debugConsoleText.text += String.Format("addHighwaysToSceneFinished - {0}\r\n", DateTime.Now.TimeOfDay);
        Canvas.ForceUpdateCanvases();
    }

    private void AddRailwaysToScene()
    {
        RailwaysGenerator.AddRailwayToGameObject(railwaysData, transform, bounds);
        debugConsoleText.text += String.Format("addRailwaysToSceneFinished - {0}\r\n", DateTime.Now.TimeOfDay);
        Canvas.ForceUpdateCanvases();
    }

    private void AddBuildingsToScene()
    {

        //debugConsoleText.text += String.Format("addBuildingsToSceneFinished - {0}\r\n", DateTime.Now.TimeOfDay);
        //Canvas.ForceUpdateCanvases();
    }
}