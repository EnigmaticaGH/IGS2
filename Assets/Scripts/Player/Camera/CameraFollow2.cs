using UnityEngine;
using System.Collections;

public class CameraFollow2 : MonoBehaviour
{
    public bool followVertically; //Should the camera move along the Y axis with the player?
    public GameObject[] targets;
    public float followDistance;
    public float heightAbovePlayer;

    void Update()
    {
        UpdateCameraPosition();
    }

    void UpdateCameraPosition()
    {
        float currentDistance = followDistance;
        float currentHeight = heightAbovePlayer;

        float minX = float.MaxValue, maxX = 0;
        float minY = float.MaxValue, maxY = 0;

        foreach (GameObject g in targets)
        {
            if (g != null)
            {
                if (g.transform.position.x < minX)
                    minX = g.transform.position.x;
                if (g.transform.position.x > maxX)
                    maxX = g.transform.position.x;

                if (g.transform.position.y < minY)
                    minY = g.transform.position.y;
                if (g.transform.position.y > maxY)
                    maxY = g.transform.position.y;
            }
        }
        float distance = maxX - minX;
        float midpointX = (minX + maxX) / 2f;
        float midpointY = (minY + maxY) / 2f;

        currentDistance = distance;
        
        if (currentDistance < followDistance)
            currentDistance = followDistance;

        currentHeight = currentDistance / followDistance;

        //Use camera's Y position if FollowVertically is disabled
        Vector3 targetPosition = followVertically ? Vector3.right * midpointX + Vector3.up * midpointY
            : new Vector3(midpointX, 0, 0);
        Vector3 destination = targetPosition + (Vector3.back * currentDistance) + (Vector3.up * currentHeight);
        transform.position = destination;
    }
}
