using UnityEngine;
using System.Collections;

public class MusicPicker : MonoBehaviour {
	public AudioClip[] soundFiles;
	public AudioClip deathClip;
	public AudioClip successClip;
	public AudioClip stressClip;
	public AudioClip unwellClip;
	public AudioClip endMusic;
	private AudioSource soundSource;

	// Use this for initialization
	void Start () {
		soundSource = FindObjectOfType<AudioSource> ();
	}

	// Update is called once per frame
	void Update () {
		if (!soundSource.isPlaying) {
			PlayNextSong();
		}
	}

	private void PlayNextSong (){
		int index = Random.Range (0, 6);
		soundSource.clip = soundFiles [index];
		soundSource.Play ();
	}

	public void PlayClip (AudioClip name){
		soundSource.clip = name;
		soundSource.Play ();
	}

	public void StopClip (){
		soundSource.Stop ();
	}
}
