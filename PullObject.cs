using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Collider))]
public class PullObject : MonoBehaviour, IGvrGazeResponder {
	[Tooltip("The name of the trigger parameter")]
	public string AnimationName; //name of the trigger parameter
	[Tooltip("The Animator Component we created")]
	public Animator stateMachine; //animator state machine
	public Color color = new Color(0.2f, 0.3f, 0.5f, 1.0f);
	public float fracJourney = 0.6f;

	private Vector3 startingPosition;
	private bool objectPulledOnce = false;
	private Inventory inventory;
	private Color objectColor;



	void Start() {
		startingPosition = transform.localPosition;
		SetGazedAt(false);
		inventory = GameObject.FindObjectOfType<Inventory>();
		objectColor = GetComponent<Renderer> ().material.color;
	}

	void LateUpdate() {
		GvrViewer.Instance.UpdateState();
	}

	public void SetGazedAt(bool gazedAt) {
		// Alternative color should be original color, not red.
		GetComponent<Renderer>().material.color = gazedAt ? color : objectColor;
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
		//Move object from its current position towards player (move fracJourney of the distance)
		GvrHead player = FindObjectOfType<GvrHead>();
		transform.position = Vector3.Lerp(transform.position, player.transform.position, fracJourney);
		//Vector3 diff = player.transform.position - transform.position;
		//Ray ray = new Ray (transform.position, diff);
		//transform.position = ray.GetPoint(diff.magnitude * fracJourney);
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
