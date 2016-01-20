using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AbilityTracker : MonoBehaviour
{
    public GameObject text;
    private GameObject[] abilityTexts;
    string[] trapNames;

    void Awake()
    {
        TrapControl.TrapInitEvent += InitalizeTextBoxes;
        TrapControl.TrapStatusEvent += UpdateUI;
    }

    void OnDestroy()
    {
        TrapControl.TrapInitEvent -= InitalizeTextBoxes;
        TrapControl.TrapStatusEvent -= UpdateUI;
    }

    void InitalizeTextBoxes(string[] names)
    {
        abilityTexts = new GameObject[names.Length];
        for(int i = 0; i < names.Length; i++)
        {
            abilityTexts[i] = (GameObject)Instantiate(text, new Vector3(92, 375 - (i * 20), 0), Quaternion.identity);
            abilityTexts[i].transform.SetParent(transform);
            abilityTexts[i].GetComponent<Text>().text = names[i] + ": Ready!";
        }
        trapNames = names;
    }
    
    void UpdateUI(string status, int index, float time)
    {
        abilityTexts[index].GetComponent<Text>().text = trapNames[index] + ": " + status;
    }

    /*IEnumerator CDTime(Text text, float time)
    {

    }*/
}
