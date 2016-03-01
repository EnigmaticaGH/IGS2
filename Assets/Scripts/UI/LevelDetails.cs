using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelDetails : MonoBehaviour {

    public Button[] Levels;

    public Text LevelDetailsText;
    public Text SelectedText;

    int one = 0, two = 0, three = 0, four = 0;

    void Awake()
    {
    }

    void OnEnable() 
    {
        CursorMovement.Level1Hover += Level1Detail;
        CursorMovement.Level1Load += Level1Loads;
        CursorMovement.Level2Load += Level2Loads;
        CursorMovement.Level2Hover += Level2Detail;
        CursorMovement.Level3Load += Level3Loads;
        CursorMovement.Level3Hover += Level3Detail;
        CursorMovement.Level4Hover += Level4Detail;
        CursorMovement.Level4Load += Level4Loads;
        
    }

    void OnDisable()
    {
        LevelDetailsText.text = "";
        CursorMovement.Level1Hover -= Level1Detail;
        CursorMovement.Level2Load -= Level2Loads;
        CursorMovement.Level1Load -= Level1Loads;
        CursorMovement.Level2Hover -= Level2Detail;
        CursorMovement.Level3Load -= Level3Loads;
        CursorMovement.Level3Hover -= Level3Detail;
        CursorMovement.Level4Hover -= Level4Detail;
        CursorMovement.Level4Load -= Level4Loads;
    }

    void Level1Detail()
    {
        one++;
        if(one == 1)
            StartCoroutine("resetText1");
        LevelDetailsText.text = "A simple level meant for easy enjoyment! Test your skills against friends in Level 1";
    }

    void Level2Detail()
    {
        two++;
        if(two == 1)
            StartCoroutine("resetText2");
        LevelDetailsText.text = "Level 2: Great starter level to get a real feel for the mechiancs";

    }

    void Level3Detail()
    {
        three++;
        if(three == 1)
            StartCoroutine("resetText3");
        LevelDetailsText.text = "Level 3: Looks like a face! Launch some blocks at eachother!";
    }

    void Level4Detail()
    {
        four++;
        if(four == 1)
            StartCoroutine("resetText4");
        LevelDetailsText.text = "Level 4: Watch out the blocks aren't kinematic in this intense thriller";
    }

    IEnumerator resetText1()
    {
        yield return new WaitForSeconds(10);
        Debug.Log("work" + SelectedText.text);
        LevelDetailsText.text = SelectedText.text;
        one = 0;
    }
    IEnumerator resetText2()
    {
        yield return new WaitForSeconds(10);
        Debug.Log("work" + SelectedText.text);
        LevelDetailsText.text = SelectedText.text;
        two = 0;
    }
    IEnumerator resetText3()
    {
        yield return new WaitForSeconds(10);
        Debug.Log("work" + SelectedText.text);
        LevelDetailsText.text = SelectedText.text;
        three = 0;
    }
    IEnumerator resetText4()
    {
        yield return new WaitForSeconds(10);
        Debug.Log("work" + SelectedText.text);
        LevelDetailsText.text = SelectedText.text;//Setting up temp to reset level details to selected string
        four = 0;
    }


    void Level1Loads() 
    {
        ColorBlock temp;
        temp = Levels[0].GetComponent<Button>().colors;
        temp.normalColor = Color.yellow;
        Levels[0].GetComponent<Button>().colors = temp; 
        temp.normalColor = Color.white;
        Levels[1].GetComponent<Button>().colors = temp;
        Levels[2].GetComponent<Button>().colors = temp;
        Levels[3].GetComponent<Button>().colors = temp;
        SelectedText.text = LevelDetailsText.text; //Setting up temp to reset level details to selected string
        Debug.Log(SelectedText.text);
    }

    void Level2Loads()
    {
        ColorBlock temp;
        temp = Levels[0].GetComponent<Button>().colors;
        temp.normalColor = Color.yellow;
        Levels[1].GetComponent<Button>().colors = temp; 
        temp.normalColor = Color.white;
        Levels[0].GetComponent<Button>().colors = temp;
        Levels[2].GetComponent<Button>().colors = temp;
        Levels[3].GetComponent<Button>().colors = temp;        
        SelectedText.text = LevelDetailsText.text; //Setting up temp to reset level details to selected string
        Debug.Log(SelectedText.text);


    }

    void Level3Loads()
    {
        ColorBlock temp;
        temp = Levels[0].GetComponent<Button>().colors;
        temp.normalColor = Color.yellow;
        Levels[2].GetComponent<Button>().colors = temp; 
        temp.normalColor = Color.white;
        Levels[0].GetComponent<Button>().colors = temp;
        Levels[1].GetComponent<Button>().colors = temp;
        Levels[3].GetComponent<Button>().colors = temp;
        SelectedText.text = LevelDetailsText.text; //Setting up temp to reset level details to selected string
        Debug.Log(SelectedText.text);


    }

    void Level4Loads()
    {
        ColorBlock temp;
        temp = Levels[0].GetComponent<Button>().colors;
        temp.normalColor = Color.yellow;
        Levels[3].GetComponent<Button>().colors = temp; 
        temp.normalColor = Color.white;
        Levels[0].GetComponent<Button>().colors = temp;
        Levels[1].GetComponent<Button>().colors = temp;
        Levels[2].GetComponent<Button>().colors = temp;
        SelectedText.text = LevelDetailsText.text; //Setting up temp to reset level details to selected string
        Debug.Log(SelectedText.text);



    }


	
	// Update is called once per frame
	void Update () {

        
	
	}
}
