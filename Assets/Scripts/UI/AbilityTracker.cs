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
            abilityTexts[i].GetComponent<Text>().text = names[i] + ":\tReady";
        }
        trapNames = names;
    }
    
    void UpdateUI(string status, int index, float time)
    {
        if(status == "Cooldown")
        {
            abilityTexts[index].GetComponent<Text>().text = trapNames[index] + ":\t";
            StartCoroutine(CDTime(abilityTexts[index].GetComponent<Text>(), time));
        }
        else
        {
            abilityTexts[index].GetComponent<Text>().text = trapNames[index] + ":\t" + status;
        }
    }

    IEnumerator CDTime(Text text, float time)
    {
        float t = time;
        string s = text.text;
        while(t > 0)
        {
            text.text = s + t.ToString("0.0s");
            yield return new WaitForFixedUpdate();
            t -= Time.deltaTime;
        }
    }
}
