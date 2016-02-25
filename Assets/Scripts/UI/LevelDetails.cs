using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelDetails : MonoBehaviour {

    public Button[] Levels;

    public Text LevelDetailsText;

    void OnEnable() 
    {
        CursorMovement.Level1Hover += Level1Detail;
        CursorMovement.Level1Load += Level1Load;
        CursorMovement.Level2Load += Level2Load;
        CursorMovement.Level2Hover += Level2Detail;
        CursorMovement.Level3Load += Level3Load;
        CursorMovement.Level3Hover += Level3Detail;
        CursorMovement.Level4Hover += Level4Detail;
        CursorMovement.Level4Load += Level4Load;
        
    }

    void OnDisable()
    {
        LevelDetailsText.text = "";
        CursorMovement.Level1Hover -= Level1Detail;
        CursorMovement.Level2Load -= Level2Load;
        CursorMovement.Level1Load -= Level1Load;
        CursorMovement.Level2Hover -= Level2Detail;
        CursorMovement.Level3Load -= Level3Load;
        CursorMovement.Level3Hover -= Level3Detail;
        CursorMovement.Level4Hover -= Level4Detail;
        CursorMovement.Level4Load -= Level4Load;
    }

    void Level1Detail()
    {
        LevelDetailsText.text = "A simple level meant for easy enjoyment! Test your skills against friends in Level 1";
    }

    void Level2Detail()
    {
        LevelDetailsText.text = "Level 2: Great starter level to get a real feel for the mechiancs";
    }

    void Level3Detail()
    {
        LevelDetailsText.text = "Level 3: Looks like a face! Launch some blocks at eachother!";
    }

    void Level4Detail()
    {
        LevelDetailsText.text = "Level 4: Watch out the blocks aren't kinematic in this intense thriller";
    }


    void Level1Load() 
    {
        ColorBlock temp;
        temp = Levels[0].GetComponent<Button>().colors;
        temp.normalColor = Color.yellow;
        Levels[0].GetComponent<Button>().colors = temp; 
        temp.normalColor = Color.white;
        Levels[1].GetComponent<Button>().colors = temp;
        Levels[2].GetComponent<Button>().colors = temp;
        Levels[3].GetComponent<Button>().colors = temp;
    }

    void Level2Load()
    {
        ColorBlock temp;
        temp = Levels[0].GetComponent<Button>().colors;
        temp.normalColor = Color.yellow;
        Levels[1].GetComponent<Button>().colors = temp; 
        temp.normalColor = Color.white;
        Levels[0].GetComponent<Button>().colors = temp;
        Levels[2].GetComponent<Button>().colors = temp;
        Levels[3].GetComponent<Button>().colors = temp;

    }

    void Level3Load()
    {
        ColorBlock temp;
        temp = Levels[0].GetComponent<Button>().colors;
        temp.normalColor = Color.yellow;
        Levels[2].GetComponent<Button>().colors = temp; 
        temp.normalColor = Color.white;
        Levels[0].GetComponent<Button>().colors = temp;
        Levels[1].GetComponent<Button>().colors = temp;
        Levels[3].GetComponent<Button>().colors = temp;

    }

    void Level4Load()
    {
        ColorBlock temp;
        temp = Levels[0].GetComponent<Button>().colors;
        temp.normalColor = Color.yellow;
        Levels[3].GetComponent<Button>().colors = temp; 
        temp.normalColor = Color.white;
        Levels[0].GetComponent<Button>().colors = temp;
        Levels[1].GetComponent<Button>().colors = temp;
        Levels[2].GetComponent<Button>().colors = temp; 


    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        
	
	}
}
