using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public Canvas menuCanvas;
    public GameObject wall;
    /*float canvasScaleX = 0;
    float canvasScaleY = 0;
    float xBarrier = 0;
    float xOGBarrier = 0;
    float xBarrierLoc = 0;
    //float yBarrier = 0;*/
    public void Start()
    {
        /*Rect canvasSize = menuCanvas.pixelRect;
        canvasScaleX = canvasSize.width;
        canvasScaleY = canvasSize.height;
        Debug.LogError(canvasSize);
        //Use this to break the UI into four equal sizes
        xBarrier = canvasScaleX / 16;
        xOGBarrier = xBarrier;
        Debug.Log(xBarrier);
        for (int i = 0; i <= 3; i++)
        {
            Object clone;
            //xBarrier += xOGBarrier;
            xBarrierLoc += xOGBarrier;
            Debug.Log(xBarrierLoc);
            Debug.DrawRay(new Vector3(xBarrier, 20, 0), new Vector3(xBarrier, 0, 0), Color.green);
            wall.GetComponent<Transform>().localScale = new Vector3(5, canvasSize.height - 20, 1);
            clone = Instantiate(wall, new Vector3((xBarrierLoc), 0, 0), Quaternion.identity);
        }*/
    }
    public void PlayButtonClick()
    {
        //Application.LoadLevel(1);
        SceneManager.LoadScene(2);
    }
}
