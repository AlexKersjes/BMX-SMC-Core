using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SmartDLL;
using System;
using System.IO;
using System.Linq;
using UnityEngine.Video;

public class LoadVid : MonoBehaviour
{
    public Button OpenFileBtn;
    public VideoPlayer vidToLoad;
    public RectTransform videoPanel;

    private GameObject videoTimer;
    private WWW w;

    //TODO: This file explorer is awful, change it to just using windows's file explorer
    public SmartFileExplorer fileExplorer = new SmartFileExplorer();

    private bool readText = false;
    private Vector2 showVideoPanel = new Vector2(480, 0f);

    void OnEnable()
    {
        OpenFileBtn.onClick.AddListener(delegate { ShowExplorer(); });
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (fileExplorer.resultOK && readText)
        {
            
            LoadFile(fileExplorer.fileName);
            readText = false;
        }

        //test code
        if (Input.GetKey(KeyCode.F)) 
        {
            LoadFile("https://r6---sn-32o-guhl.googlevideo.com/videoplayback?expire=1614872386&ei=4qpAYOmBCoyr1wKIkIagCA&ip=84.81.86.11&id=o-ADFe9G6J04Y-DhLmHqmi2lu6xd53U5MQgcjX5BAPk2Vm&itag=298&aitags=133%2C134%2C135%2C136%2C160%2C242%2C243%2C244%2C247%2C278%2C298%2C302&source=youtube&requiressl=yes&mh=Vb&mm=31%2C29&mn=sn-32o-guhl%2Csn-32o-5hne&ms=au%2Crdu&mv=m&mvi=6&pl=24&initcwndbps=1750000&vprv=1&mime=video%2Fmp4&ns=y_RnlHqNtpyDPJZZHx-AE-gF&gir=yes&clen=3472287&dur=59.999&lmt=1450180685507002&mt=1614850594&fvip=6&keepalive=yes&fexp=24001374%2C24007246&c=WEB&n=4zODkBHfQwlJeSFoK_-&sparams=expire%2Cei%2Cip%2Cid%2Caitags%2Csource%2Crequiressl%2Cvprv%2Cmime%2Cns%2Cgir%2Cclen%2Cdur%2Clmt&sig=AOq0QJ8wRQIhAKYZUOl7EDL3zi1gk1bEJo_GBgVd4xWxOdz7Q2M5x2G0AiANdb2V1S6mOR7zuZuuxAavq_zaqRuMSHWLN3BygVukug%3D%3D&lsparams=mh%2Cmm%2Cmn%2Cms%2Cmv%2Cmvi%2Cpl%2Cinitcwndbps&lsig=AG3C_xAwRQIhAIomnObXxT2eCXGZ5tn3UUW9z1sf-A5S58iSCj8yW0mWAiBPlMEyV30vZQu5g24_3Yk6zWJC-3d8HAuYX_FG2qkUug%3D%3D&ratebypass=yes");
        }
    }

    void ShowExplorer()
    {
        Mqtt.Instance.Disconnect();
        string initialDir = @"C:\";
        bool restoreDir = true;
        string title = "Open a Recorded Run";
        string defExt = "MP4";
        string filter = "MP4 files (*.MP4)|*.MP4";

        fileExplorer.OpenExplorer(initialDir, restoreDir, title, defExt, filter);
        CameraOrbit.CameraDisabled = true;
        readText = true;
        //Debug.Log(fileExplorer.fileName);
    }

    void LoadFile(string path)
    {
        StartCoroutine(loadVideo("file:///"+ path));
        
    }
    
    //TODO: Should be replaced with UnityWebRequest
    IEnumerator loadVideo(string path)
    {
        w = new WWW(path);
        vidToLoad.url = w.url;
        vidToLoad.playOnAwake = false;
        vidToLoad.Prepare();
        
        WaitForSeconds waitTime = new WaitForSeconds(2);
        while (!vidToLoad.isPrepared)
        {
            Debug.Log("Preparing Video");
            //Prepare/Wait for 5 sceonds only
            yield return waitTime;
            //Break out of the while loop after 5 seconds wait
            break;
        }
        if (vidToLoad.isPrepared)
        {
            Camera.main.rect = new Rect(0f, 0f, .5f, 1f);
            vidToLoad.frame = 10;
            vidToLoad.GetComponent<RawImage>().enabled = true;
            videoPanel.anchoredPosition = showVideoPanel;
            vidToLoad.Play();
            vidToLoad.targetTexture = (RenderTexture)vidToLoad.GetComponent<RawImage>().texture;
            vidToLoad.Pause();
            GameObject.Find("ReplaySliderCompare").GetComponent<Slider>().maxValue = vidToLoad.frameCount;

        }
    }
}
