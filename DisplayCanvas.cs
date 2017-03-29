using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DisplayCanvas : MonoBehaviour {

	public static bool pickedObjectGUIWasDisplayed = false;

	public float offsetObjectFront = 8.0f;
	public GameObject player;
	public GameObject thirstyCanvas;
	public GameObject hungryCanvas;
	public GameObject startCanvas;
	public GameObject pickedObjectCanvas;
	public GameObject interactCanvas;


	void OnLevelWasLoaded(){
		if (pickedObjectGUIWasDisplayed) {
			pickedObjectGUIWasDisplayed = false;
		}
	}

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private void PositionCanvas(GameObject canvas){
		Vector3 offset = offsetObjectFront * Vector3.forward;
		canvas.transform.position = player.transform.position + new Vector3 (0, 1.5f, 0) + player.transform.rotation * offset;
		canvas.transform.rotation = player.transform.rotation;
		canvas.transform.parent = player.transform;
	}

	// Display GUI to warn player that they're thirsty or hungry
	public IEnumerator DisplayStressGUI (GameObject canvas){
		PositionCanvas (canvas);
		canvas.SetActive (true);
		yield return new WaitForSeconds (5.0f);
		canvas.SetActive (false);
	}

	public void HandlePickedObjectGUI(){
		StartCoroutine (DisplayPickedObjectGUI ());
		pickedObjectGUIWasDisplayed = true;
	}

	// Display GUI to guide player what to do with an object once they've picked it up
	public IEnumerator DisplayPickedObjectGUI (){
		PositionCanvas (pickedObjectCanvas);
		pickedObjectCanvas.SetActive (true);
		pickedObjectCanvas.GetComponentInChildren<Text> ().text = "You've just picked an object";
		yield return new WaitForSeconds (2.0f);
		pickedObjectCanvas.GetComponentInChildren<Text> ().text = "You can now use that object to interact with other objects";
		yield return new WaitForSeconds (2.0f);
		pickedObjectCanvas.GetComponentInChildren<Text> ().text = "Press the trigger with the reticle on the object to interact";
		yield return new WaitForSeconds (2.0f);
		pickedObjectCanvas.GetComponentInChildren<Text> ().text = "You can only interact with an object if you have the correct object in your inventory";
		yield return new WaitForSeconds (3.0f);
		pickedObjectCanvas.SetActive (false);
	}

	// Display GUI to give instructions to player at the start of the game
	public IEnumerator DisplayStartGUI (){
		PositionCanvas (startCanvas);
		yield return new WaitForSeconds (10.0f);
		startCanvas.SetActive (true);
		startCanvas.GetComponentInChildren<Text> ().text = "What happened?";
		yield return new WaitForSeconds (2.0f);
		startCanvas.GetComponentInChildren<Text> ().text = "Vague memories come back to you...";
		yield return new WaitForSeconds (2.0f);
		startCanvas.GetComponentInChildren<Text> ().text = "You lost control and went down...";
		yield return new WaitForSeconds (2.0f);
		startCanvas.GetComponentInChildren<Text> ().text = "You need to get back home";
		yield return new WaitForSeconds (2.0f);
		startCanvas.GetComponentInChildren<Text> ().text = "Maybe there's something lying around here that can help you get out?";
		yield return new WaitForSeconds (3.0f);
		startCanvas.GetComponentInChildren<Text> ().text = "Look around and use your trigger when you see the green cylinder to move around";
		yield return new WaitForSeconds (3.0f);
		startCanvas.GetComponentInChildren<Text> ().text = "If the white reticle grow wider on an object, you can pick it up with the trigger";
		yield return new WaitForSeconds (3.0f);
		startCanvas.GetComponentInChildren<Text> ().text = "Just make sure you're close enough to the object";
		yield return new WaitForSeconds (2.0f);
		startCanvas.GetComponentInChildren<Text> ().text = "Other object can't be picked up";
		yield return new WaitForSeconds (2.0f);
		startCanvas.GetComponentInChildren<Text> ().text = "You can only interact with them, if you have the correct object in your inventory";
		yield return new WaitForSeconds (3.0f);
		startCanvas.GetComponentInChildren<Text> ().text = "And for some objects to activate, you need more than one object of the same kind (wood for example)";
		yield return new WaitForSeconds (3.0f);
		startCanvas.SetActive (false);
	}

	// Display GUI to tell player they need more of that object
	public IEnumerator DisplayInteractGUI (string objectName, int numberOfObjectsMissing){
		PositionCanvas (interactCanvas);
		interactCanvas.SetActive (true);
		interactCanvas.GetComponentInChildren<Text> ().text = "You need another " + numberOfObjectsMissing + " " + objectName + " to interact with this object" ;
		yield return new WaitForSeconds (5.0f);
		interactCanvas.SetActive (false);
	}
}
