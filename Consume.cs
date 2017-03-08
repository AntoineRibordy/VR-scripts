using UnityEngine;
using System.Collections;	
using System.Collections.Generic;

[RequireComponent(typeof(Collider))]
public class Consume : MonoBehaviour, IGvrGazeResponder {
	[Tooltip("The name of the trigger parameter")]
	public string AnimationName; //name of the trigger parameter
	[Tooltip("The Animator Component we created")]
	public Animator stateMachine; //animator state machine
	public float offsetObject = 1.0f;

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
		
	public void PullTowardsPlayer() {
		//Get player position (GvrViewer)
		//Add 0.95 * player position to transform
		GvrViewer player = FindObjectOfType<GvrViewer>();
		float fracJourney = 0.7f;
		transform.position = Vector3.Lerp(transform.position, player.transform.position, fracJourney) + new Vector3(0,0.75f,0);
	}

	public void TriggerAnimation()
	{
		GetComponent<Animator>().enabled = true;
		stateMachine.SetTrigger(AnimationName); //sets trigger
		StartCoroutine (DestroyAfterAnimation (gameObject));
	}
		
	public void PositionItem()
	{
		GvrViewer player = FindObjectOfType<GvrViewer>();
		var empty = new GameObject();
		//Position object player is using to the right of the player & make it active (use empty gameobject to play animation in the right place)
		empty.transform.position = player.transform.position;
		empty.transform.parent = player.transform;
		Vector3 delta = transform.position - player.transform.position;
		delta = Vector3.Normalize (delta);
		empty.transform.position += offsetObject * delta;
		empty.transform.position += new Vector3 (0, 0.7f, 0);
		// Capture angle between delta and Vector3.right
		float sign = (Vector3.left.z < delta.z)? 1.0f : -1.0f;
		float angle = Vector3.Angle(delta, Vector3.left) * sign;
		Debug.Log ("Angle: " + angle);
		//angle += 30;
		//Rotate empty gameobject by angle between delta and Vector3.right +  degrees
		empty.transform.Rotate(0,angle,0);
		transform.position = empty.transform.position;
		transform.rotation = empty.transform.rotation;
		transform.parent = empty.transform;
	}

	IEnumerator DestroyAfterAnimation(GameObject obj)
	{
		yield return new WaitForEndOfFrame();
		//Destroy (obj, obj.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
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
			PositionItem ();
			TriggerAnimation ();
		}
	}

	#endregion
}
	
