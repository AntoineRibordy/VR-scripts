using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WakingUp : MonoBehaviour {
	public GameObject player;
	public float offsetObjectFront = 8.0f;
	public GameObject startCanvas;

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
		StartCoroutine (DisplayStartGUI (startCanvas));
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

	// Display GUI to warn player that they're thirsty or hungry
	private IEnumerator DisplayStartGUI (GameObject canvas){
		PositionCanvas (canvas);
		yield return new WaitForSeconds (10.0f);
		canvas.SetActive (true);
		yield return new WaitForSeconds (3.0f);
		canvas.GetComponentInChildren<Text> ().text = "Vague memories come back to you...";
		yield return new WaitForSeconds (3.0f);
		canvas.GetComponentInChildren<Text> ().text = "You lost control and went down...";
		yield return new WaitForSeconds (3.0f);
		canvas.GetComponentInChildren<Text> ().text = "You need to get back home";
		yield return new WaitForSeconds (3.0f);
		canvas.GetComponentInChildren<Text> ().text = "Maybe there's something lying around here that can help you get out?";
		yield return new WaitForSeconds (3.0f);
		canvas.SetActive (false);
	}

	private void PositionCanvas(GameObject canvas){
		Vector3 offset = offsetObjectFront * Vector3.forward;
		canvas.transform.position = player.transform.position + new Vector3 (0, 1.0f, 0) + player.transform.rotation * offset;
		canvas.transform.rotation = player.transform.rotation;
		canvas.transform.parent = player.transform;
	}


}
