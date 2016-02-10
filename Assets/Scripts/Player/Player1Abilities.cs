using UnityEngine;
using System.Collections;

public class Player1Abilities : MonoBehaviour
{
    private int controllerNumber;
    private GameObject[] blocks;
    private Vector3 pos;
    //Assign new abilities here
    Ability[] abilities = new Ability[]
    {
        new Ability("Block Smash", "A", 5)
    };

	// Use this for initialization
	void Start ()
    {
        controllerNumber = GetComponent<ControllerNumber>().controllerNumber;
        //Get a list of all blocks in the scene
        blocks = GameObject.FindGameObjectsWithTag("Block");
        foreach(Ability ability in abilities)
        {
            //Initially set all abilities to READY
            ability.AbilityStatus = Ability.Status.READY;
        }
        pos = blocks[0].transform.position;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if(Input.GetKeyDown(KeyCode.T) && abilities[0].AbilityStatus == Ability.Status.READY)
        {
            //Only for testing
            StartCoroutine("Ability_" + abilities[0].Button + "_Activate", abilities[0]);
        }
        foreach (Ability ability in abilities)
        {
            //Check for input on abilities
            if (Input.GetButton(ability.Button + "_" + controllerNumber) && ability.AbilityStatus == Ability.Status.READY)
            {
                StartCoroutine("Ability_" + ability.Button + "_Activate", ability);
            }
        }
    }

    IEnumerator Ability_A_Activate(Ability ability)
    {
        ability.AbilityStatus = Ability.Status.ACTIVE;
        // Ability code here
        blocks[0].GetComponent<Rigidbody>().useGravity = true; //Make object fall
        blocks[0].GetComponent<Rigidbody>().isKinematic = false; //Allow it to be moved by gravity
        // -----------------
        ability.AbilityStatus = Ability.Status.COOLDOWN;
        yield return new WaitForSeconds(ability.CooldownTime);
        ability.AbilityStatus = Ability.Status.READY;
        blocks[0].transform.position = pos;
    }
}
