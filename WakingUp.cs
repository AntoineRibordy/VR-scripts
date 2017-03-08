using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WakingUp : MonoBehaviour {
	private ScreenFader screenfader;

	private bool faded = true;
	// Use this for initialization

	/*void Awake()
	{
		screenfader = FindObjectOfType<ScreenFader> ();
		screenfader.fadeIn = true;
	}
	*/

	void Start () {
		screenfader = FindObjectOfType<ScreenFader> ();
	}

	public IEnumerator WakeUp()
	{
		if (faded)
		{
			float elapsedTime = 0.0f;
			while (elapsedTime < 0.01f)
			{
				yield return new WaitForEndOfFrame();
				elapsedTime += Time.deltaTime;
			}
			screenfader.fadeIn = true;
		}
		faded = false;
	}

	void Update (){
		if (!screenfader.fadeIn) {
			StartCoroutine (WakeUp ());
		}
	}



}
