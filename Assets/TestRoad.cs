using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OSMtoSharp;
using System;
using System.Threading;
using UnityEngine.UI;

public class TestRoad : MonoBehaviour
{
    public GameObject line;
    public Text text;
    //string fileName = "18.27758_50.28279_18.69443_50.30892.xml";
    //string fileName = "9.3542_47.02903_9.7511_47.3323.xml";
    string fileName = "map.osm";
    double minLon = -100;
    double minLat = -100;
    double maxLon = 100;
    double maxLat = 100;


    OsmBounds bounds;
    //public float minLong = 18.5484f;
    //public float minLat = 50.2426f;
    //public float maxLong = 18.8179f;
    //public float maxLat = 50.3354f;


    IEnumerable<IUnityModel> highwaysData;
    OsmDataManager osmDataManager;
    private bool gettingDataManagerFinished;
    private bool gettingDataManagerStarted;
    private bool gettinHighwayDataFinished;
    private bool gettinHighwayDataStarted;
    private bool addHighwaysToSceneFinished;
    private object dataManagerLock;
    private object highwayDataLock;

    private void GetOsmDataManager()
    {
        new Thread(delegate ()
       {
           OsmData osmData = OsmParser.GetDataFromOSM(fileName, minLon, minLat, maxLon, maxLat);
           OsmDataManager tmpOsmDataManager = new OsmDataManager(osmData);

           lock (dataManagerLock)
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
            //IEnumerable<IUnityModel> tmpHighwaysData = osmDataManager.GetHighways(Enum.GetValues(typeof(HighwayTypeEnum)) as HighwayTypeEnum[]);

            IEnumerable<IUnityModel> tmpHighwaysData = osmDataManager.GetBuilding();
            lock (highwayDataLock)
            {
                highwaysData = tmpHighwaysData;
            }
        }).Start();

    }

    private void AddHighwaysToScene()
    {

        foreach (UnityWay item in highwaysData)
        {
            if (item.HighwayPoints.Count > 1)
            {
                GameObject newLine = Instantiate(line);
                newLine.transform.SetParent(transform);
                newLine.name = item.Name;

                Vector3[] linePoints = new Vector3[item.HighwayPoints.Count];

                for (int i = 0; i < item.HighwayPoints.Count; i++)
                {
                    float multipler = 10000;
                    float x = (item.HighwayPoints[i].Lat - (float)bounds.MinLat) * multipler;
                    float y = 0;
                    float z = (item.HighwayPoints[i].Lon - (float)bounds.MinLon) * multipler;

                    linePoints[i] = new Vector3(x, y, z);
                }
                LineRenderer lineRender = newLine.GetComponent<LineRenderer>();
                lineRender.numPositions = item.HighwayPoints.Count;
                lineRender.startWidth = 0.3f;
                lineRender.endWidth = 0.3f;
                lineRender.SetPositions(linePoints);
            }


        }
        Debug.Log(String.Format("addHighwaysToSceneFinished - {0}", DateTime.Now.TimeOfDay));
        text.text += String.Format("addHighwaysToSceneFinished - {0}\r\n", DateTime.Now.TimeOfDay);
        Canvas.ForceUpdateCanvases();
    }

    private void AddBuildingsToScene()
    {

        foreach (UnityBuilding item in highwaysData)
        {
            if (item.BuildingPoints.Count > 1)
            {
                GameObject newLine = Instantiate(line);
                newLine.transform.SetParent(transform);
                newLine.name = item.Name;

                Vector3[] linePoints = new Vector3[item.BuildingPoints.Count];

                for (int i = 0; i < item.BuildingPoints.Count; i++)
                {
                    float multipler = 10000;
                    float x = (item.BuildingPoints[i].Lat - (float)bounds.MinLat) * multipler;
                    float y = 0;
                    float z = (item.BuildingPoints[i].Lon - (float)bounds.MinLon) * multipler;

                    linePoints[i] = new Vector3(x, y, z);
                }
                LineRenderer lineRender = newLine.GetComponent<LineRenderer>();
                lineRender.numPositions = item.BuildingPoints.Count;
                lineRender.startWidth = 0.3f;
                lineRender.endWidth = 0.3f;
                lineRender.SetPositions(linePoints);
            }


        }
        Debug.Log(String.Format("addHighwaysToSceneFinished - {0}", DateTime.Now.TimeOfDay));
        text.text += String.Format("addHighwaysToSceneFinished - {0}\r\n", DateTime.Now.TimeOfDay);
        Canvas.ForceUpdateCanvases();
    }

    void Start()
    {
        dataManagerLock = new object();
        highwayDataLock = new object();
    }

    void Update()
    {
        if (!gettingDataManagerFinished)
        {
            if (!gettingDataManagerStarted)
            {
                gettingDataManagerStarted = true;
                Debug.Log(String.Format("gettingDataManagerStarted - {0}", DateTime.Now.TimeOfDay));
                text.text += String.Format("Loading data:\r\n");
                //text.text += String.Format("minLong - {0}\r\n", bounds.MinLon);
                //text.text += String.Format("minLat - {0}\r\n", bounds.MinLat);
                //text.text += String.Format("maxLong - {0}\r\n", bounds.MaxLon);
                //text.text += String.Format("maxLat - {0}\r\n", bounds.MaxLat);
                text.text += String.Format("gettingDataManagerStarted - {0}\r\n", DateTime.Now.TimeOfDay);
                Canvas.ForceUpdateCanvases();
                GetOsmDataManager();
            }
            else
            {
                lock (dataManagerLock)
                {
                    if (osmDataManager != null)
                    {
                        gettingDataManagerFinished = true;
                    }

                }
            }
        }
        else if (!gettinHighwayDataFinished)
        {
            if (!gettinHighwayDataStarted)
            {
                gettinHighwayDataStarted = true;
                Debug.Log(String.Format("gettinHighwayDataStarted - {0}", DateTime.Now.TimeOfDay));
                text.text += String.Format("gettinHighwayDataStarted - {0}\r\n", DateTime.Now.TimeOfDay);
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
        else if (!addHighwaysToSceneFinished)
        {
            Debug.Log(String.Format("addHighwaysToSceneStarting - {0}", DateTime.Now.TimeOfDay));
            text.text += String.Format("addHighwaysToSceneStarting - {0}\r\n", DateTime.Now.TimeOfDay);
            Canvas.ForceUpdateCanvases();
            addHighwaysToSceneFinished = true;
            //AddHighwaysToScene();
            AddBuildingsToScene();
        }

    }
}
//float minLong = 18.67758f;
//float minLat = 50.28279f;
//float maxLong = 18.69443f;
//float maxLat = 50.28892f;

//float minLong = -0.1353000f;
//float minLat = 51.5019000f;
//float maxLong = -0.1016000f;
//float maxLat = 51.5132000f;
