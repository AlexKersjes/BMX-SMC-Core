using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class CompareSlider : MonoBehaviour
{
    public Slider slider;
    private LoadCSV recordedFile;
    public VideoPlayer videoPlayer;
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
        //only allow this is no video is selected!
        if(videoPlayer.frameCount == 0)
        {
            foreach (Transform child in tagList)
            {
                selectedTag = child.gameObject;
                TagName = selectedTag.GetComponent<Tag>();
                if (TagName.isComparenSelected)
                {
                    foreach (List<string[]> data in LoadCSV.RecordedTagList)
                    {
                        for (int i = 0; i < 10; i++)
                        {
                            if (TagName.name == data[i][2] + "")
                            {
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
        }
        
        
        if (playing)
        {
            timer += (Time.deltaTime * playbackSpeed);
            if (timer != videoPlayer.length && videoPlayer.frameCount > 0)
            {
                TimedSliderVal = scaleTime(0, videoPlayer.length, 0, videoPlayer.frameCount, timer);
                slider.value = TimedSliderVal;
            }
            else if(videoPlayer.frameCount > 0)
            {
                playing = false;
            }
            else
            {
                if (timer != LoadCSV.diffirent && sliderMaxCount > 0)
                {
                    TimedSliderVal = scaleTime(0, LoadCSV.diffirent, 0, sliderMaxCount, timer);
                    slider.value = TimedSliderVal;
                }
                else if (sliderMaxCount > 0)
                {
                    playing = false;
                }
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
        if(videoPlayer.frameCount == 0)
        {
            slider.minValue = 1;
            if (!playing)
            {
                TimedSliderVal = Convert.ToInt32(slider.value);
                timer = scaleTime(0, slider.maxValue, 0, LoadCSV.diffirent, TimedSliderVal);
            }
        }
    }
    public void UpdateVideo()
    {
        maxFrames = videoPlayer.frameCount;
        if (maxFrames > 0)
        {
            slider.maxValue = maxFrames;
            double video = scaleTime(0, slider.maxValue, 0, maxFrames, slider.value);
            videoPlayer.frame = (long)video;
            TimedSliderVal = Convert.ToInt32(slider.value);
            timer = scaleTime(0, slider.maxValue, 0 , videoPlayer.length, TimedSliderVal);
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
