using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour {

    Button newButton;
    Button loadButton;

    public string savePath = "Assets/Resources/save.txt";
    public bool loading = false;

    public static ButtonManager myManager;

    private void Awake()
    {
        if (myManager == null)
        {
            // If we're on a scene other than the main menu who cares
            if (GameObject.Find("NewButton") != null)
            {
                newButton = GameObject.Find("NewButton").GetComponent<Button>();
                loadButton = GameObject.Find("LoadButton").GetComponent<Button>();
            }
            

            myManager = this;
            DontDestroyOnLoad(this);
        }
        else if (myManager != this)
        {
            Destroy(gameObject);
        }
    }

    public void NewGame()
    {
        loading = false;
        SwitchScene();
    }

    public void LoadGame()
    {
        loading = true;
        SwitchScene();
    }

    public void SwitchScene()
    {
        /*
        float fadeTime = myFading.BeginFade(1);
        yield return new WaitForSeconds(fadeTime);
        */
        SceneManager.LoadScene(0);
    }
}
