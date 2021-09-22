using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class Draw : MonoBehaviour
{
    public Color c1 = Color.yellow;
    public Color c2 = Color.red;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("drawing instance");
        CultureInfo info = new CultureInfo("en-US");
        info.NumberFormat.NumberDecimalSeparator = ".";
        Thread.CurrentThread.CurrentCulture = info;
        Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

        


        Material material = new Material(Shader.Find("Sprites/Default"));
        LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.material = material;
        lineRenderer.widthMultiplier = 0.2f;

        // A simple 2 color gradient with a fixed alpha of 1.0f.
        float alpha = 1.0f;
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(c1, 0.0f), new GradientColorKey(c2, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) }
        );
        lineRenderer.colorGradient = gradient;
    }
    

    public void drawing(List<Vector3> DataFromFile)
    {
        LineRenderer lineRenderer = GetComponent<LineRenderer>();

        Vector3[] Positions = new Vector3[DataFromFile.Count];
        lineRenderer.positionCount = Positions.Length;
        lineRenderer.SetPositions(Positions);
        Vector3[] arrayLines = DataFromFile.ToArray();
        lineRenderer.SetPositions(arrayLines);
        lineRenderer.SetPosition(0, arrayLines[0]);


    }
}
