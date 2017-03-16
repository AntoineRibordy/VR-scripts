using UnityEngine;
using System.Collections;

public class Glow : MonoBehaviour {
	private Material mat;

	public float glowValue = 0.2f;
	public bool pickedUp = false;
	// Use this for initialization
	void Start () {
		mat = GetComponent<Renderer>().material;
	}

	// Update is called once per frame
	void Update () {
		if (!pickedUp) {
			float emission = Mathf.PingPong (Time.time, 1.0f);
			Color baseColor = Color.white; //Replace this with whatever you want for your base color at emission level '1'

			Color finalColor = baseColor * glowValue * Mathf.LinearToGammaSpace (emission);

			mat.SetColor ("_EmissionColor", finalColor);
		}
	
	}
}
