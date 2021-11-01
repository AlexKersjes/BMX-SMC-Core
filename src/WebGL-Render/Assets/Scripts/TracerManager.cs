using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TracerManager : MonoBehaviour
{
    [SerializeField]
    private Tracer tracerPrefab;
    private List<Tracer> tracers = new List<Tracer>();
    [SerializeField]
    private float speed;
    private float lastRenderedTime = 0;

    private void Update()
    {
        var newTime = lastRenderedTime + Time.deltaTime * 1000 * speed;
        tracers.ForEach(t => t.RenderTime(newTime));
        lastRenderedTime = newTime;
    }

    public void StartTracers()
    {
        List<float> startTimes = new List<float>();
        foreach (LinkedList<PathPoint> path in JsonReader.GetInstance().paths)
        {
            var tracer = Instantiate<Tracer>(tracerPrefab);
            tracer.SetPath(path);
            tracers.Add(tracer);
            startTimes.Add(path.First.Value.timestamp);
        }
        lastRenderedTime = Mathf.Min(startTimes.ToArray());     
    }
}
