using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tracer : MonoBehaviour
{
    LinkedListNode<PathPoint> lastPoint;
    private bool rewinding = false;
    private float lastTimeRendered = 0;

    public void SetPath (LinkedList<PathPoint> path)
	{
        lastPoint = path.First;
        this.transform.position = lastPoint.Value.transform.position;
	}

    public void RenderTime (float renderTime)
	{
        if(renderTime >= lastTimeRendered)
		{
			if (rewinding)
			{
                rewinding = false;
                lastPoint = lastPoint.Previous ?? lastPoint.List.First;
			}
            moveAlong(renderTime);
		}
		else
		{
            if (!rewinding)
			{
                rewinding = true;
                lastPoint = lastPoint.Next ?? lastPoint.List.Last;
			}
            moveBack(renderTime);
		}
	}

    private void moveAlong (float renderTime)
	{
        if (lastPoint.Next == null)
		{
            return;
        }
        while (lastPoint.Next.Value.timestamp < renderTime)
		{
            lastPoint = lastPoint.Next;
		}
        Vector3.Lerp(lastPoint.Value.transform.position, lastPoint.Next.Value.transform.position, (renderTime - lastPoint.Value.timestamp) / (lastPoint.Next.Value.timestamp - lastPoint.Value.timestamp));
	}

    private void moveBack (float renderTime)
	{
        if (lastPoint.Previous == null)
        {
            return;
        }
        while (lastPoint.Previous.Value.timestamp > renderTime)
        {
            lastPoint = lastPoint.Previous;
        }
        Vector3.Lerp(lastPoint.Value.transform.position, lastPoint.Previous.Value.transform.position, (renderTime - lastPoint.Value.timestamp) / (lastPoint.Previous.Value.timestamp - lastPoint.Value.timestamp));
    }
}
