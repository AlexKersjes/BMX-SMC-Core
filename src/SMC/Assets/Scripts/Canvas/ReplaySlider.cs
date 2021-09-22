using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class ReplaySlider : MonoBehaviour
{
    public Slider slider;
    private LoadCSV recordedFile;
    public Transform tagList;

    //private long TimedSliderVal;
    public static bool playing = false;

    private double timer = 0;
    private long TimedSliderVal;
    public static float playbackSpeed = 1;
    private ulong maxFrames;

    private GameObject selectedTag;
    private Tag TagName;
    private int sliderMaxCount;

    // Start is called before the first frame update
    void Start()
    {
        CultureInfo info = new CultureInfo("en-US");
        info.NumberFormat.NumberDecimalSeparator = ".";
        Thread.CurrentThread.CurrentCulture = info;
        Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

        recordedFile = GetComponent<LoadCSV>();
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Transform child in tagList)
        {
            selectedTag = child.gameObject;
            TagName = selectedTag.GetComponent<Tag>();
            if (TagName.isMainSelected)
            {
                foreach (List<string[]> data in LoadCSV.RecordedTagList)
                {
                    for (int i = 0; i < 10; i++)
                    {
                        if (TagName.name == data[i][2] + "")
                        {
                            //Debug.Log(i + " - " + TagName.name + " ---" + data[i][2]);
                            sliderMaxCount = data.Count;
                            slider.maxValue = sliderMaxCount;
                        }
                    }
                }
            }
            else
            {
                //sliderMaxCount = 0;
            }
        }
        if (playing)
        {
            timer += (Time.deltaTime * playbackSpeed);
            if (timer <= LoadCSV.diffirent)
            {
                TimedSliderVal = scaleTime(0, LoadCSV.diffirent, 0, sliderMaxCount, timer);
                slider.value = TimedSliderVal;
            }
            else
            {
                playing = false;
            }
        }

    }

    public void PlayPause()
    {
        //Change Look and play/pause accordingly
        timer = 0;
        if (!playing)
        {
            playing = true;
        }
        else
        {
            playing = false;
        }
    }

    public void UpdateValue()
    {
        slider.minValue = 1;
        slider.maxValue = sliderMaxCount;
        if (!playing)
        {
            TimedSliderVal = Convert.ToInt32(slider.value);
            timer = scaleTime(0, sliderMaxCount, 0, LoadCSV.diffirent, TimedSliderVal);
        }
    }
    
    public long scaleTime(double OldMin, double OldMax, double NewMin, double NewMax, double OldValue)
    {
        double OldRange = (OldMax - OldMin);
        double NewRange = (NewMax - NewMin);
        long NewValue = Convert.ToInt64((((OldValue - OldMin) * NewRange) / OldRange) + NewMin);

        return (NewValue);
    }
}
