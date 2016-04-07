using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;


public class SceneLoader : MonoBehaviour {

    public Sprite[] LevelImages;

    private bool loadScene = false;
    public bool sceneLoad = false;

    [SerializeField]
    private int scene;

    public Image loadingImage;
    public Text loadingText;
    public Image Controller;

    public GameObject Controls;

    public GameObject levelManager;

    Color ogColor;

    Setup Setup;

    int cd = 0;


    void Start()
    {
        loadingImage.enabled = false;

        loadingText.enabled = false;

        ogColor = loadingText.color;

        Controller.enabled = false;

        Controls.SetActive(false);

        Setup = levelManager.GetComponent<Setup>();

    }

    void FixedUpdate()
    {
        sceneLoad = Setup.GetLoadScene();
    }

    // Updates once per frame
    void Update()
    {

        scene = GameObject.Find("LevelManager").GetComponent<Setup>().placeHolder;

        scene = scene + 3; //For main menu/character menu/game setup menu

        if (sceneLoad && cd == 0)
        {
            cd++;
            Debug.Log("Load Scene: " + (scene + 3));

            loadScene = true;

            Controller.enabled = true;

            StartCoroutine(LoadNewScene());

            loadingImage.enabled = true;

            loadingText.enabled = true;

            Controls.SetActive(true);

            //SceneManager.LoadScene(scene);
        }



        // If the new scene has started loading...
        if (loadScene == true)
        {

            // ...then pulse the transparency of the loading text to let the player know that the computer is still working.
            loadingText.color = new Color(loadingImage.color.r, loadingImage.color.g, loadingImage.color.b, Mathf.PingPong(Time.time, 1));

        }

    }


    // The coroutine runs on its own at the same time as Update() and takes an integer indicating which scene to load.
    IEnumerator LoadNewScene()
    {

        // This line waits for 3 seconds before executing the next line in the coroutine.
        // This line is only necessary for this demo. The scenes are so simple that they load too fast to read the "Loading..." text.
        yield return new WaitForSeconds(5);

        // Start an asynchronous operation to load the scene that was passed to the LoadNewScene coroutine.
        AsyncOperation async = SceneManager.LoadSceneAsync(scene); //FIX THIS
        //SceneManager.LoadScene(scene);
        

        // While the asynchronous operation to load the new scene is not yet complete, continue waiting until it's done. //FIX THIS
        while (!async.isDone)
        {
            yield return null;
        }

    }

}
