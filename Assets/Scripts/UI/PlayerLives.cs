using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerLives : MonoBehaviour {

    public GameObject player1;
    public Text player1Lives;
    public GameObject player2;
    public Text player2Lives;

	// Use this for initialization
	void Start () {

        Debug.Log(player1.GetComponent<DeathControl>().getLives());

        //player1Lives.text = "Player 1 Lives: " + player1.GetComponent<DeathControl>().numberOfLives.ToString(); //.ToString();
        //player2Lives.text = "Player 2 Lives: " + player1.GetComponent<DeathControl>().numberOfLives.ToString();
	
	}
	
	// Update is called once per frame
	void Update () {

        player1Lives.text = "Player 1 Lives: " + player1.GetComponent<DeathControl>().getLives();
        if(CharacterMenuController.ControllerNumber > 1)
            player2Lives.text = "Player 2 Lives: " + player2.GetComponent<DeathControl>().getLives();

	
	}
}
