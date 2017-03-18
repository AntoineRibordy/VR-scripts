using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.PostProcessing;

public class HealthManagement : MonoBehaviour {

	public GameObject player;
	public int thirstCounter = 600;
	public int hungerCounter = 1000;
	public bool hasdrunk = false;
	public bool haseaten = false;
	public PostProcessingBehaviour leftCamera;
	public PostProcessingBehaviour rightCamera;
	public GameObject thirstyCanvas;
	public GameObject hungryCanvas;
	public float offsetObjectFront = 8.0f;

	private IEnumerator thirstCoroutine;
	private IEnumerator hungerCoroutine;
	private int stressed = 0;
	private MusicPicker musicPicker; 
	private EndGame endgame;
	// Use this for initialization
	void Start () {
		thirstCoroutine = ThirstTimer ();
		hungerCoroutine = HungerTimer ();
		StartCoroutine (thirstCoroutine);
		StartCoroutine (hungerCoroutine);
		musicPicker = FindObjectOfType<MusicPicker> ();
		endgame = FindObjectOfType<EndGame>();
	}
	
	// Update is called once per frame
	void Update () {
		// If player drinks, reset the counters
		if (hasdrunk) {
			StopCoroutine (thirstCoroutine);
			thirstCoroutine = ThirstTimer ();
			StartCoroutine (thirstCoroutine);
			hasdrunk = false;
			stressed--;
			if (stressed == 0) {
				leftCamera.enabled = false;
				rightCamera.enabled = false;
				musicPicker.StopClip ();
				thirstyCanvas.SetActive (false);
			}
		}

		// If player eats, reset the counters
		if (haseaten) {
			StopCoroutine (hungerCoroutine);
			hungerCoroutine = HungerTimer ();
			StartCoroutine (hungerCoroutine);
			haseaten = false;
			stressed--;
			if (stressed == 0) {
				leftCamera.enabled = false;
				rightCamera.enabled = false;
				musicPicker.StopClip ();
				hungryCanvas.SetActive (false);
			}
		}
			
	}

	// Timer co-routine
	private IEnumerator ThirstTimer(){
		yield return new WaitForSeconds (thirstCounter);
		// When timer runs out, call alter view function and issue warning about being thirsty
		//Debug.Log ("Thirsty");
		StartCoroutine(DisplayStressGUI (thirstyCanvas));
		StartCoroutine(Stressed());
		yield return new WaitForSeconds (thirstCounter/2);
		string level = "GameOver";
		endgame.EndTheGame (level, 8.0f);
	}

	private IEnumerator HungerTimer(){
		yield return new WaitForSeconds (hungerCounter);
		// When timer runs out, call alter view function and issue warning about being hungry
		//Debug.Log ("Hungry");
		StartCoroutine(DisplayStressGUI (hungryCanvas));
		StartCoroutine(Stressed());
		yield return new WaitForSeconds (hungerCounter/2);
		string level = "GameOver";
		endgame.EndTheGame (level, 8.0f);
	}

	private IEnumerator Stressed (){
		leftCamera.enabled = true;
		rightCamera.enabled = true;
		// Play clips to show player he needs to drink or eat
		AudioClip unwellClip = musicPicker.unwellClip;
		musicPicker.PlayClip (unwellClip);
		yield return new WaitForSeconds (unwellClip.length-0.1f);
		musicPicker.StopClip ();
		AudioClip stressClip = musicPicker.stressClip;
		musicPicker.PlayClip (stressClip);	
		stressed++;
	}

	// Display GUI to warn player that they're thirsty or hungry
	private IEnumerator DisplayStressGUI (GameObject canvas){
		PositionCanvas (canvas);
		canvas.SetActive (true);
		yield return new WaitForSeconds (5.0f);
		canvas.SetActive (false);
	}

	private void PositionCanvas(GameObject canvas){
		Vector3 offset = offsetObjectFront * Vector3.forward;
		canvas.transform.position = player.transform.position + new Vector3 (0, 1.0f, 0) + player.transform.rotation * offset;
		canvas.transform.rotation = player.transform.rotation;
		canvas.transform.parent = player.transform;
	}


}
