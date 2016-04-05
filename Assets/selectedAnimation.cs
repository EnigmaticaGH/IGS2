using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class selectedAnimation : MonoBehaviour {

    public Image object1;

    public float Buffer = 10;

    Rect Size;
    float Width;
    float Height;
    int i = 0;
    float bigWidth = 0;
    float bigHeight = 0;
    float smallWidth = 0;
    float smallHeight = 0;

    Animator anim;


    // Use this for initialization
    void Start () {

        Size = object1.GetComponent<RectTransform>().rect;

        anim = GetComponent<Animator>();

        anim.Play("UPPERLEFT");

        Width = Size.width;

        Height = Size.height;


        bigWidth = Width + Buffer;
        bigHeight = Height + Buffer;
        smallWidth = Width - Buffer;
        smallHeight = Height - Buffer;

        StartCoroutine("Big");

    }



    IEnumerator Big()
    {

        i++;

        Debug.Log(i);
        Debug.Log(bigHeight);
        Debug.Log(bigWidth);

        object1.GetComponent<RectTransform>().sizeDelta = new Vector2(bigWidth, bigHeight);

        yield return new WaitForSeconds(2);

        i++;

        Debug.Log(i);
        Debug.Log(smallHeight);
        Debug.Log(smallWidth);

        object1.GetComponent<RectTransform>().sizeDelta = new Vector2(smallWidth, smallHeight);

        yield return new WaitForSeconds(2f);
        yield return StartCoroutine("Big");
    }

}
