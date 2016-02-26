using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class CursorMovement : MonoBehaviour {

    public delegate void ClickAction();
    public static event ClickAction OnClickedLeft;
    public static event ClickAction OnClickedRight;
    public static event ClickAction OnClickedLeftTime;
    public static event ClickAction OnClickedRightTime;

    public delegate void LevelLoader();
    public static event LevelLoader Level1Hover;
    public static event LevelLoader Level1Load;
    public static event LevelLoader Level2Hover;
    public static event LevelLoader Level2Load;
    public static event LevelLoader Level3Load;
    public static event LevelLoader Level3Hover;
    public static event LevelLoader Level4Load;
    public static event LevelLoader Level4Hover;

    public delegate void PlayerReady();
    public static event PlayerReady P1ReadyEvent;
    public static event PlayerReady P2ReadyEvent;
    int p1Count, p2Count, p3Count, p4Count;
    bool p1Ready = false;
    bool p2Ready = false;
    bool p3Ready = false;
    bool p4Ready = false;
    //public static event PlayerReady Player3;
    //public static event PlayerReady Player4;

    public delegate void CharacterSelection();
    public static event CharacterSelection p1_Next;
    public static event CharacterSelection p2_Next;
    public static event CharacterSelection p3_Next;
    public static event CharacterSelection p4_Next;

    bool loadScene1 = false;
    bool loadScene2 = false;
    bool loadScene3 = false;
    bool loadScene4 = false;


    private Rigidbody rb;
    private float CursorSpeed = 5;
    public int ControllerNumber;
    public bool LeftButton = false;
    public bool RightButton = false;
    int i = 0;
    int c = 0;
    int NAC = 0; //NAC - Number of Active Controllers

	// Use this for initialization
	void Start () 
    {

        rb = GetComponent<Rigidbody>();
        NAC = CharacterMenuController.ControllerNumber;
        Debug.Log(NAC);
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        float horizontal = Input.GetAxis("L_XAxis_" + ControllerNumber);
        float vertical = Input.GetAxis("L_YAxis_" + ControllerNumber);

        rb.velocity = new Vector3(horizontal * CursorSpeed, -vertical * CursorSpeed, 0);


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

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Button")
        {
            //Debug.LogError("Button Collision Detected");
            
        }
    }

    void OnTriggerStay(Collider col)
    {

        //Character Selection Menu
        if (col.name == "Button_p1")
        {
            if (Input.GetButton("A_1") && (i == 0))
            {
                p1_Next();
                //Debug.Log("Player Character Selection Pressed A");
            }
        }
        if (col.name == "Button_p2")
        {
            if (Input.GetButton("A_2") && (i == 0))
            {
                p2_Next();
            }
        }

        if (col.name == "Button_p3")
        {
            if (Input.GetButton("A_3") && (i == 0))
            {
                p3_Next();
            }
        }
        if (col.name == "Button_p4")
        {
            if (Input.GetButton("A_4") && (i == 0))
            {
                p4_Next();
            }
        }

        if (col.tag == "Button")
        {
                //Debug.LogError("Wow collision detected");
            if (Input.GetButton("A_" + ControllerNumber) && (i == 0))
            {
                 i++; //Make i one so you can't constantly press A 
               //Debug.Log("Player clicked A");
                 Invoke("CooldownA", .5f);
            }
        }

        if (col.name == "ReadyButton p1")
        {
            if (Input.GetButton("A_1") && (i == 0))
            {
                p1Count++;
                p1Ready = true;

                //Button Toggle
                if (p1Count > 1)
                {
                    p1Count = 0;
                    p1Ready = false;
                }

                Debug.Log(p1Ready);


                Debug.Log("Player 1 clicked ready");
                P1ReadyEvent();
                i++;

                Invoke("CooldownA", .5f);
            }
                
        }

        if (col.name == "ReadyButton p2")
        {
            if (Input.GetButton("A_2") && (c == 0))
            {
                p2Count++;
                p2Ready = true;

                //Button Toggle
                if (p2Count > 1)
                {
                    p2Count = 0;
                    p2Ready = false;
                }
                Debug.Log(p2Ready);
                Debug.Log("Player 2 clicked ready");
                P2ReadyEvent();
                c++;

                Invoke("CooldownA", .5f);
            }

        }

        if (col.name == "ReadyButton p3")
        {
            if (Input.GetButton("A_3") && (i == 0))
            {
                p3Count++;
                p3Ready = true;

                //Button Toggle
                if (p3Count > 1)
                {
                    p3Count = 0;
                    p3Ready = false;
                }

                Debug.Log(p3Ready);


                Debug.Log("Player 1 clicked ready");
                P1ReadyEvent();
                i++;

                Invoke("CooldownA", .5f);
            }

        }
        if (col.name == "ReadyButton p4")
        {
            if (Input.GetButton("A_4") && (i == 0))
            {
                p4Count++;
                p4Ready = true;

                //Button Toggle
                if (p4Count > 1)
                {
                    p4Count = 0;
                    p4Ready = false;
                }

                Debug.Log(p4Ready);


                Debug.Log("Player 1 clicked ready");
                P1ReadyEvent();
                i++;

                Invoke("CooldownA", .5f);
            }

        }

        if (col.tag == "Start")
        {
            Debug.Log("I touched start" + ControllerNumber);
            if (Input.GetButton("A_" + ControllerNumber) && (i == 0))
            {
                Debug.Log("PLayer clicked A" + ControllerNumber);
                Debug.Log(p1Ready);
                Debug.Log(p2Ready);
                i++; //Make i one so you can't constantly press A 
                //Debug.Log("Player clicked A " + col.tag);
                /*
                 * Used for player readiness if controller number is 1 then if player 1 is ready go to next scene and so on 
                 */

                if (p1Ready && (NAC == 1))
                    SceneManager.LoadScene(2);
                if((p1Ready && p2Ready) && NAC == 2)
                    SceneManager.LoadScene(2);
                if ((p1Ready && p2Ready) && p3Ready)
                {
                    if(NAC == 3)
                        SceneManager.LoadScene(2);
                }

                if ((p1Ready && p2Ready) && (p2Ready && p4Ready))
                    SceneManager.LoadScene(2);
                    


                //SceneManager.LoadScene(2);
                    

                Invoke("CooldownA", .5f);
            }
        }

        if (col.name == "Left Button")
        {
            //Debug.Log("Left Button Detected");
            LeftButton = true;
            
            if (Input.GetButton("A_" + ControllerNumber) && (i == 0))
            {
                i++; //Make i one so you can't constantly press A 
               // Debug.Log("Player clicked A ");

                OnClickedLeft();

                Invoke("CooldownA", .5f);
            }
        }
        else
            LeftButton = false;

        if (col.name == "Right Button")
        {
            RightButton = true;
           
            if (Input.GetButton("A_" + ControllerNumber) && (i == 0))
            {
                i++; 

                OnClickedRight();

                Invoke("CooldownA", .5f);
            }
        }
        else
            RightButton = false;

        if (col.name == "Level - 1") 
        {
            Level1Hover();
            if (Input.GetButton("A_" + ControllerNumber) && (i == 0)) 
            {
                loadScene1 = true;
                loadScene2 = false;
                loadScene3 = false;
                loadScene4 = false;
                i++;

                Level1Load();

                Invoke("CooldownA", .5f);
            }
            
        }

        if (col.name == "Level - 2") 
        {
            Level2Hover();
            if (Input.GetButton("A_" + ControllerNumber) && (i == 0))
            {
                loadScene2 = true;
                loadScene1 = false;
                loadScene3 = false;
                loadScene4 = false;
                i++;

                Level2Load();

                Invoke("CooldownA", .5f);
            }
            
        }

        if (col.name == "Level - 3") 
        {
            Level3Hover();
            if (Input.GetButton("A_" + ControllerNumber) && (i == 0))
            {
                loadScene3 = true;
                loadScene1 = false;
                loadScene2 = false;
                loadScene4 = false;
                i++;

                Level3Load();

                Invoke("CooldownA", .5f);
            }
        }

        if (col.name == "Level - 4")
        {
            Level4Hover();
            if (Input.GetButton("A_" + ControllerNumber) && (i == 0))
            {
                loadScene4 = true;
                loadScene3 = false;
                loadScene1 = false;
                loadScene2 = false;
                i++;

                Level4Load();

                Invoke("CooldownA", .5f);
            }
        }

        if (col.name == "Right Button - TIME")
        {
            if (Input.GetButton("A_" + ControllerNumber) && (i == 0))
            {
                //Debug.Log("Level Selected");

                i++;

                OnClickedRightTime();

                Invoke("CooldownA", .5f);
            }
        }

        if (col.name == "Left Button - TIME")
        {
            if (Input.GetButton("A_" + ControllerNumber) && (i == 0))
            {
                //Debug.Log("Level Selected");

                i++;

                OnClickedLeftTime();

                Invoke("CooldownA", .5f);
            }
        }

        if (col.name == "Start Game")
        {
            if (Input.GetButton("A_" + ControllerNumber) && (i == 0))
            {
                Debug.Log("Start Game");

                i++;
                if(loadScene1)
                    SceneManager.LoadScene(3);
                if (loadScene2)
                    SceneManager.LoadScene(4);
                if (loadScene3)
                    SceneManager.LoadScene(5);
                if (loadScene4)
                {
                    SceneManager.LoadScene(6);
                }

                Invoke("CooldownA", .5f);
            }
        }
    }

    void CooldownA()
    {
        i = 0;
        c = 0;
    }
}
