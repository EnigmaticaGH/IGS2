using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class ProceduralGenerator : MonoBehaviour
{
    private Dictionary<string, GameObject> prefabs;
    private Dictionary<int, string> indexes;
    private Dictionary<int, GameObject> indexedGameObjects;

    public float ProceduralPartSize; //How big is each section?
    private int sectorX; //Which sector is the player currently in? Sectors are defined by the part size above
    private int oldSectorX; //Keep track of the sector the player was in during the previous frame

    void Start()
    {
        prefabs = new Dictionary<string, GameObject>();
        indexes = new Dictionary<int, string>();
        //We store generated sectors based on the generated prefab and the sector it was spawned in
        indexedGameObjects = new Dictionary<int, GameObject>();
        LoadAssets();
    }

    //Start us off with 3 sections surrounding the origin
    void InitalizeLevel()
    {
        indexedGameObjects.Add(-1, (GameObject)Instantiate(prefabs[indexes[(int)(Random.value * prefabs.Count)]],
            Vector3.left * ProceduralPartSize, Quaternion.identity));

        indexedGameObjects.Add(0, (GameObject)Instantiate(prefabs["Start"],
            Vector3.zero, Quaternion.identity));

        indexedGameObjects.Add(1, (GameObject)Instantiate(prefabs[indexes[(int)(Random.value * prefabs.Count)]],
            Vector3.right * ProceduralPartSize, Quaternion.identity));
    }

    void Update()
    {
        oldSectorX = sectorX;
        sectorX = (int)(transform.position.x / ProceduralPartSize);
        if(sectorX != oldSectorX)
        {
            //Whoo, we entered a new sector! Let's generate some stuff
            int direction = sectorX - oldSectorX;
            if (!indexedGameObjects.ContainsKey(sectorX))
            {
                //If the land under us doesn't exist, then make it so!
                Generate(sectorX);
            }
            if (!indexedGameObjects.ContainsKey(sectorX + direction))
            {
                //If the land ahoy doesn't exist, then make it so!
                Generate(sectorX + direction);
            }
            if (!indexedGameObjects.ContainsKey(sectorX + (direction * 2)))
            {
                //If the land ahoy ahoy doesn't exist, then make it so!
                Generate(sectorX + (direction * 2));
            }
            //We only want the stuff directly around us to do anything
            UpdateVisibleSectors();
        }
    }

    void Generate(int sector)
    {
        int i = (int)(Random.value * prefabs.Count);
        indexedGameObjects.Add(sector, (GameObject)Instantiate(prefabs[indexes[i]],
            Vector2.right * ProceduralPartSize * sector, Quaternion.identity));
    }

    void UpdateVisibleSectors()
    {
        foreach (KeyValuePair<int, GameObject> indexedGameObject in indexedGameObjects)
        {
            indexedGameObject.Value.SetActive(Mathf.Abs(indexedGameObject.Key - sectorX) <= 2);
        }
    }

    //Assets are loaded from /Assets/Resources/Procedural/
    void LoadAssets()
    {
        foreach (GameObject g in Resources.LoadAll("Procedural"))
        {
            //index each game object by their name and a number
            indexes.Add(prefabs.Count, g.name);
            prefabs.Add(g.name, g);
        }
        if (prefabs.Count > 0)
            InitalizeLevel();
    }
}
