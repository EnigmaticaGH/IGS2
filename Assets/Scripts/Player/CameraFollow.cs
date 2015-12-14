using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
    public bool autoFindPlayer; //Finds GameObject tagged "Player" if enabled
    public bool followVertically; //Should the camera move along the Y axis with the player?
    public GameObject target; //Used if AutoFind is disabled
    public float followDistance;
    public float heightAbovePlayer;
    private GameObject player;
    private Rigidbody body;

    void Start()
    {
        if (autoFindPlayer)
            player = GameObject.FindGameObjectWithTag("Player");
        else
            player = target;

        if (player == null)
            Debug.LogError("Camera has nothing to follow. Did you forget to assign the camera a follow target?");
        else
            body = player.GetComponent<Rigidbody>();

        if (body == null)
            Debug.LogError("Target has no RigidBody attached to it.");
    }

    void Update()
    {
        if (body != null)
            UpdateCameraPosition();
    }

    void UpdateCameraPosition()
    {
        //Use camera's Y position if FollowVertically is disabled
        Vector3 targetPosition = followVertically ? player.transform.position 
            : new Vector3(player.transform.position.x, 0, player.transform.position.z);
        Vector3 destination = targetPosition + (Vector3.back * followDistance) + (Vector3.up * heightAbovePlayer);
        transform.position = destination;
    }
}
