using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public void PlayButtonClick()
    {
        //Application.LoadLevel(1);
        SceneManager.LoadScene(1);
    }
}
