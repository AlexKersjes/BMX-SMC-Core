using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CameraOrbit : MonoBehaviour
{
    //Get slider info to disable camera orbit during slide
    private GameObject slider;
    private Slider slideButton;

    protected Transform _XForm_Camera;
    protected Transform _XForm_Parent;

    protected Vector3 _LocalRotation;
    protected float _CameraDistance = 35f;
    //Original Camera Rotation 25,150m0
    public float MouseSensitivity = 4f;
    public float ScrollSensitivity = 2f;
    public float OrbitDampening = 10f;
    public float ScrollDampening = 6f;

    //Test Time bound Camera Orbit
    public float startTime = 0f;
    public float holdTime = 0.1f;

    public static bool CameraDisabled = true;


    // Start is called before the first frame update
    void Start()
    {
        this._XForm_Camera = this.transform;
        this._XForm_Parent = this.transform.parent;
        
        this.slideButton = GetComponent<Slider>();
    }

    // Late Update is called once per frame, after Update() on every game object in the scene.
    void LateUpdate()
    {
        if (Input.GetMouseButtonDown(1))
        {
            CameraDisabled = !CameraDisabled;
        }
        if (Input.GetMouseButtonUp(1))
        {
            CameraDisabled = !CameraDisabled;
        }
        if (!CameraDisabled)
        {
            //Rotation of the Camera based on Mouse Coordinates
            
            if(Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
            {
                _LocalRotation.y += Input.GetAxis("Mouse X") * MouseSensitivity;
                _LocalRotation.x -= Input.GetAxis("Mouse Y") * MouseSensitivity;

                //Clamp the y Rotation to horizon and not flipping over at the top
                if (_LocalRotation.x < 0f)
                    _LocalRotation.x = 0f;
                else if (_LocalRotation.x > 90f)
                    _LocalRotation.x = 90f;
                //_LocalRotation.y = Mathf.Clamp(_LocalRotation.y, 0f, 90f);
            }
        }
        //Zooming Input from our Mouse Scroll Wheel
        if (Input.GetAxis("Mouse ScrollWheel") != 0f)
        {
            float ScrollAmount = Input.GetAxis("Mouse ScrollWheel") * ScrollSensitivity;

            //Makes camera zoom faster the further awat it is from the target
            ScrollAmount *= (this._CameraDistance * 0.3f);

            this._CameraDistance += ScrollAmount * -1f;

            //This Makes the camera go no closer then 1.5 meters from the target and no further then 100 meters. 
            this._CameraDistance = Mathf.Clamp(this._CameraDistance, 1.5f, 100f);
        }
        //Actual Camera Rig Transformations
        Quaternion QT = Quaternion.Euler(_LocalRotation.x, _LocalRotation.y, 0);
        this._XForm_Parent.rotation = Quaternion.Lerp(this._XForm_Parent.rotation, QT, Time.deltaTime * OrbitDampening);
        
        if(this._XForm_Camera.localPosition.z != this._CameraDistance * -1f)
        {
            this._XForm_Camera.localPosition = new Vector3(0f, 5f, Mathf.Lerp(this._XForm_Camera.localPosition.z, this._CameraDistance * -1f, Time.deltaTime * ScrollDampening));
        }
        
    }
}
