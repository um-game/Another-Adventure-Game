using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public static GameManager instance = null;

	private bool doingSetup;

	// Use this for initialization
	void Awake () {
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);

		DontDestroyOnLoad (gameObject);
		InitGame ();
	}

	void OnLevelWasLoaded (int index)
	{
		InitGame ();
	}

	void InitGame()
	{
		doingSetup = true;
	}

	public void GameOver()
	{
		enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (doingSetup)
			return;

	}


}