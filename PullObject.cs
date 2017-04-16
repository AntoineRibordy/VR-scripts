using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Collider))]
public class PullObject : MonoBehaviour, IGvrGazeResponder {

	[Tooltip("The name of the trigger parameter")]
	public string AnimationName; //name of the trigger parameter
	[Tooltip("The Animator Component we created")]
	public Animator stateMachine; //animator state machine
	public float fracJourney = 0.6f;
	private DisplayCanvas displayCanvas;
	public float offsetObjectFront = 8.0f;

	private GvrHead player;
	private Vector3 startingPosition;
	private bool objectPulledOnce = false;
	private Inventory inventory;
	private Glow glow;

	void Start() {
		startingPosition = transform.localPosition;
		inventory = GameObject.FindObjectOfType<Inventory>();
		player = FindObjectOfType<GvrHead>();
		glow = GetComponent<Glow> ();
		displayCanvas = FindObjectOfType<DisplayCanvas> ();
		if (glow == null) {
			glow = GetComponentInChildren<Glow> ();
		}
	}

	void LateUpdate() {
		GvrViewer.Instance.UpdateState();
	}

	public void Reset() {
		transform.localPosition = startingPosition;
	}

	public void ToggleVRMode() {
		GvrViewer.Instance.VRModeEnabled = !GvrViewer.Instance.VRModeEnabled;
	}

	public void ToggleDistortionCorrection() {
		switch(GvrViewer.Instance.DistortionCorrection) {
		case GvrViewer.DistortionCorrectionMethod.Unity:
			GvrViewer.Instance.DistortionCorrection = GvrViewer.DistortionCorrectionMethod.Native;
			break;
		case GvrViewer.DistortionCorrectionMethod.Native:
			GvrViewer.Instance.DistortionCorrection = GvrViewer.DistortionCorrectionMethod.None;
			break;
		case GvrViewer.DistortionCorrectionMethod.None:
		default:
			GvrViewer.Instance.DistortionCorrection = GvrViewer.DistortionCorrectionMethod.Unity;
			break;
		}
	}

	public void ToggleDirectRender() {
		GvrViewer.Controller.directRender = !GvrViewer.Controller.directRender;
	}

	void PutInInventory ()
	{
		//Put object in inventory
		inventory.inventory.Add (gameObject);
		// Display information GUI the first time the player picks up an object
		if (!DisplayCanvas.pickedObjectGUIWasDisplayed) {
			displayCanvas.HandlePickedObjectGUI();
		}
		//Set object as inactive
		gameObject.SetActive (false);
		inventory.reticleOnObject = false;
		for (int i = 0; i< inventory.inventory.Count; i++)
		{
			print ("Inventory " + i + ": " + inventory.inventory[i]);
		}
	}


	public void PullTowardsPlayer() {
		//Get player position (GvrHead)
		//Move object from its current position towards player (move fracJourney of the distance)
		transform.position = Vector3.Lerp(transform.position, player.transform.position, fracJourney);
	}

	public void TriggerAnimation()
	{
		stateMachine.SetTrigger(AnimationName); //sets trigger
	}

	#region IGvrGazeResponder implementation

	/// Called when the user is looking on a GameObject with this script,
	/// as long as it is set to an appropriate layer (see GvrGaze).
	public void OnGazeEnter() {
		inventory.reticleOnObject = true;
	}

	/// Called when the user stops looking on the GameObject, after OnGazeEnter
	/// was already called.
	public void OnGazeExit() {
		Reset ();
		objectPulledOnce = false;
		inventory.reticleOnObject = false;
		glow.pickedUp = false;
	}

	/// Called when the viewer's trigger is used, between OnGazeEnter and OnGazeExit.
	public void OnGazeTrigger() {
		if (!objectPulledOnce) {
			PullTowardsPlayer ();
			objectPulledOnce = true;
			glow.pickedUp = true;
		} else {
			PutInInventory ();
		}
	}

	#endregion
}
