using UnityEngine;
using System.Collections;
using UnityEngine.PostProcessing;

public class HealthManagement : MonoBehaviour {

	public int thirstCounter = 600;
	public int hungerCounter = 1000;
	public bool hasdrunk = false;
	public bool haseaten = false;
	public PostProcessingBehaviour leftCamera;
	public PostProcessingBehaviour rightCamera;

	private IEnumerator thirstCoroutine;
	private IEnumerator hungerCoroutine;
	private int stressed = 0;
	// Use this for initialization
	void Start () {
		thirstCoroutine = Timer (thirstCounter);
		hungerCoroutine = Timer (hungerCounter);
		StartCoroutine (thirstCoroutine);
		StartCoroutine (hungerCoroutine);
	}
	
	// Update is called once per frame
	void Update () {
		// If player drinks, reset the counters
		if (hasdrunk) {
			StopCoroutine (thirstCoroutine);
			thirstCoroutine = Timer (thirstCounter);
			StartCoroutine (thirstCoroutine);
			hasdrunk = false;
			stressed--;
			if (stressed == 0) {
				leftCamera.enabled = false;
				rightCamera.enabled = false;
			}
		}

		// If player eats, reset the counters
		if (haseaten) {
			StopCoroutine (hungerCoroutine);
			hungerCoroutine = Timer (hungerCounter);
			StartCoroutine (hungerCoroutine);
			haseaten = false;
			stressed--;
			if (stressed == 0) {
				leftCamera.enabled = false;
				rightCamera.enabled = false;
			}
		}
			

			
	}

	// Timer co-routine
	private IEnumerator Timer(int time){
			yield return new WaitForSeconds (time);
		// When timer runs out, call alter view function and issue warning about being thirsty
			Debug.Log ("Thirsty or Hungry");
		leftCamera.enabled = true;
		rightCamera.enabled = true;
		stressed++;
			yield return new WaitForSeconds (time/2);
			Debug.Log ("Dead");
	}

	//}
					
	// Reset thirst or hunger counters

	// Alter view function

	// Issue warning function


}
