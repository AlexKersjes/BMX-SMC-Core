using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoControls : MonoBehaviour
{
    public RawImage Videodisplay;
    public RectTransform videoPanel;

    private VideoPlayer videoPlayer;
    private Vector2 hideVideoPanel = new Vector2(1920f, 0f);
    // Start is called before the first frame update
    void Start()
    {
        Videodisplay.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void removeVideo()
    {
        Camera.main.rect = new Rect(0f, 0f, 1f, 1f);
        videoPlayer = Videodisplay.GetComponent<VideoPlayer>();
        videoPanel.anchoredPosition = hideVideoPanel;
        videoPlayer.url = "";
        videoPlayer.Stop();
        Videodisplay.enabled = false;
    }
    public void removeData()
    {
        foreach (List<string[]> dataList in LoadCSV.RecordedTagList)
        {
            dataList.Clear();
        }
        LoadCSV.RecordedTagList.Clear();
    }
    public void playPause()
    {
        videoPlayer = Videodisplay.GetComponent<VideoPlayer>();
        if (Videodisplay.IsActive())
        {
            if (videoPlayer.isPlaying)
            {
                videoPlayer.Pause();
            }
            else
            {
                videoPlayer.Play();
            }
        }
    }
}
