using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public AudioClip Click;

    public AudioSource Object;

    public Text loadingText;

    bool loadScene = false;


    string[] Buttons = {
                           "A_",
                           "Start_"
                       };

    void Start() 
    {
        loadingText.enabled = false;
    }

    void Update()
    {
        for (int i = 1; i <= 4; i++)
        {
            for (int b = 0; b <= 1; b++)
            {
                if (Input.GetButtonUp(Buttons[b] + i))
                {
                    loadScene = true;
                    loadingText.enabled = true;
                    PlayButtonClick();
                    Object.clip = Click;
                    Object.Play();
                }
            }
        }

        // If the new scene has started loading...
        if (loadScene == true)
        {

            // ...then pulse the transparency of the loading text to let the player know that the computer is still working.
            loadingText.color = new Color(loadingText.color.r, loadingText.color.g, loadingText.color.b, Mathf.PingPong(Time.time, 1));

        }

    }
    public void PlayButtonClick()
    {
        StartCoroutine("LoadNewScene");
    }

    IEnumerator LoadNewScene()
    {

        yield return new WaitForSeconds(.00001f);

        AsyncOperation async = SceneManager.LoadSceneAsync(1); //FIX THIS


        while (!async.isDone)
        {
            yield return null;
        }

    }
}
