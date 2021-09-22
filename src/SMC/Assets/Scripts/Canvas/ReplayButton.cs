using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReplayButton : MonoBehaviour
{
    public Button btnReplay;
    public Sprite playBtn;
    public Sprite pauseBtn;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PlayPause()
    {
        //Change Look and play/pause accordingly
        if (!ReplaySlider.playing && !CompareSlider.playing)
        {
            Debug.Log("aan");
            ReplaySlider.playing = true;
            CompareSlider.playing = true;
            btnReplay.GetComponent<Image>().sprite = pauseBtn;
        }
        else
        {
            Debug.Log("uit");
            ReplaySlider.playing = false;
            CompareSlider.playing = false;
            btnReplay.GetComponent<Image>().sprite = playBtn;
        }
    }
}
