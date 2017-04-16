using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Glow : MonoBehaviour {
	private Material[] mat;
	private bool alteredEmissionColor = true;

	public float glowValue = 0.2f;
	public bool pickedUp = false;
	// Use this for initialization
	void Start () {
		mat = GetComponent<Renderer>().materials;
	}

	// Update is called once per frame
	void Update () {
		if (!pickedUp) {
			float emission = Mathf.PingPong (Time.time, 1.0f);
			Color baseColor = Color.white; //Replace this with whatever you want for your base color at emission level '1'

			Color finalColor = baseColor * glowValue * Mathf.LinearToGammaSpace (emission);
			for (int i = 0; i < mat.Length; i++) {
				mat [i].SetColor ("_EmissionColor", finalColor);
			}
		} else if (alteredEmissionColor) {
			for (int i = 0; i < mat.Length; i++) {
				mat [i].SetColor ("_EmissionColor", Color.black);
			}
			alteredEmissionColor = false;
		}
	
	}
}
