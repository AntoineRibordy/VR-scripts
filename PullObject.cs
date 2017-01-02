using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Collider))]
public class PullObject : MonoBehaviour, IGvrGazeResponder {
	[Tooltip("The name of the trigger parameter")]
	public string AnimationName; //name of the trigger parameter
	[Tooltip("The Animator Component we created")]
	public Animator stateMachine; //animator state machine

	private Vector3 startingPosition;
	private bool objectPulledOnce = false;
	private Inventory inventory;



	void Start() {
		startingPosition = transform.localPosition;
		SetGazedAt(false);
		inventory = GameObject.FindObjectOfType<Inventory>();
	}

	void LateUpdate() {
		GvrViewer.Instance.UpdateState();
	}

	public void SetGazedAt(bool gazedAt) {
		GetComponent<Renderer>().material.color = gazedAt ? Color.green : Color.red;
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
		//Set object as inactive
		gameObject.SetActive (false);
		inventory.reticleOnObject = false;
		for (int i = 0; i< inventory.inventory.Count; i++)
		{
			print ("Inventory " + i + ": " + inventory.inventory[i]);
		}
	}


	public void PullTowardsPlayer() {
		//Get player position (GvrViewer)
		//Add 0.95 * player position to transform
		GvrViewer player = FindObjectOfType<GvrViewer>();
		float fracJourney = 0.7f;
		transform.position = Vector3.Lerp(transform.position, player.transform.position, fracJourney) + new Vector3(0,0.75f,0);
	}

	public void TriggerAnimation()
	{
		stateMachine.SetTrigger(AnimationName); //sets trigger
	}

	#region IGvrGazeResponder implementation

	/// Called when the user is looking on a GameObject with this script,
	/// as long as it is set to an appropriate layer (see GvrGaze).
	public void OnGazeEnter() {
		SetGazedAt(true);
		inventory.reticleOnObject = true;
	}

	/// Called when the user stops looking on the GameObject, after OnGazeEnter
	/// was already called.
	public void OnGazeExit() {
		SetGazedAt(false);
		Reset ();
		objectPulledOnce = false;
		inventory.reticleOnObject = false;
	}

	/// Called when the viewer's trigger is used, between OnGazeEnter and OnGazeExit.
	public void OnGazeTrigger() {
		if (!objectPulledOnce) {
			PullTowardsPlayer ();
			objectPulledOnce = true;
		} else {
			PutInInventory ();
			//gameObject.SetActive (false);
		}
	}

	#endregion
}
