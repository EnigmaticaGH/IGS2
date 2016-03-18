using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    void Update()
    {
        if (Input.GetButtonDown("A_1") || Input.GetButtonDown("A_2") || Input.GetButtonDown("A_3") || Input.GetButtonDown("A_4"))
        {
            PlayButtonClick();
        }
    }
    public void PlayButtonClick()
    {
        //Application.LoadLevel(1);
        SceneManager.LoadScene(1);
    }
}
