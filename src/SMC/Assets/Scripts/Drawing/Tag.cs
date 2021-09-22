using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class Tag : MonoBehaviour
{
    [SerializeField]
    public Text myText;
    public GameObject Bike;
    public GameObject InfoPanel;

    public Draw drawScript;
    public InputField SecondsOfData;

    private List<double> distanceOverTime = new List<double>(); //Saves all the data
    private List<Vector3> measureDistance = new List<Vector3>(); // Only contains a section of tagXYZ to be drawn

    private Vector3 EmptyVector = new Vector3(0f,0f,0f);
    private Vector3 TagData;
    private List<Vector3> EmptyList = new List<Vector3>();

    private bool drawLine = false;
    private string myTextString;
    private int DataLength;
    private Slider replaySlider;
    private double speed, averageTime, sqrtDistance, lastSpeed;
    private double height;

    public Toggle isMain;
    public bool isMainSelected = false;
    public Toggle isCompare;
    public bool isComparenSelected = false;

    private List<List<string[]>> recordedTagList;
    private LoadCSV test;
    
    

    //speed measurings
    private string[] sliderData;
    private int dataValueCount = 25;
    

    // Start is called before the first frame update
    void Start()
    {
        CultureInfo info = new CultureInfo("en-US");
        info.NumberFormat.NumberDecimalSeparator = ".";
        Thread.CurrentThread.CurrentCulture = info;
        Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
        
        DataLength = Convert.ToInt32(SecondsOfData.text);
        

        EmptyList.Add(EmptyVector);
        if(LoadCSV.RecordedTagList.Count > 0)
        {
            foreach (List<string[]> recordedData in LoadCSV.RecordedTagList)
            {
                foreach (string[] data in recordedData)
                {
                    if (Convert.ToInt32(data[2]) == Convert.ToInt32(GetComponent<Toggle>().name))
                    {
                        TagData = new Vector3(float.Parse(data[4]), float.Parse(data[5]), float.Parse(data[6]));
                        Vector3 filteredData = GetScaledXYZ(TagData);
                        //tagXYZ.Add(filteredData);
                    }
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (drawLine && replaySlider != null)
        {
            foreach(List<string[]> data in LoadCSV.RecordedTagList)
            {
                Bike.gameObject.GetComponent<TrailRenderer>().time = float.Parse(SecondsOfData.text);
                measureDistance.Clear();
                distanceOverTime.Clear();

                if ((isComparenSelected == true && GameObject.Find("VideoPlayer").GetComponent<VideoPlayer>().frameCount == 0) || isMainSelected)
                {
                    if (replaySlider.value > dataValueCount)
                    {
                        for (int i = dataValueCount; i > measureDistance.Count; i--)
                        {
                            //TODO: Instead of hard coding array positions, base this on parameter names in CSV file
                            sliderData = data[Convert.ToInt32(replaySlider.value - i)];
                            if (Convert.ToInt32(sliderData[2]) == Convert.ToInt32(GetComponent<Toggle>().name))
                            {
                                TagData = new Vector3(float.Parse(sliderData[4]), float.Parse(sliderData[5]), float.Parse(sliderData[6]));
                                measureDistance.Add(TagData);
                                distanceOverTime.Add(Convert.ToDouble(sliderData[4]));
                                Bike.transform.position = GetScaledXYZ(TagData);
                            }
                        }
                        if (distanceOverTime.Count > 1)
                        {
                            averageTime = (distanceOverTime.Last() - distanceOverTime[0]);
                        }
                        if(measureDistance.Count > 1)
                        {
                            sqrtDistance = Math.Sqrt(Math.Pow((measureDistance[1].x / 1000 - measureDistance.Last().x / 1000), 2) +
                                                                          Math.Pow((measureDistance[1].y / 1000 - measureDistance.Last().y / 1000), 2) +
                                                                          Math.Pow((measureDistance[1].z / 1000 - measureDistance.Last().z / 1000), 2));
                            speed = (sqrtDistance / averageTime) * 3.6; //3.6 to km/u
                            height = float.Parse(sliderData[6]) / 1000;
                            if (height < 1)
                            {
                                height = 0;
                            }
                        }
                    }
                }

                else
                {
                    speed = 0; // Doesnt have enough data yet
                }

                GameObject speedText = GameObject.Find(GetComponent<Toggle>().name + "Info/tagSpeed");
                GameObject heightText = GameObject.Find(GetComponent<Toggle>().name + "Info/tagHeight");

                speedText.GetComponent<Text>().text = "Snelheid: " + Math.Round(speed) + " km/u";
                heightText.GetComponent<Text>().text = "Hoogte: " + Math.Round(height, 1) + " m";
            }
            
        }
    }


    
    public void ActivateTag()
    {
        //Filling information

        //
        drawLine = !drawLine;
        if (drawLine)
        {
            Debug.Log("Activated " + GetComponent<Toggle>().name);
            GameObject information = Instantiate(InfoPanel) as GameObject;
            information.SetActive(true);
            information.name = GetComponent<Toggle>().name+"Info";
            GameObject informationText = GameObject.Find(GetComponent<Toggle>().name + "Info/tagText");
            informationText.GetComponent<Text>().text = GetComponent<Toggle>().name + " Info";
            information.transform.SetParent(InfoPanel.transform.parent, false);
            
            //Vector3 TagData = new Vector3(float.Parse(LoadCSV.RecordedTagData[1][4]), float.Parse(LoadCSV.RecordedTagData[1][5]), float.Parse(LoadCSV.RecordedTagData[1][6]));
            Bike.transform.position = GetScaledXYZ(TagData);
            Bike.SetActive(true);
            if (isMain.isOn)
            {
                replaySlider = GameObject.Find("ReplaySlider").GetComponent<Slider>();
                isMainSelected = true;
            }
            if (isCompare.isOn)
            {
                if(GameObject.Find("VideoPlayer").GetComponent<VideoPlayer>().frameCount == 0)
                {
                    replaySlider = GameObject.Find("ReplaySliderCompare").GetComponent<Slider>();
                    isComparenSelected = true;
                }
            }
        }
        else
        {
            Debug.Log("de-activated " + GetComponent<Toggle>().name);

            GameObject information = GameObject.Find(GetComponent<Toggle>().name + "Info");
            information.SetActive(false);
            Bike.SetActive(false);
            if (!isMain.isOn)
            {
                isMainSelected = false;
            }
            if (!isCompare.isOn)
            {
                isComparenSelected = false;
            }
        }
    }

    public void SetText(string name)
    {
        myTextString = name;
        myText.text = name;
    }
    
    public Vector3 GetScaledXYZ(Vector3 xyzVal)
    {
        float newxLine = scale(500, -27535, -28, 0, xyzVal.y);
        float newyLine = scale(0, 6265, 0, 6f, xyzVal.z);
        float newzLine = scale(0, 30300, 0, 30f, xyzVal.x);
        Vector3 newPos = new Vector3(newxLine, newyLine, newzLine);
        return newPos;
    }
    // Function also exist in mqtt but should later only exist in this class
    public float scale(float OldMin, float OldMax, float NewMin, float NewMax, float OldValue)
    {
        float OldRange = (OldMax - OldMin);
        float NewRange = (NewMax - NewMin);
        float NewValue = (((OldValue - OldMin) * NewRange) / OldRange) + NewMin;

        return (NewValue);
    }
    
}
