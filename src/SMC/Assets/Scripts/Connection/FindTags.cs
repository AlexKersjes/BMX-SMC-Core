using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Video;

public class FindTags : MonoBehaviour
{
    [SerializeField]
    public GameObject toggleTemplate;

    //private float waitTime = 2.0f; // wait x seconds till finding new tags
    private JArray jsonVal;
    public static List<int> foundTags = new List<int>();
    public static List<int> prevFoundTags = new List<int>();

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("updateTagList", 0, 2.0f);
    }

    // Update is called once per frame
    void Update()
    {
        

    }

    void updateTagList()
    {
        
        //Tag data from MQTT
        if (Mqtt.Instance.client.IsConnected)
        {
            
            //TODO: Make this into an event instead of trying to poll it
            //Find Tags
            jsonVal = Mqtt.Instance.getData();
            if (jsonVal != null)
            {
                var tags = jsonVal;
                foreach (var tag in tags)
                {
                    if (!foundTags.Contains(Convert.ToInt32(tag["tagId"])))
                    {
                        foundTags.Add(Convert.ToInt32(tag["tagId"]));
                    }
                }
            }
        }

        //Tag data from CSV log file
        //TODO: stop running this code after it's been excecuted once
        // AKA, either empty RecordedTagList after reading, or listen to an event from LoadCSV
        if(LoadCSV.RecordedTagList.Count != 0)
        {
            foreach (List<string[]> data in LoadCSV.RecordedTagList)
            {
                for (int i = 1; i < 10; i++)
                {
                    if (!foundTags.Contains(Convert.ToInt32(data[i][2])))
                    {
                        foundTags.Add(Convert.ToInt32(data[i][2]));
                        Debug.Log(Convert.ToInt32(data[i][2]));
                        
                    }
                }
            }
        }
        /*
        if (LoadCSV.RecordedTagData.Count != 0)
        {
            foreach (string[] Tag in LoadCSV.RecordedTagData)
            {
                if (!foundTags.Contains(Convert.ToInt32(Tag[2])))
                {
                    foundTags.Add(Convert.ToInt32(Tag[2]));
                    Debug.Log(Convert.ToInt32(Tag[2]));
                }
            }
        }
        */

        foreach (int item in foundTags)
        {
            if (!prevFoundTags.Contains(item))
            {
                prevFoundTags.Add(item);
                GameObject toggle = Instantiate(toggleTemplate) as GameObject;
                toggle.SetActive(true);
                if (GameObject.Find("VideoPlayer").GetComponent<VideoPlayer>().frameCount > 0)
                {
                    GameObject.Find("Compare").GetComponent<Toggle>().enabled = false;
                }
                else
                {
                    GameObject.Find("Compare").GetComponent<Toggle>().enabled = true;
                }
                toggle.GetComponent<Tag>().SetText("tag: " + item);
                toggle.GetComponent<Tag>().name = item.ToString();
                toggle.transform.SetParent(toggleTemplate.transform.parent, false);
            }
        }
    }
}
