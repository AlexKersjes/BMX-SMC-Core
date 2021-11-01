using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JsonReader : MonoBehaviour
{
    [SerializeField]
    private PathPoint pathPointPrefab;
    public readonly List<LinkedList<PathPoint>> paths = new List<LinkedList<PathPoint>>() {};
    private static JsonReader instance;
    public static JsonReader GetInstance() { return instance; }
	private void Start()
	{
        JsonReader.instance = JsonReader.instance ?? this;
	}



	public void InsertData(string data)
	{
        var run = JsonUtility.FromJson<RunData>(data);

        run.sensors.ForEach(s => 
        {
            var points = new LinkedList<PathPoint>();
			for (int i = 0; i < s.points.Count; i++)
			{
                points.AddLast(PathPoint.Create(pathPointPrefab, s.points[i]));
			}
            LinkedListNode<PathPoint> cursor = points.First;
            while (cursor.Next != null)
			{
                cursor.Value.setLineRendererTarget(cursor.Next.Value);
			}
            paths.Add(points);
        });
	}
}
