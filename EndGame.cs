using UnityEngine;
using System.Collections;

public class EndGame : MonoBehaviour {
	public float waitTime = 10.0f;
	public GameObject canvas;

	private ScreenFader screenfader;
	private Inventory inventory;
	private bool faded = false;


	// Use this for initialization
	void Start () {
		screenfader = FindObjectOfType<ScreenFader> ();
		inventory = GameObject.FindObjectOfType<Inventory>();
	}


	public IEnumerator FadeOut()
	//private void FadeOut()
	{
		
		float elapsedTime = 0.0f;
		while (elapsedTime < 0.01f)
		{
			yield return new WaitForEndOfFrame();
			elapsedTime += Time.deltaTime;
		}
		screenfader.fadeIn = false;
		faded = true;
	}

	public IEnumerator CallEndGameGUI()
	{
		yield return new WaitForSeconds (waitTime);
		FindObjectOfType<LevelManager> ().LoadLevel ("EndGame");
	}

	// Update is called once per frame
	void Update () {
	}

	public void EndTheGame (){
		// Fade to black at the end of the game
		//FadeOut();
		GameObject reticle = GameObject.Find("GvrReticle");
		reticle.SetActive (false);
		StartCoroutine (FadeOut ());
		// Play endgame clip
		MusicManager musicManager = FindObjectOfType<MusicManager>();
		AudioSource audio = musicManager.GetComponent<AudioSource>();
		audio.Play ();
		// Wait for waitTime seconds, then call endgame scene
		StartCoroutine (CallEndGameGUI());
	}
}
