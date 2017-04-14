using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Collider))]
public class ResourceMining : MonoBehaviour {
	public float objectSpeed = 3.0f;
	public float offsetObjectFront = 1.0f;
	public float offsetObjectRight = 0.5f;
	public int maxObjectCount = 1;
	public bool stopInteracting = false;

	private GameObject player;
	private Vector3 startingPosition;
	private Inventory inventory;
	private bool itemFromInventory = false;
	private GameObject item = null;
	private ValidateObject validateObject;
	private int objectsFromInventoryCount = 0;
	private int objectCount;


	void Start() {
		startingPosition = transform.localPosition;
		inventory = GameObject.FindObjectOfType<Inventory>();
		player = GameObject.Find("Player");
		validateObject = GameObject.FindObjectOfType<ValidateObject> ();
		objectCount = Random.Range (1, maxObjectCount+1);
	}

	void Update()
	{
		if (itemFromInventory) {
			InteractFromInventory ();
			itemFromInventory = false;
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

	void InteractFromInventory ()
	{
		// Pull object from inventory to interact
		for (int i = 0; i < inventory.inventory.Count; i++){
			// Check if object is a valid object to interact with and if so, instantiate it
			if (inventory.inventory [i]) {
				if (validateObject.IsObjectValidForInteraction (this.gameObject, inventory.inventory [i])) {
					item = Instantiate (inventory.inventory [i]) as GameObject;
					var empty = new GameObject ();
					PositionEmpty (empty);
					// Place object used for mining at the right place to animate it
					item.transform.position = empty.transform.position;
					item.transform.rotation = empty.transform.rotation;
					item.transform.parent = empty.transform;
					ObjectInteract (item);
				}
			}
		}
		// If all components have been added on object, make it inactive
		if (objectsFromInventoryCount == objectCount) {
			EventTrigger trigger = GetComponent<EventTrigger> ();
			trigger.enabled = false;
		}
	}

	//Position object player is using to the right of the player & make it active (use empty gameobject to play animation in the right place)
	private void PositionEmpty(GameObject empty){
		empty.transform.position = player.transform.position;
		empty.transform.parent = player.transform;
		Vector3 delta = transform.position - player.transform.position;
		delta = Vector3.Normalize (delta);
		empty.transform.position += offsetObjectFront * delta;
		Vector3 offset = Quaternion.AngleAxis(90.0f, Vector3.up) * delta * offsetObjectRight;
		empty.transform.position += offset;
		empty.transform.position += new Vector3 (0, 0.2f, 0);
		// Capture angle between delta and Vector3.right
		float sign = (Vector3.right.z < delta.z)? -1.0f : 1.0f;
		float angle = Vector3.Angle(delta, Vector3.right) * sign;
		//Rotate empty gameobject by angle between delta and Vector3.right
		empty.transform.Rotate(0,angle,0);
	}

	private void ObjectInteract (GameObject obj)
	{
		// Play animation of object from inventory
		obj.SetActive (true);
		obj.GetComponent<Animator>().enabled = true;
		obj.GetComponent<PullObject>().TriggerAnimation();
		StartCoroutine (DestroyAfterAnimation (obj));
	}
		

	IEnumerator DestroyAfterAnimation(GameObject obj)
	{
		GameObject reticle = GameObject.Find("GvrReticlePointer");
		reticle.SetActive (false);
		stopInteracting = true;
		AudioSource audioSource = obj.GetComponent<AudioSource> ();
		audioSource.Play ();
		yield return new WaitForEndOfFrame();
		yield return new WaitForSeconds (obj.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).length);
		ProduceResource ();
		Destroy (obj);
		if (objectsFromInventoryCount == objectCount) {
			gameObject.SetActive (false);
		}
		reticle.SetActive (true);
		stopInteracting = false;
	}

	private void ProduceResource (){
		// Pb: new resource replaces old one and sometimes glitches when trying to pick it up

		Vector3 offset2 = Random.Range(-2f, 2f) * Vector3.forward;
		GameObject resource = Instantiate(validateObject.InstantiateFinalObject(this.gameObject)) as GameObject;
		//Object location at mining source
		resource.transform.position = transform.position + offset2;
		resource.SetActive (true);
		resource.name = validateObject.nameFinalObject(this.gameObject);
		EventTrigger objectTrigger = item.GetComponent<EventTrigger> ();
		objectTrigger.enabled = false;
		objectsFromInventoryCount++;
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
		inventory.reticleOnObject = false;
	}

	/// Called when the viewer's trigger is used, between OnGazeEnter and OnGazeExit.
	public void OnGazeTrigger() {
		if (!stopInteracting) {
			itemFromInventory = true;
			inventory.reticleOnObject = false;
		}
	}

	#endregion
}
