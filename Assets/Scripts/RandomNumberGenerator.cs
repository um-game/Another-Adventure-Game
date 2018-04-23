using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Random number generator weighted by probabilities
// Adapted from Choose funcion given at
// https://docs.unity3d.com/Manual/RandomNumbers.html
public class RandomNumberGenerator {

	private float total;
	private float[] probs;

	public RandomNumberGenerator(float[] probs) {
		foreach (float elem in probs) {
			total += elem;
		}

		// C# copies arrays automatically
		this.probs = probs;
	}

	public int next () {
		// Generate random point
		float randomPoint = Random.value * total;

		// Figure out where it falls(using cumulative probability)
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