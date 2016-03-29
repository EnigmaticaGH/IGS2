using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ActiveControllers : MonoBehaviour {

    int ControllersActive = 0;

    public Sprite[] CharacterHeads;

    public Image[] PlayerHead;

    public Image[] UnActive;

    public string[] Names = new string[]{
        "Ash",
        "Annie",
        "Wayne",
        "Ned",
        "Disco"
    };

    public Text[] NamesLoc;

	// Use this for initialization
	void Awake () {

        int p1Pos = 0;
        int p2Pos = 0;
        int p3Pos = 0;
        int p4Pos = 0;
        p1Pos = CharacterMenuController.p1Pos; //Use this for spawning selected characters
        p2Pos = CharacterMenuController.p2Pos; //Use this for spawning selected characters
        p3Pos = CharacterMenuController.p3Pos; //Use this for spawning selected characters
        p4Pos = CharacterMenuController.p4Pos; //Use this for spawning selected characters

        ControllersActive = CharacterMenuController.ControllerNumber;



        for (int b = 0; b < UnActive.Length; b++)
        {
            UnActive[b].enabled = true;

            PlayerHead[b].sprite = CharacterHeads[0];

            NamesLoc[b].text = "";
         }

        for (int i = 0; i < ControllersActive; i++)
        {
            UnActive[i].enabled = false;


            switch (i)
            {
                case 0:
                    PlayerHead[i].sprite = CharacterHeads[p1Pos];
                    NamesLoc[i].text = Names[p1Pos];
                    break;
                case 1:
                    PlayerHead[i].sprite = CharacterHeads[p2Pos];
                    NamesLoc[i].text = Names[p2Pos];
                    break;
                case 2:
                    PlayerHead[i].sprite = CharacterHeads[p3Pos];
                    NamesLoc[i].text = Names[p3Pos];
                    break;
                case 3:
                    PlayerHead[i].sprite = CharacterHeads[p4Pos];
                    NamesLoc[i].text = Names[p4Pos];
                    break;
                default:
                    break;
            }

        }
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
