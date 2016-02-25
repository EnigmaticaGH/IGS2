using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ReadyButtons : MonoBehaviour {

    

    public Button[] ReadyBtns;
    int ACount = 0;
    int ACount2 = 0;

    void OnEnable()
    {
        CursorMovement.P1ReadyEvent += Player1Button;
        CursorMovement.P2ReadyEvent += Player2Button;
    }

    void OnDisable()
    {
        CursorMovement.P1ReadyEvent -= Player1Button;
        CursorMovement.P2ReadyEvent -= Player2Button;

    }

	void Start () {
	
	}
	
	// Update is called once per frame
	void Player1Button() {

        ColorBlock temp;
        temp = ReadyBtns[0].GetComponent<Button>().colors;
        temp.normalColor = Color.yellow;
        ReadyBtns[0].GetComponent<Button>().colors = temp; //Omg i finally changed a button's color via script yay me

        ACount++;

        if (ACount >= 2) 
        {
            temp.normalColor = Color.white;
            ReadyBtns[0].GetComponent<Button>().colors = temp;
            ACount = 0;
        }

        Debug.Log("Button Clicked");

       
	
	}


    void Player2Button()
    {

        ColorBlock temp;
        temp = ReadyBtns[1].GetComponent<Button>().colors;
        temp.normalColor = Color.yellow;
        ReadyBtns[1].GetComponent<Button>().colors = temp; //Omg i finally changed a button's color via script yay me

        ACount2++;

        if (ACount2 >= 2)
        {
            temp.normalColor = Color.white;
            ReadyBtns[0].GetComponent<Button>().colors = temp;
            ACount2 = 0;
        }

        Debug.Log("Button Clicked");



    }


}
