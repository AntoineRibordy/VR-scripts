using UnityEngine;
using System.Collections;	
using System.Collections.Generic;

[RequireComponent(typeof(Collider))]
public class Consume : MonoBehaviour, IGvrGazeResponder {
	[Tooltip("The name of the trigger parameter")]
	public string AnimationName; //name of the trigger parameter
	[Tooltip("The Animator Component we created")]
	public Animator stateMachine; //animator state machine
	public float fracJourney = 0.6f;
	public float offsetObjectFront = 1.0f;
	public float offsetObjectRight = 0.0f;

	private GvrHead player;
	private Vector3 startingPosition;
	private bool objectPulledOnce = false;
	private Inventory inventory;
	private Glow glow;

	void Start() {
		startingPosition = transform.localPosition;
		SetGazedAt(false);
		inventory = GameObject.FindObjectOfType<Inventory>();
		player = FindObjectOfType<GvrHead>();
		glow = GetComponent<Glow> ();
		if (glow == null) {
			glow = GetComponentInChildren<Glow> ();
		}
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
		//Move object from its current position towards player (move fracJourney of the distance)
		transform.position = Vector3.Lerp(transform.position, player.transform.position, fracJourney);
	}

	public void TriggerAnimation()
	{
		GetComponent<Animator>().enabled = true;
		stateMachine.SetTrigger(AnimationName); //sets trigger
		StartCoroutine (DestroyAfterAnimation (gameObject));
	}
		
	public void PositionItem()
	{
		var empty = new GameObject();
		PositionEmpty (empty);
		//Rotate empty gameobject by angle between delta and Vector3.right +  degrees
		transform.position = empty.transform.position;
		transform.rotation = empty.transform.rotation;
		transform.parent = empty.transform;
	}

	private void PositionEmpty(GameObject empty){
		//Position object player is using to the right of the player & make it active (use empty gameobject to play animation in the right place)
		//player = GameObject.Find("Player");
		empty.transform.position = player.transform.position;
		empty.transform.parent = player.transform;
		Vector3 delta = transform.position - player.transform.position;
		delta = Vector3.Normalize (delta);
		empty.transform.position += offsetObjectFront * delta;
		Vector3 offset = Quaternion.AngleAxis(90.0f, Vector3.up) * delta * offsetObjectRight;
		empty.transform.position += offset;
		empty.transform.position += new Vector3 (0, 0.3f, 0);
		// Capture angle between delta and Vector3.back
		float sign = (Vector3.right.z < delta.z)? 1.0f : -1.0f;
		float angle = Vector3.Angle(delta, Vector3.right) * sign;
		//Rotate empty gameobject by angle between delta and Vector3.right
		empty.transform.Rotate(0,angle,0);
	}

	IEnumerator DestroyAfterAnimation(GameObject obj)
	{
		// Play food/drink consumption audio clip
		AudioSource audioSource = GetComponent<AudioSource>();
		audioSource.Play ();
		yield return new WaitForEndOfFrame();
		Destroy (obj, obj.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
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
		glow.pickedUp = false;
	}

	/// Called when the viewer's trigger is used, between OnGazeEnter and OnGazeExit.
	public void OnGazeTrigger() {
		if (!objectPulledOnce) {
			PullTowardsPlayer ();
			objectPulledOnce = true;
			glow.pickedUp = true;
		} else {
			PositionItem ();
			TriggerAnimation ();
		}
	}

	#endregion
}
	
