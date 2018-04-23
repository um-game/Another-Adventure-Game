using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomNumberGenerator {

	private float total;
	private float[] probs;

	public RandomNumberGenerator(float[] probs) {

		foreach (float elem in probs) {
			total += elem;
		}

		// C# copies arrays automatically
		this.probs = probs;

		printProbs ();
	}

	public int next () {

		float randomPoint = Random.value * total;

		for (int i= 0; i < probs.Length; i++) {
			if (randomPoint < probs[i]) {
				return i;
			}
			else {
				randomPoint -= probs[i];
			}
		}
		return probs.Length - 1;
	}

	private void printProbs() {
		foreach (float elem in this.probs) {
			Debug.Log (elem);
		}
	}
}