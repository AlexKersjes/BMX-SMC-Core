using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Connection : MonoBehaviour
{
    //Mqtt mqtt;
    // Start is called before the first frame update
    void Start()
    {
        Mqtt.Instance.connectRemote();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
