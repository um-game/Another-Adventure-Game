using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsPanel : MonoBehaviour {

	public static StatsPanel myStats;

	private Player player;
	private Text atk;
	private Text def;
	private Text spd;

	// Use this for initialization
	void Start () {

		if (myStats == null) {

			player = GameObject.Find ("player").GetComponent<Player>();

			atk = this.transform.GetChild (0).GetComponent<Text> ();
			def = this.transform.GetChild (1).GetComponent<Text> ();
			spd = this.transform.GetChild (2).GetComponent<Text> ();

			atk.text = "Attack: " + player.attack.ToString ();
			def.text = "Defense: " + player.defense.ToString();
			spd.text = "Speed: " + player.maxSpeed.ToString();

			myStats = this;


		} else if (myStats != this) {
			Destroy (gameObject);
		}

	}

	public void updateStats()
	{
		atk.text = "Attack: " + player.attack.ToString ();
		def.text = "Defense: " + player.defense.ToString();
		spd.text = "Speed: " + player.maxSpeed.ToString();
	}

	// Update is called once per frame
	void Update () {

	}

	public void toggleActive()
	{
		this.gameObject.SetActive (!this.gameObject.activeSelf);
	}
}