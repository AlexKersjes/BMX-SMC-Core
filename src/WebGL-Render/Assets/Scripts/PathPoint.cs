using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathPoint : MonoBehaviour
{
    LineRenderer _line;
    LineRenderer line { get { return _line ?? this.GetComponent<LineRenderer>(); } }
    public float timestamp { get; set; }
    public Vector3 location { get { return this.transform.position; } set { this.transform.position = value; } }
    public Vector3 acceleration { get; set; }
    public float speed { get; set; }
    public PathPoint nextPoint { get; set; }
    public void SetData(DataPoint data)
	{
        location = data.coordinates;
        acceleration = data.accelerometer[0];
        speed = data.speed;
        timestamp = data.hardwareTime;
	}

    public static PathPoint Create (PathPoint prefab, DataPoint data)
	{
        PathPoint newObject = Instantiate<PathPoint>(prefab);
        newObject.SetData(data);
        return newObject;
	}

    public void setLineRendererTarget (PathPoint target)
	{
        this.line.positionCount = 2;
        this.line.useWorldSpace = true;
        this.line.SetPositions(new Vector3[] { this.location, target.location });
	}
}
