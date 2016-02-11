using UnityEngine;
using System.Collections;

public class Player1Abilities : MonoBehaviour
{
    private int controllerNumber;
    public Object weapon;
    public float basicAttackCoolDown = .5f;
    public bool abilityA = false;
    private GameObject[] blocks;
    private Vector3 pos;
    int c = 0;
    GroundSensor sensor1; 

    //Assign new abilities here
    Ability[] abilities = new Ability[]
    {
        new Ability("Block Smash", "A", 5)
    };

	// Use this for initialization
	void Start ()
    {
        sensor1 = GameObject.Find("Player 1").GetComponentInChildren<GroundSensor>(); //FINALLY OMG this took forever smh XD
        controllerNumber = GetComponent<ControllerNumber>().controllerNumber;
        //Get a list of all blocks in the scene
        blocks = GameObject.FindGameObjectsWithTag("Block");
        foreach(Ability ability in abilities)
        {
            //Initially set all abilities to READY
            ability.AbilityStatus = Ability.Status.READY;
        }
        pos = blocks[0].transform.position;
        //c = sensor.CubeLength; //Used for control of ability A so it only affects three blocks during peroid
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
    void FixedUpdate()
    {
        
    }

    void OnColliderEnter(Collider col)
    {
        /*foreach (ContactPoint contact in col.contacts) {
            Debug.DrawRay(contact.point, contact.normal, Color.green);
        }*/
        if (col.tag == "Block")
        {
            Debug.Log(blocks);
            Debug.Log("Contact");
        }
    }

    

    IEnumerator Ability_A_Activate(Ability ability)
    {
        ability.AbilityStatus = Ability.Status.ACTIVE;
        // Ability code here
        //Debug.DrawRay(transform.position, blocks[0].transform.position);
        //blocks[0].GetComponent<Rigidbody>().useGravity = true; //Make object fall
        //blocks[0].GetComponent<Rigidbody>().isKinematic = false; //Allow it to be moved by gravity
        abilityA = true;
        
        // -----------------
        ability.AbilityStatus = Ability.Status.COOLDOWN;
        yield return new WaitForSeconds(ability.CooldownTime);
        if (sensor1.GetComponent<GroundSensor>().CubeLength > 2)
        {
            sensor1.GetComponent<GroundSensor>().CubeLength = 1;
        }
        
        abilityA = false;
        ability.AbilityStatus = Ability.Status.READY;
        blocks[0].transform.position = pos;
    }

    IEnumerator BasicAttack()
    {
        //weapon.gameObject.activeSelf(true);
        Object cloneAttack;
        cloneAttack = Instantiate(weapon, new Vector2(transform.position.x + .5f, transform.position.y + .02f), Quaternion.identity);
        yield return new WaitForSeconds(basicAttackCoolDown);
        Destroy(cloneAttack);
    }
}
