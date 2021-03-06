using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SmartDLL;
using System;
using System.IO;
using System.Linq;

public class LoadCSV : MonoBehaviour
{
    public Button OpenFileBtn;
    public static double diffirent;
    public Slider replaySlider;

    //TODO: This file explorer is awful, change it to just using windows's file explorer
    public SmartFileExplorer fileExplorer = new SmartFileExplorer();
    public List<string[]> RecordedTagData;
    public static List<List<string[]>> RecordedTagList { get; set; }
    

    private bool readText = false;

    void OnEnable()
    {
        OpenFileBtn.onClick.AddListener(delegate { ShowExplorer(); });
    }

    // Start is called before the first frame update
    void Start()
    {
        RecordedTagList = new List<List<string[]>>();
    }

    // Update is called once per frame
    void Update()
    {
        if (fileExplorer.resultOK && readText)
        {
            LoadFile(fileExplorer.fileName);
            readText = false;
        }
    }

    void ShowExplorer()
    {
        Mqtt.Instance.Disconnect();
        string initialDir = @"C:\"; 
        bool restoreDir = true;
        string title = "Open a Recorded Run";
        string defExt = "csv";
        string filter = "csv files (*.csv)|*.csv";

        fileExplorer.OpenExplorer(initialDir, restoreDir, title, defExt, filter);
        readText = true;
        CameraOrbit.CameraDisabled = true;
    }

    void LoadFile(string path)
    {
        RecordedTagData = new List<string[]>();
        int i = 0;
        foreach (string a in File.ReadAllLines(path))
        {
            if (a != "") // check if the file doesnt containt open/empty lines
            {
                string b = a.Insert(0, i + ",");
                string[] splitData = b.Split(',');
                RecordedTagData.Add(splitData);
                i++;
            }
        }
        RecordedTagList.Add(RecordedTagData);
       // diffirent = Convert.ToDouble(RecordedTagData.Last()[3]) - Convert.ToDouble(RecordedTagData[1][3]);
        //replaySlider.maxValue = (int)diffirent;

    }
    public List<List<string[]>> GetTagList()
    {
        //Can return value of XYZ in the field and display it in the eText
        return RecordedTagList;
    }
}
