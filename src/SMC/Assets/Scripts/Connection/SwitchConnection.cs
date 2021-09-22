using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;// Required when using Event data.

public class SwitchConnection : MonoBehaviour, IPointerDownHandler
{
    //Mqtt mqtt;
    // Start is called before the first frame update
    void Start()
    {
        //Mqtt.Instance.connectRemote();
    }
    //Do this when the mouse is clicked over the selectable object this script is attached to.
    public void OnPointerDown(PointerEventData eventData)
    {
        if (this.gameObject.GetComponent<Slider>().value == 0)
        {
            //Remote
            this.gameObject.GetComponent<Slider>().value = 1;
            Mqtt.Instance.connectLocal();
        }
        else
        {
            //Local
            this.gameObject.GetComponent<Slider>().value = 0;
            Mqtt.Instance.connectRemote();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
