using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour {

    public string savePath = "Assets/Resources/save.txt";
    public bool loading = false;

    public static ButtonManager myManager;

    private void Awake()
    {
        if (myManager == null)
        {
            
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
