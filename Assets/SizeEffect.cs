using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SizeEffect : MonoBehaviour {

    public float value = 5;
    public Image LOGO;

    private float yVelocity = 0.0F;

    float startX;
    float startY;

    bool restart = false;

    Rect recta;

    // Use this for initialization
    void Start()
    {


        startX = LOGO.rectTransform.localScale.x;
        startY = LOGO.rectTransform.localScale.y;
    }

    // Update is called once per frame
    void Update()
    {



        /*if (LOGO.rectTransform.localScale.x == startX) 
        {
            restart = false;
            Debug.Log("WTF MAN");

        }
        if (LOGO.rectTransform.localScale.x == startX + 0.017f)
        {
            restart = true;
            Debug.Log("RESTART MEEEEE");
        }

        if(restart)
            LOGO.rectTransform.localScale = new Vector2(Mathf.Lerp(startX + .02f, startX, Time.time), Mathf.Lerp(startY + .02f, startY, Time.time));
        if (!restart)
            LOGO.rectTransform.localScale = new Vector2(Mathf.Lerp(startX, startX + .02f, Time.time), Mathf.Lerp(startY, startY + .02f, Time.time));*/


        LOGO.rectTransform.localScale = new Vector2(startX + (.01f * Mathf.Sin(.5f * Time.time)), startY + (0.013f * Mathf.Sin(.5f * Time.time)));

    }
}
