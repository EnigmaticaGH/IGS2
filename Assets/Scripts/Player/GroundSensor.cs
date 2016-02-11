using UnityEngine;
using System.Collections;

public class GroundSensor : MonoBehaviour
{
    public int CubeLength = 1;
    public float speed = 1f;
    private Movement movement;
    private GameObject[] blocks;
    string clone;
    int i = 0;
    Vector2 curPos;
    Vector2 newPos;
    bool moveCube = false;
    //Collider c;

    void Start()
    {
        movement = GetComponentInParent<Movement>();
        movement.SendGroundSensorReading(true);
        blocks = GameObject.FindGameObjectsWithTag("Block");
        for (i = 0; i <= blocks.Length; i++)
        {
            //clone[i] = blocks[i];D

            //Debug.Log(blocks[i]);
        }
    }
    void OnTriggerEnter(Collider c)
    {
        if (c.CompareTag("Ground") && movement != null)
            movement.SendGroundSensorReading(true);
        if (c.CompareTag("Block") && (GameObject.Find("Player 1").GetComponent<Player1Abilities>().abilityA) && (CubeLength <= 2) )
        {
            if ((moveCube == false) && (c.gameObject.name != clone))
            {
                Debug.Log(c.gameObject.name + CubeLength);
                CubeLength++;
                curPos = c.gameObject.transform.position;
                newPos = new Vector3(curPos.x, curPos.y + 1, 0);
                c.GetComponent<Rigidbody>().isKinematic = true;
                StartCoroutine(CubePopUp(c));
                moveCube = true;
                clone = c.gameObject.name;
            }
            
            
        }
        
        
    }

    IEnumerator CubePopUp(Collider c)
    {
        
        

        Debug.Log(c.gameObject.transform.position);
        Debug.Log(curPos);
        Debug.Log(newPos);

        c.gameObject.transform.position = newPos; 
        
        /*while (Vector2.Distance(c.gameObject.transform.position, newPos) > 0.001f)
        {
            c.gameObject.transform.position = Vector2.Lerp(curPos, newPos, speed * Time.deltaTime);
            
            yield return null;
        }*/

        yield return null;
       
        moveCube = false;
        
    }

    void OnCollisionEnter(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            Debug.DrawRay(contact.point, contact.normal, Color.white);
            Debug.Log("Contact");
        }

        if (collision.gameObject.tag == "Block")
        {
            Debug.Log("Contact");
        }


    }
    void OnTriggerExit(Collider c)
    {
        if (c.CompareTag("Ground") && movement != null)
            movement.SendGroundSensorReading(false);
    }
}
