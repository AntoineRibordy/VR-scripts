﻿using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MusicSource : MonoBehaviour
{
	static MusicSource instance = null;

	// Use this for initialization
	void Awake()
	{
		if (instance != null) {
			Destroy (gameObject);
		} else {
			instance = this;
			GameObject.DontDestroyOnLoad(gameObject);
		}
			
	}
		

	void OnLevelWasLoaded(){
		if (SceneManager.GetActiveScene ().name == "VR world") {
			AudioSource audio = GetComponent<AudioSource>();
			audio.Stop ();
		}
	}

		
}
