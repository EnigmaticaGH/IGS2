using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class IntroController : MonoBehaviour {

    public static bool Intro = false; //When this equals false the camera turns into smash camera
    public Transform locationIntro;
    public Transform platformLoc;
    public Transform[] playerSpawns;
    public GameObject[] Characters;
    //public Text[] Names;
    //int conrtollerNumber;

    void Awake()
    {
        //int p1Pos = 0;
        //int p2Pos = 0;
        //int p3Pos = 0;
        //int p4Pos = 0;
        //p1Pos = CharacterMenuController.p1Pos; //Use this for spawning selected characters
        //p2Pos = CharacterMenuController.p2Pos; //Use this for spawning selected characters
        //p3Pos = CharacterMenuController.p3Pos; //Use this for spawning selected characters
        //p4Pos = CharacterMenuController.p4Pos; //Use this for spawning selected characters

        //conrtollerNumber = CharacterMenuController.ControllerNumber;

        //transform.position = new Vector3(locationIntro.position.x, locationIntro.position.y, locationIntro.position.z);

        /*for (int i = 0; i < 1; i++)
        {
            Object p1;
            p1 = Instantiate(Characters[p1Pos], playerSpawns[i].transform.position, Quaternion.identity);
            if (i == 1)
            {
                Object p2;
                p2 = Instantiate(Characters[p2Pos], playerSpawns[i].transform.position, Quaternion.identity);
            }
            //Characters[i].transform.position = playerSpawns[i].transform.position;
        }*/


    }

	// Use this for initialization
	void Start () {


	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
