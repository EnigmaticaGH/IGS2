using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ReadyButtons : MonoBehaviour {

    

    public Button[] ReadyBtns;
    public Sprite[] ReadyBtnClick;
    private Sprite[] originial = new Sprite[4];
    int ACount = 0;
    int ACount2 = 0;

    void OnEnable()
    {
        CursorMovement.P1ReadyEvent += Player1Button;
        CursorMovement.P2ReadyEvent += Player2Button;
        CursorMovement.P3ReadyEvent += Player3Button;
        CursorMovement.P4ReadyEvent += Player4Button;

    }

    void OnDisable()
    {
        CursorMovement.P1ReadyEvent -= Player1Button;
        CursorMovement.P2ReadyEvent -= Player2Button;
        CursorMovement.P3ReadyEvent -= Player3Button;
        CursorMovement.P4ReadyEvent -= Player4Button;



    }

	void Start () {

        for (int i = 0; i < ReadyBtns.Length; i++)
        {
            originial[i] = ReadyBtns[i].GetComponent<Button>().image.overrideSprite;
        }
	
	}
	
	// Update is called once per frame
	void Player1Button() {

        /*ColorBlock temp;
        temp = ReadyBtns[0].GetComponent<Button>().colors;
        temp.normalColor = Color.yellow;
        ReadyBtns[0].GetComponent<Button>().colors = temp; //Omg i finally changed a button's color via script yay me*/
        ReadyBtns[0].GetComponent<Button>().image.overrideSprite = ReadyBtnClick[0];
        ACount++;

        if (ACount >= 2) 
        {
            /*temp.normalColor = Color.white;
            ReadyBtns[0].GetComponent<Button>().colors = temp;*/
            ReadyBtns[0].GetComponent<Button>().image.overrideSprite = originial[0];
            ACount = 0;
        }

        Debug.Log("Button Clicked");

       
	
	}


    void Player2Button()
    {

        /*ColorBlock temp;
        temp = ReadyBtns[1].GetComponent<Button>().colors;
        temp.normalColor = Color.yellow;
        ReadyBtns[1].GetComponent<Button>().colors = temp; //Omg i finally changed a button's color via script yay me*/
        ReadyBtns[1].GetComponent<Button>().image.overrideSprite = ReadyBtnClick[1];

        ACount2++;

        if (ACount2 >= 2)
        {
            /*temp.normalColor = Color.white;
            ReadyBtns[0].GetComponent<Button>().colors = temp;*/
            ReadyBtns[1].GetComponent<Button>().image.overrideSprite = originial[1];
            ACount2 = 0;
        }

        Debug.Log("Button Clicked");



    }
    void Player3Button()
    {

        /*ColorBlock temp;
        temp = ReadyBtns[1].GetComponent<Button>().colors;
        temp.normalColor = Color.yellow;
        ReadyBtns[1].GetComponent<Button>().colors = temp; //Omg i finally changed a button's color via script yay me*/
        ReadyBtns[2].GetComponent<Button>().image.overrideSprite = ReadyBtnClick[2];

        ACount2++;

        if (ACount2 >= 2)
        {
            /*temp.normalColor = Color.white;
            ReadyBtns[0].GetComponent<Button>().colors = temp;*/
            ReadyBtns[2].GetComponent<Button>().image.overrideSprite = originial[2];
            ACount2 = 0;
        }

        Debug.Log("Button Clicked");



    }
    void Player4Button()
    {

        /*ColorBlock temp;
        temp = ReadyBtns[1].GetComponent<Button>().colors;
        temp.normalColor = Color.yellow;
        ReadyBtns[1].GetComponent<Button>().colors = temp; //Omg i finally changed a button's color via script yay me*/
        ReadyBtns[3].GetComponent<Button>().image.overrideSprite = ReadyBtnClick[3];

        ACount2++;

        if (ACount2 >= 2)
        {
            /*temp.normalColor = Color.white;
            ReadyBtns[0].GetComponent<Button>().colors = temp;*/
            ReadyBtns[3].GetComponent<Button>().image.overrideSprite = originial[3];
            ACount2 = 0;
        }

        Debug.Log("Button Clicked");



    }


}
