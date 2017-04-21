using OSMtoSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class DataManager : MonoBehaviour
{
    string fileName = "C:\\Users\\Public\\Documents\\Unity Projects\\Tests\\Files\\Gliwice.osm";
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

    private bool gettinPowerLinesDataStarted;
    private bool gettinPowerLinesDataFinished;

    private bool gettinPowerTowersDataStarted;
    private bool gettinPowerTowersDataFinished;

    private object dataParserLock;
    private object highwayDataLock;
    private object railwayDataLock;
    private object buildingsDataLock;
    private object powerLinesDataLock;
    private object powerTowersDataLock;

    OsmBounds bounds;
    IEnumerable<IUnityModel> highwaysData;
    IEnumerable<IUnityModel> buildingsData;
    IEnumerable<IUnityModel> railwaysData;
    IEnumerable<IUnityModel> powerLinesData;
    IEnumerable<IUnityModel> powerTowersData;
    OsmDataManager osmDataManager;

    void Start()
    {
        dataParserLock = new object();
        highwayDataLock = new object();
        railwayDataLock = new object();
        buildingsDataLock = new object();
        powerLinesDataLock = new object();
        powerTowersDataLock = new object();

        dataParserStarted = false;
        dataParserFinished = false;

        gettinHighwayDataStarted = false;
        gettinHighwayDataFinished = false;

        gettinBuildingsDataStarted = false;
        gettinBuildingsDataFinished = false;

        gettinRailwayDataStarted = false;
        gettinRailwayDataFinished = false;

        gettinPowerLinesDataStarted = false;
        gettinPowerLinesDataFinished = false;


        gettinPowerTowersDataStarted = false;
        gettinPowerTowersDataFinished = false;
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
            else
            {
                if (!gettinHighwayDataFinished)
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
                                AddHighwaysToScene();
                            }

                        }
                    }
                }

                if (!gettinRailwayDataFinished && gettinHighwayDataFinished)
                {
                    if (!gettinRailwayDataStarted)
                    {
                        gettinRailwayDataStarted = true;

                        debugConsoleText.text += String.Format("gettinRailwayDataStarted - {0}\r\n", DateTime.Now.TimeOfDay);
                        Canvas.ForceUpdateCanvases();

                        GetRailwayData();
                    }
                    else
                    {
                        lock (railwayDataLock)
                        {
                            if (railwaysData != null)
                            {
                                gettinRailwayDataFinished = true;
                                AddRailwaysToScene();
                            }

                        }
                    }
                }

                if (!gettinBuildingsDataFinished && gettinRailwayDataFinished)
                {
                    if (!gettinBuildingsDataStarted)
                    {
                        gettinBuildingsDataStarted = true;

                        debugConsoleText.text += String.Format("gettinBuildingsDataStarted - {0}\r\n", DateTime.Now.TimeOfDay);
                        Canvas.ForceUpdateCanvases();

                        GetBuildingsData();
                    }
                    else
                    {
                        lock (buildingsDataLock)
                        {
                            if (buildingsData != null)
                            {
                                gettinBuildingsDataFinished = true;

                                AddBuildingsToScene();
                            }

                        }
                    }
                }

                if (!gettinPowerLinesDataFinished && gettinBuildingsDataFinished)
                {
                    if (!gettinPowerLinesDataStarted)
                    {
                        gettinPowerLinesDataStarted = true;

                        debugConsoleText.text += String.Format("gettinPowerLinesDataStarted - {0}\r\n", DateTime.Now.TimeOfDay);
                        Canvas.ForceUpdateCanvases();

                        GetPowerLinesData();
                    }
                    else
                    {
                        lock (powerLinesDataLock)
                        {
                            if (powerLinesData != null)
                            {
                                gettinPowerLinesDataFinished = true;
                                AddPowerLinesToScene();
                            }

                        }
                    }
                }

                if (!gettinPowerTowersDataFinished && gettinPowerLinesDataFinished)
                {
                    if (!gettinPowerTowersDataStarted)
                    {
                        gettinPowerTowersDataStarted = true;

                        debugConsoleText.text += String.Format("gettinPowerTowersDataStarted - {0}\r\n", DateTime.Now.TimeOfDay);
                        Canvas.ForceUpdateCanvases();

                        GetPowerTowersData();
                    }
                    else
                    {
                        lock (powerLinesDataLock)
                        {
                            if (powerLinesData != null)
                            {
                                gettinPowerTowersDataFinished = true;
                                AddPowerTowersToScene();
                            }

                        }
                    }
                }

                if (gettinHighwayDataFinished && gettinRailwayDataFinished && gettinBuildingsDataFinished && gettinPowerLinesDataFinished && gettinPowerTowersDataFinished)
                {
                    dataManagerFinished = true;
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
            IEnumerable<IUnityModel> tmpHighwaysData = osmDataManager.GetHighways(true, Enum.GetValues(typeof(HighwayTypeEnum)) as HighwayTypeEnum[]);

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
            IEnumerable<IUnityModel> tmpRailwayData = osmDataManager.GetRailways(true);

            lock (railwayDataLock)
            {
                railwaysData = tmpRailwayData;
            }
        }).Start();
    }

    private void GetPowerLinesData()
    {
        new Thread(delegate ()
        {
            IEnumerable<IUnityModel> tmpPowerLinesData = osmDataManager.GetPowerLines(true);

            lock (powerLinesDataLock)
            {
                powerLinesData = tmpPowerLinesData;
            }
        }).Start();
    }

    private void GetBuildingsData()
    {
        new Thread(delegate ()
        {
            IEnumerable<IUnityModel> tmpBuildingsData = osmDataManager.GetBuildings(true, Enum.GetValues(typeof(BuildingsTypeEnum)) as BuildingsTypeEnum[]);

            lock (buildingsDataLock)
            {
                buildingsData = tmpBuildingsData;
            }
        }).Start();
    }

    private void GetPowerTowersData()
    {
        new Thread(delegate ()
        {
            IEnumerable<IUnityModel> tmpPowerTowersData = osmDataManager.GetPowerTowers();

            lock (powerTowersDataLock)
            {
                powerTowersData = tmpPowerTowersData;
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
        BuildingsGenerator.AddBuildingsToGameObject(buildingsData, transform, bounds);
        debugConsoleText.text += String.Format("addBuildingsToSceneFinished - {0}\r\n", DateTime.Now.TimeOfDay);
        Canvas.ForceUpdateCanvases();
    }

    private void AddPowerLinesToScene()
    {
        PowerLinesGenerator.AddPowerLinesToGameObject(powerLinesData, transform, bounds);
        debugConsoleText.text += String.Format("addPowerLinesToSceneFinished - {0}\r\n", DateTime.Now.TimeOfDay);
        Canvas.ForceUpdateCanvases();
    }


    private void AddPowerTowersToScene()
    {
        PowerTowersGenerator.AddPowerTowersToGameObject(powerTowersData, transform, bounds);
        debugConsoleText.text += String.Format("addPowerTowersToSceneFinished - {0}\r\n", DateTime.Now.TimeOfDay);
        Canvas.ForceUpdateCanvases();
    }
}