//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using OSMtoSharp;
//using System;
//using System.Threading;
//using UnityEngine.UI;

//public class TestRoad : MonoBehaviour
//{
//    public GameObject line;
//    public Text text;
//    //string fileName = "18.27758_50.28279_18.69443_50.30892.xml";
//    //string fileName = "9.3542_47.02903_9.7511_47.3323.xml";
//    string fileName = "map.osm";
//    double minLon = -100;
//    double minLat = -100;
//    double maxLon = 100;
//    double maxLat = 100;


//    OsmBounds bounds;
//    //public float minLong = 18.5484f;
//    //public float minLat = 50.2426f;
//    //public float maxLong = 18.8179f;
//    //public float maxLat = 50.3354f;


//    IEnumerable<IUnityModel> highwaysData;
//    IEnumerable<IUnityModel> highwaysData1;
//    OsmDataManager osmDataManager;
//    private bool gettingDataManagerFinished;
//    private bool gettingDataManagerStarted;
//    private bool gettinHighwayDataFinished;
//    private bool gettinHighwayDataStarted;
//    private bool addHighwaysToSceneFinished;
//    private object dataManagerLock;
//    private object highwayDataLock;

//    private void GetOsmDataManager()
//    {
//        new Thread(delegate ()
//       {
//           OsmData osmData = OsmParser.GetDataFromOSM(fileName, minLon, minLat, maxLon, maxLat);
//           OsmDataManager tmpOsmDataManager = new OsmDataManager(osmData);

//           lock (dataManagerLock)
//           {
//               bounds = osmData.bounds;
//               osmDataManager = tmpOsmDataManager;
//           }
//       }).Start();
//    }

//    private void GetHighwayData()
//    {
//        new Thread(delegate ()
//        {
//            IEnumerable<IUnityModel> tmpHighwaysData = osmDataManager.GetHighways(Enum.GetValues(typeof(HighwayTypeEnum)) as HighwayTypeEnum[]);

//            //IEnumerable<IUnityModel> tmpHighwaysData1 = osmDataManager.GetBuilding();
//            lock (highwayDataLock)
//            {
//                highwaysData = tmpHighwaysData;
//                //highwaysData1 = tmpHighwaysData1;
//            }
//        }).Start();

//    }

//    private void AddHighwaysToScene()
//    {
//        HighwaysGenerator.AddHighwaysToGameObject(highwaysData, transform, bounds);

//        Debug.Log(String.Format("addHighwaysToSceneFinished - {0}", DateTime.Now.TimeOfDay));
//        text.text += String.Format("addHighwaysToSceneFinished - {0}\r\n", DateTime.Now.TimeOfDay);
//        Canvas.ForceUpdateCanvases();
//    }

//    private void AddBuildingsToScene()
//    {

//        foreach (UnityBuilding item in highwaysData1)
//        {
//            GameObject newBuid = new GameObject();
//            newBuid.transform.SetParent(transform);
//            newBuid.name = item.Name;
//            if (item.BuildingPoints.Count > 1)
//            {
//                for (int i = 1; i < item.BuildingPoints.Count; i++)
//                {
//                    GameObject newLine = GameObject.CreatePrimitive(PrimitiveType.Cube);
//                    newLine.transform.SetParent(newBuid.transform);
//                    newLine.name = "Wal";

//                    float multipler = 10000;
//                    float x = (item.BuildingPoints[i].Lat - (float)bounds.MinLat) * multipler;
//                    float y = 0;
//                    float z = (item.BuildingPoints[i].Lon - (float)bounds.MinLon) * multipler;

//                    float x1 = (item.BuildingPoints[i - 1].Lat - (float)bounds.MinLat) * multipler;
//                    float y1 = 0;
//                    float z1 = (item.BuildingPoints[i - 1].Lon - (float)bounds.MinLon) * multipler;


//                    Vector3 pA = new Vector3(x, y, z);
//                    Vector3 pB = new Vector3(x1, y1, z1);
//                    Vector3 between = pB - pA;
//                    float distance = between.magnitude;
//                    newLine.transform.localScale = new Vector3(0.0001f, 3, distance); ;
//                    newLine.transform.position = pA + (between / 2.0f);
//                    newLine.transform.LookAt(pB);
//                }



//            }


//        }
//        Debug.Log(String.Format("addHighwaysToSceneFinished - {0}", DateTime.Now.TimeOfDay));
//        text.text += String.Format("addHighwaysToSceneFinished - {0}\r\n", DateTime.Now.TimeOfDay);
//        Canvas.ForceUpdateCanvases();
//    }

//    void Start()
//    {
//        dataManagerLock = new object();
//        highwayDataLock = new object();
//    }

//    void Update()
//    {
//        if (!gettingDataManagerFinished)
//        {
//            if (!gettingDataManagerStarted)
//            {
//                gettingDataManagerStarted = true;
//                Debug.Log(String.Format("gettingDataManagerStarted - {0}", DateTime.Now.TimeOfDay));
//                text.text += String.Format("Loading data:\r\n");
//                //text.text += String.Format("minLong - {0}\r\n", bounds.MinLon);
//                //text.text += String.Format("minLat - {0}\r\n", bounds.MinLat);
//                //text.text += String.Format("maxLong - {0}\r\n", bounds.MaxLon);
//                //text.text += String.Format("maxLat - {0}\r\n", bounds.MaxLat);
//                text.text += String.Format("gettingDataManagerStarted - {0}\r\n", DateTime.Now.TimeOfDay);
//                Canvas.ForceUpdateCanvases();
//                GetOsmDataManager();
//            }
//            else
//            {
//                lock (dataManagerLock)
//                {
//                    if (osmDataManager != null)
//                    {
//                        gettingDataManagerFinished = true;
//                    }

//                }
//            }
//        }
//        else if (!gettinHighwayDataFinished)
//        {
//            if (!gettinHighwayDataStarted)
//            {
//                gettinHighwayDataStarted = true;
//                Debug.Log(String.Format("gettinHighwayDataStarted - {0}", DateTime.Now.TimeOfDay));
//                text.text += String.Format("gettinHighwayDataStarted - {0}\r\n", DateTime.Now.TimeOfDay);
//                Canvas.ForceUpdateCanvases();
//                GetHighwayData();
//            }
//            else
//            {
//                lock (highwayDataLock)
//                {
//                    if (highwaysData != null)
//                    {
//                        gettinHighwayDataFinished = true;
//                    }

//                }
//            }
//        }
//        else if (!addHighwaysToSceneFinished)
//        {
//            Debug.Log(String.Format("addHighwaysToSceneStarting - {0}", DateTime.Now.TimeOfDay));
//            text.text += String.Format("addHighwaysToSceneStarting - {0}\r\n", DateTime.Now.TimeOfDay);
//            Canvas.ForceUpdateCanvases();
//            addHighwaysToSceneFinished = true;
//            AddHighwaysToScene();
//            // AddBuildingsToScene();
//        }

//    }






//}
