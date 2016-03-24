using UnityEngine;
using System.Collections;

public class SmashCamera : MonoBehaviour {

    public float motionDamping;
    public Vector2 levelBounds;
    private static GameObject[] players;
    private static float[] xValues, yValues;
    private float minX, minY, maxX, maxY;
    private Vector3 size;
    private Vector3 velocity = Vector3.zero;

    public static void InitalizePlayers(GameObject[] p)
    {
        players = p;
        xValues = new float[p.Length];
        yValues = new float[p.Length];
    }

    void Update()
    {
        if (players != null && IntroController.Intro == false)
        {
            UpdateCamera();
        }
    }

    void UpdateCamera()
    {
        Vector3 midpoint = Midpoint(true);
        for(int i = 0; i < players.Length; i++)
        {
            xValues[i] = players[i].transform.position.x;
            yValues[i] = players[i].transform.position.y;
        }
        size = ScreenSize();
        float zoom = Mathf.Sqrt(Mathf.Pow(maxX - minX, 2) + Mathf.Pow(maxY - minY, 2));
        zoom = Mathf.Clamp(zoom, 6, 12);
        Vector3 zpos = new Vector3(transform.position.x, transform.position.y, -zoom);
        Vector3.SmoothDamp(transform.position, zpos, ref velocity, motionDamping * Time.deltaTime);
        Vector3 destination = midpoint + Vector3.forward * -zoom;
        Vector3 clampedPosition = new Vector3(
            Mathf.Clamp(destination.x, -levelBounds.x + (size.x / 2f), levelBounds.x - (size.x / 2f)),
            Mathf.Clamp(destination.y, -1 + (size.y / 2f), levelBounds.y - (size.y / 2f)),
            transform.position.z);
        transform.position = Vector3.SmoothDamp(transform.position, clampedPosition, ref velocity, motionDamping * Time.deltaTime);
    }

    Vector3 ScreenSize()
    {
        Camera cam = GetComponent<Camera>();
        Vector3 point1 = cam.ViewportToWorldPoint(new Vector3(0, 0, transform.position.z));
        Vector3 point2 = cam.ViewportToWorldPoint(new Vector3(1, 0, transform.position.z));
        Vector3 point3 = cam.ViewportToWorldPoint(new Vector3(1, 1, transform.position.z));

        float width = (point2 - point1).magnitude;
        float height = (point3 - point2).magnitude;

        return new Vector3(width, height, 0);
    }

    Vector3 Midpoint(bool includeVertical)
    {
        minX = Mathf.Min(xValues);
        minY = Mathf.Min(yValues);
        maxX = Mathf.Max(xValues);
        maxY = Mathf.Max(yValues);
        float midPointX = (minX + maxX) / 2f;
        float midPointY = (minY + maxY) / 2f;

        if(includeVertical)
            return new Vector3(midPointX, midPointY, 0);
        else
            return new Vector3(midPointX, transform.position.y, 0);
    }
}
