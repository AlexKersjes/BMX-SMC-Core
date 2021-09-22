using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveReplayPanel : MonoBehaviour
{
    public RectTransform replayPanel;
    private Vector2 closedReplayPanel, openReplayPanel;
    private bool replayPanelStatus = false;

    // Start is called before the first frame update
    void Start()
    {
        closedReplayPanel = replayPanel.anchoredPosition;
        openReplayPanel = new Vector2(0, 55);
    }
    public void moveReplayPanel()
    {
        if (replayPanelStatus)
        {
            replayPanel.anchoredPosition = closedReplayPanel;
        }
        else
        {
            replayPanel.anchoredPosition = openReplayPanel;
        }
        replayPanelStatus = !replayPanelStatus;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
