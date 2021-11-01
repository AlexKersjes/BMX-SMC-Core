using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RunData
{
	public int RiderID 
	{ 
		get; set; 
	}
	public DateTime EndOfRunUTC 
	{ 
		get; set; 
	}
	public List<SensorData> sensors
	{
		get; set;
	}
}

[Serializable]
public struct SensorData
{
	public int id { get; set; }
	public Vector3 ClbrOffset
	{
		get; set;
	}

	public List<DataPoint> points
	{
		get; set;
	}
}

[Serializable]
public struct DataPoint
{
	public float confidence
	{
		get; set;
	}
	public List<Vector3> accelerometer
	{
		get; set;
	}
	public Vector3 coordinates
	{
		get; set;
	}
	public float hardwareTime 
	{ 
		get; set; 
	}
	public long systemTime
	{
		get; set;
	}
	public float speed
	{
		get; set;
	}
}
