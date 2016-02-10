using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class AbilityTracker : MonoBehaviour
{
    public GameObject text;
    public GameObject winArea;
    private List<GameObject> abilities;
    private List<string> trapNames;
    private Dictionary<string, int> senderIndex;
    private GameObject distanceText;
    private GameObject livesText;
    private float winAreaPosition;
    static int i = 0;

    void Awake()
    {
        TrapControl.TrapInitEvent += InitalizeTextBoxes;
        TrapControl.TrapStatusEvent += UpdateAbilities;
        companionScript.AbilityInitEvent += InitalizeTextBoxes;
        companionScript.AbilityStatusEvent += UpdateAbilities;
        portalScriptPlayer.PortalInitEvent += InitalizeTextBoxes;
        portalScriptPlayer.PortalStatusEvent += UpdateAbilities;
        //DeathControl.OnHurt += UpdateLives;
        winAreaPosition = winArea.transform.position.x;

        distanceText = (GameObject)Instantiate(text, new Vector3(92, 20, 0), Quaternion.identity);
        distanceText.transform.SetParent(transform);
        distanceText.GetComponent<Text>().text = "Distance: ";
        distanceText.GetComponent<Text>().color = Color.white;

        livesText = (GameObject)Instantiate(text, new Vector3(92, 40, 0), Quaternion.identity);
        livesText.transform.SetParent(transform);
        livesText.GetComponent<Text>().text = "Lives: ";
        livesText.GetComponent<Text>().color = Color.white;

        abilities = new List<GameObject>();
        trapNames = new List<string>();
        senderIndex = new Dictionary<string, int>();
    }

    void OnDestroy()
    {
        TrapControl.TrapInitEvent -= InitalizeTextBoxes;
        TrapControl.TrapStatusEvent -= UpdateAbilities;
        companionScript.AbilityInitEvent -= InitalizeTextBoxes;
        companionScript.AbilityStatusEvent -= UpdateAbilities;
        portalScriptPlayer.PortalInitEvent -= InitalizeTextBoxes;
        portalScriptPlayer.PortalStatusEvent -= UpdateAbilities;
        //DeathControl.OnHurt -= UpdateLives;
    }

    void InitalizeTextBoxes(string[] names, string sender)
    {
        int j = 0;
        senderIndex.Add(sender, i);
        abilities.Add((GameObject)Instantiate(text, new Vector3(92, 395 - (i * 20), 0), Quaternion.identity));
        abilities[i].transform.SetParent(transform);
        abilities[i].GetComponent<Text>().color = Color.white;
        abilities[i++].GetComponent<Text>().text = sender + " Abilities";
        trapNames.Add(sender);
        foreach (string s in names)
        {
            trapNames.Add(s);
            abilities.Add((GameObject)Instantiate(text, new Vector3(92, 395 - (i * 20), 0), Quaternion.identity));
            abilities[i].transform.SetParent(transform);
            abilities[i].GetComponent<Text>().color = new Color(0.8f, 0.8f, 0.8f);
            abilities[i++].GetComponent<Text>().text = names[j++] + ":\tReady";
        }
    }

    void UpdatePlayerPosition(float position, string sender)
    {
        if (sender != "Player") return;
        float percentComplete = Mathf.Ceil((position / winAreaPosition) * 100);
        distanceText.GetComponent<Text>().text = "Distance: " + percentComplete.ToString("0") + "%";
    }

    void UpdateLives(int lives)
    {
        livesText.GetComponent<Text>().text = "Lives: " + lives;
    }
    
    void UpdateAbilities(string sender, string status, int index, float time)
    {
        int i = senderIndex[sender] + index + 1;
        if(status == "Cooldown")
        {
            abilities[i].GetComponent<Text>().text = trapNames[i] + ":\t";
            StartCoroutine(CDTime(abilities[i].GetComponent<Text>(), time));
        }
        else
        {
            abilities[i].GetComponent<Text>().text = trapNames[i] + ":\t" + status;
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
