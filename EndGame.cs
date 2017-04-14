using UnityEngine;
using System.Collections;

public class EndGame : MonoBehaviour {
	public GameObject canvas;

	private ScreenFader screenfader;
	private ResourceMining resourceMining;
	private AudioClip endMusic;


	// Use this for initialization
	void Start () {
		screenfader = FindObjectOfType<ScreenFader> ();
		resourceMining = GameObject.FindObjectOfType<ResourceMining> ();
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
	}

	public IEnumerator CallEndGameGUI(string levelToLoad, float waitTime)
	{
		yield return new WaitForSeconds (waitTime);
		FindObjectOfType<LevelManager> ().LoadLevel (levelToLoad);
	}

	// Update is called once per frame
	void Update () {
	}

	public void EndTheGame (string endLevelToLoad, float waitTime){
		// Fade to black at the end of the game
		GameObject reticle = GameObject.Find("GvrReticlePointer");
		reticle.SetActive (false);
		resourceMining.stopInteracting = true;
		StartCoroutine (FadeOut ());
		// Play endgame clip
		MusicPicker musicPicker = FindObjectOfType<MusicPicker> ();
		if (endLevelToLoad == "EndGame") {
			endMusic = musicPicker.endMusic;
		} else endMusic = musicPicker.deathClip;
		musicPicker.PlayClip (endMusic);
		// Wait for waitTime seconds, then call endgame scene
		StartCoroutine (CallEndGameGUI(endLevelToLoad, waitTime));
	}

	void OnLevelWasLoaded(){
		resourceMining = GameObject.FindObjectOfType<ResourceMining> ();
		resourceMining.stopInteracting = false;
	}
}
