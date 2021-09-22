using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveSidePanel : MonoBehaviour
{
    public RectTransform sidePanel;
    public Text imageText;

    private Vector2 closedSidePanel, openSidePanel;
    private bool sidePanelStatus = false;
    // Start is called before the first frame update
    void Start()
    {
        closedSidePanel = sidePanel.anchoredPosition;
    }

    public void moveSidePanel()
    {
        openSidePanel = new Vector2(0, sidePanel.anchoredPosition.y);
        if (sidePanelStatus)
        {
            sidePanel.anchoredPosition = closedSidePanel;
        }
        else
        {
            sidePanel.anchoredPosition = openSidePanel;
        }
        sidePanelStatus = !sidePanelStatus;
    }
    
    public void changeText()
    {
        if (sidePanelStatus)
        {
            imageText.text = "<";
        }
        else
        {
            imageText.text = ">";
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
