using UnityEngine;
using System.Collections;

public class CursorMovement_p3 : MonoBehaviour {

    public Rigidbody rb;
    public float CursorSpeed = 100;

    // Use this for initialization
    void Start()
    {

        rb = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("L_XAxis_");
        float vertical = Input.GetAxis("L_YAxis_3");

        rb.velocity = new Vector3(horizontal * CursorSpeed * Time.deltaTime, vertical * CursorSpeed * Time.deltaTime, 0);


        // ...

        // 6 - Make sure we are not outside the camera bounds
        var dist = (transform.position - Camera.main.transform.position).z;

        var leftBorder = Camera.main.ViewportToWorldPoint(
          new Vector3(0, 0, dist)
        ).x;

        var rightBorder = Camera.main.ViewportToWorldPoint(
          new Vector3(1, 0, dist)
        ).x;

        var topBorder = Camera.main.ViewportToWorldPoint(
          new Vector3(0, 0, dist)
        ).y;

        var bottomBorder = Camera.main.ViewportToWorldPoint(
          new Vector3(0, 1, dist)
        ).y;

        transform.position = new Vector3(
          Mathf.Clamp(transform.position.x, leftBorder, rightBorder),
          Mathf.Clamp(transform.position.y, topBorder, bottomBorder),
          transform.position.z
        );
    }
}
