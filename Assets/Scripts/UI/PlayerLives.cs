using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerLives : MonoBehaviour {

    public GameObject player1;
    public Text player1Lives;
    public GameObject player2;
    public Text player2Lives;
    public GameObject player3;
    public Text player3Lives;
    public GameObject player4;
    public Text player4Lives;
    private int ControllerNumber;

    GameObject[] players;

    void Awake()
    {
        player1Lives.gameObject.SetActive(false);
        player2Lives.gameObject.SetActive(false);
        player3Lives.gameObject.SetActive(false);
        player4Lives.gameObject.SetActive(false);

        for (int j = 0; j < Input.GetJoystickNames().Length; j++)
        {

            Debug.Log(Input.GetJoystickNames()[j]);
            if (Input.GetJoystickNames()[j].Contains("Xbox"))
            {
                ControllerNumber++;
                Debug.Log("Controller " + ControllerNumber);
            }

            if (Input.GetJoystickNames()[j].Contains("XBOX"))
            {
                ControllerNumber++;
                Debug.Log("Controller " + ControllerNumber);
            }

            //ActiveControllers = Input.GetJoystickNames();

        }

        player1Lives.gameObject.SetActive(true);
        //Debug.Log(player1.GetComponent<DeathControl>().lives);
        if (CharacterMenuController.ControllerNumber > 1)
            player2Lives.gameObject.SetActive(true);
        if (CharacterMenuController.ControllerNumber > 2)
            player3Lives.gameObject.SetActive(true);
        if (CharacterMenuController.ControllerNumber > 3)
            player4Lives.gameObject.SetActive(true);
    }

	// Use this for initialization
	void Start () {

        //Debug.Log(player1.GetComponent<DeathControl>().getLives());

	}
	
	// Update is called once per frame
	void Update () {


        player1Lives.text = "Player 1 Lives: " + PlayerTracker.players[0].GetComponent<DeathControl>().getLives();
        //Debug.Log(player1.GetComponent<DeathControl>().getLives());
        if(CharacterMenuController.ControllerNumber > 1)
            player2Lives.text = "Player 2 Lives: " + PlayerTracker.players[1].GetComponent<DeathControl>().getLives();
        if (CharacterMenuController.ControllerNumber > 2)
            player3Lives.text = "Player 3 Lives " + PlayerTracker.players[2].GetComponent<DeathControl>().getLives();
        if (CharacterMenuController.ControllerNumber > 3)
            player4Lives.text = "Player 4 Lives " + PlayerTracker.players[3].GetComponent<DeathControl>().getLives();
	
	}
}
