using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

public class Tutorial : MonoBehaviour
{
    public static Tutorial Instance;

    public static GameObject TutorialPanel;
    public static GameObject SteamStick;
    public static GameObject SteamRT;
    public static GameObject SteamX;
    public static GameObject SteamY;
    public static GameObject KeyboardA;
    public static GameObject KeyboardD;
    public static GameObject KeyboardS;
    public static GameObject KeyboardW;
    public static GameObject KeyboardH;
    public static GameObject KeyboardI;
    public static GameObject MouseL;
    public static GameObject TutorialText;
    public static Text text;

    public bool isEnabled = false;
    public bool learnedMovement = false;
    public bool learnedTutorial = false;
    public bool learnedInventory = false;
    public bool learnedAttack = false;
    public bool displayMovement = false;
    public bool displayTutorial = false;
    public bool displayInventory = false;
    public bool displayAttack = false;

    void Start()
    {
        if (Instance == null)
        {
            Debug.Log("Tutorial!");
            TutorialPanel = GameObject.Find("TutorialPanel");
            Debug.Log(TutorialPanel);
            SteamStick = GameObject.Find("Steam_Stick");
            Debug.Log(SteamStick);
            SteamRT = GameObject.Find("Steam_RT");
            SteamX = GameObject.Find("Steam_X");
            SteamY = GameObject.Find("Steam_Y");
            KeyboardA = GameObject.Find("Keyboard_Black_A");
            KeyboardD = GameObject.Find("Keyboard_Black_D");
            KeyboardS = GameObject.Find("Keyboard_Black_S");
            KeyboardW = GameObject.Find("Keyboard_Black_W");
            KeyboardI = GameObject.Find("Keyboard_Black_I");
            KeyboardH = GameObject.Find("Keyboard_Black_H");
            MouseL = GameObject.Find("Keyboard_Black_Mouse_Left");
            TutorialText = GameObject.Find("TutorialText");
            text = TutorialText.GetComponent<Text>();

            hideMovement();
            hideTutorial();
            hideInventory();
            hideAttack();

            showTutorial();

            Instance = this;
        } else if (Instance != this)
        {

        }
    }

    public void toggle()
    {
        isEnabled = !isEnabled;
        
        if (isEnabled)
        {
            hideTutorial();
            TutorialText.SetActive(true);
            text.text = "Tutorial Enabled!";
            StartCoroutine(moveAround());
            learnedTutorial = true;
        }
        else
        {
            TutorialText.SetActive(true);
            text.text = "Tutorial Disabled!";
        }

        StartCoroutine(fadeText());

        //TODO: tutorial text tiemr
    }

    IEnumerator fadeText()
    {
        yield return new WaitForSeconds(5);
        TutorialText.SetActive(false);
    }

    IEnumerator fadeTutorial()
    {
        yield return new WaitForSeconds(30);
        hideTutorial();
    }

    IEnumerator fadeMovement()
    {
        yield return new WaitForSeconds(30);
        hideMovement();
    }

    IEnumerator fadeInventory()
    {
        yield return new WaitForSeconds(30);
        hideInventory();
    }

    IEnumerator fadeAttack()
    {
        yield return new WaitForSeconds(30);
        hideAttack();
    }

    IEnumerator moveAround()
    {
        yield return new WaitForSeconds(10);
        showMovement();
    }

    void showMovement()
    {
        if (isEnabled && learnedMovement == false)
        {
            displayMovement = true;
            SteamStick.SetActive(true);
            KeyboardA.SetActive(true);
            KeyboardD.SetActive(true);
            KeyboardS.SetActive(true);
            KeyboardW.SetActive(true);
            TutorialText.SetActive(true);
            text.text = "Move Around";
            StartCoroutine(fadeMovement());
        }
    }

    void hideMovement()
    {
        displayMovement = false;
        SteamStick.SetActive(false);
        KeyboardA.SetActive(false);
        KeyboardD.SetActive(false);
        KeyboardS.SetActive(false);
        KeyboardW.SetActive(false);
        TutorialText.SetActive(false);
    }

    void showTutorial()
    {
        if (!learnedTutorial)
        {
            displayTutorial = true;
            SteamX.SetActive(true);
            KeyboardH.SetActive(true);
            TutorialText.SetActive(true);
            text.text = "Toggle Tutorial";
            StartCoroutine(fadeTutorial());
        }
    }

    void hideTutorial()
    {
        displayTutorial = false;
        SteamX.SetActive(false);
        KeyboardH.SetActive(false);
        TutorialText.SetActive(false);
    }

    void showInventory()
    {
        if (isEnabled && learnedInventory == false)
        {
            displayInventory = true;
            SteamY.SetActive(true);
            KeyboardI.SetActive(true);
            TutorialText.SetActive(true);
            text.text = "Open Inventory";
            StartCoroutine(fadeInventory());
        }
    }

    void hideInventory()
    {
        displayInventory = false;
        SteamY.SetActive(false);
        KeyboardI.SetActive(false);
        TutorialText.SetActive(false);
    }

    void showAttack()
    {
        if (isEnabled)
        {
            displayAttack = true;
            SteamRT.SetActive(true);
            MouseL.SetActive(true);
            TutorialText.SetActive(true);
            text.text = "Use Attack";
            StartCoroutine(fadeAttack());
        }
    }

    void hideAttack()
    {
        displayAttack = false;
        SteamRT.SetActive(false);
        MouseL.SetActive(false);
        TutorialText.SetActive(false);
    }

    public void learnMovement()
    {
        if (isEnabled && displayMovement)
        {
            learnedMovement = true;
            hideMovement();
        }
    }

    public void learnInventory()
    {
        if (isEnabled && displayInventory)
        {
            learnedInventory = true;
            hideInventory();
        }
    }

    public void learnAttack()
    {
        if (isEnabled && displayAttack)
        {
            learnedAttack = true;
            hideAttack();
        }
    }

}
