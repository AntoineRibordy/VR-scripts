using UnityEngine;
using System.Collections;

public class cutSmallObjects : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Camera camera = GetComponent<Camera>();
		float[] distances = new float[32];
		distances [8] = 100;
		camera.layerCullDistances = distances;
	}
}
