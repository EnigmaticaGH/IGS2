using UnityEngine;
using System.Collections;

public class SmashCamera : MonoBehaviour {

    public float motionDamping;
    public Transform upperBounds;
    public Transform lowerBounds;
    private static GameObject[] players;
    private static float[] xValues, yValues;
    private float minX, minY, maxX, maxY;
    private Vector3 size;
    private Vector3 velocity = Vector3.zero;
    private float vel = 0;
    private Camera cam;
    public Vector3 Velocity { get; private set; }
    public GameObject Skybox;
    Vector3 prevPos;

    public static void InitalizePlayers(GameObject[] p)
    {
        players = p;
        xValues = new float[p.Length];
        yValues = new float[p.Length];
    }

    void Awake()
    {
        cam = GetComponent<Camera>();
        prevPos = transform.position;
    }

    void Update()
    {
        if (players != null && IntroController.Intro == false)
        {
            UpdateCamera();
        }
    }

    void FixedUpdate()
    {
        Velocity = (transform.position - prevPos) / Time.deltaTime;
        prevPos = transform.position;
    }

    void UpdateCamera()
    {
        Vector3 midpoint = Midpoint(true);
        for(int i = 0; i < players.Length; i++)
        {
            xValues[i] = players[i].transform.position.x;
            yValues[i] = players[i].transform.position.y;
        }
        size = new Vector3(cam.orthographicSize * cam.aspect, cam.orthographicSize);
        float zoom = Mathf.Clamp(Mathf.Sqrt(Mathf.Pow(maxX - minX, 2) + Mathf.Pow(maxY - minY, 2)), 5, (upperBounds.position.x - lowerBounds.position.x) / 2);
        Vector3 clampedPosition = new Vector3(
            Mathf.Clamp(midpoint.x, lowerBounds.position.x + size.x, upperBounds.position.x - size.x),
            Mathf.Clamp(midpoint.y, lowerBounds.position.y + size.y, upperBounds.position.y - size.y),
            transform.position.z);
        transform.position = Vector3.SmoothDamp(transform.position, clampedPosition, ref velocity, motionDamping * Time.deltaTime);
        cam.orthographicSize = Mathf.SmoothDamp(cam.orthographicSize, zoom * 0.66f, ref vel, motionDamping * Time.deltaTime);
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
