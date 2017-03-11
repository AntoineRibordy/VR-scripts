using UnityEngine;
using System.Collections;

public class EndGame : MonoBehaviour {
	public float waitTime = 10.0f;
	public GameObject canvas;

	private ScreenFader screenfader;
	private ResourceMining resourceMining;


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
		GameObject reticle = GameObject.Find("GvrReticle");
		reticle.SetActive (false);
		resourceMining.stopInteracting = true;
		StartCoroutine (FadeOut ());
		// Play endgame clip
		MusicPicker musicPicker = FindObjectOfType<MusicPicker> ();
		AudioClip endMusic = musicPicker.endMusic;
		musicPicker.PlayClip (endMusic);
		// Wait for waitTime seconds, then call endgame scene
		StartCoroutine (CallEndGameGUI());
	}

	void OnLevelWasLoaded(){
		resourceMining.stopInteracting = false;
	}
}
