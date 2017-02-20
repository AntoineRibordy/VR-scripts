using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Collider))]
public class Palm: MonoBehaviour, IGvrGazeResponder {
	public GameObject palmLog;
	public float logSpeed = 3.0f;
	public float offsetObject = 1.0f;
	public int logCount;
	//public int numberOfObjectsToBeAdded = 1;

	private GameObject player;
	private Vector3 startingPosition;
	private bool objectPulledOnce = false;
	private bool objectComplete = false;
	private Inventory inventory;
	GameObject cubeFinal;
	private float fracJourney = 0.5f;
	private bool itemFromInventory = false;
	//private bool objectsToBeMerged = false;
	private GameObject item = null;
	private int objectsFromInventoryCount = 0;





	void Start() {
		startingPosition = transform.localPosition;
		SetGazedAt(false);
		inventory = GameObject.FindObjectOfType<Inventory>();
		//logCount = Random.Range (1, 3);
	}

	void Update()
	{
		if (itemFromInventory) {
			InteractFromInventory ();
			itemFromInventory = false;
		}
		/* if (objectComplete)
		{
			PlayAudio();
			GameWon = true;
		}*/


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

	void InteractFromInventory ()
	{
		//Move current object to the left
		//transform.position += new Vector3(-0.25f,0,0);
		//Pull object from inventory to interact
		for (int i = 0; i < inventory.inventory.Count; i++){
			if (inventory.inventory [i].name == "Axe") {
				item = Instantiate (inventory.inventory [i]) as GameObject;
				player = GameObject.Find("GvrViewerMain");
				var empty = new GameObject();
				//Position object player is using to the right of the player & make it active (use empty gameobject to play animation in the right place)
				empty.transform.position = player.transform.position;
				empty.transform.parent = player.transform;
				Vector3 delta = transform.position - player.transform.position;
				delta = Vector3.Normalize (delta);
				empty.transform.position += offsetObject * delta;
				Debug.Log ("delta: " + delta);
				Vector3 offset = Quaternion.AngleAxis(90.0f, Vector3.up) * delta * 0.7f;
				Debug.Log ("offset: " + offset);
				empty.transform.position += offset;
				empty.transform.position += new Vector3 (0, 0.7f, 0);
				// Capture angle between delta and Vector3.forward
				float sign = (Vector3.right.z < delta.z)? -1.0f : 1.0f;
				float angle = Vector3.Angle(delta, Vector3.right) * sign;
				//Rotate empty gameobject by angle between delta and Vector3.forward
				empty.transform.Rotate(0,angle,0);
				/*
				item.transform.position = player.transform.position;
				item.transform.parent = player.transform;
				Vector3 delta = transform.position - player.transform.position;
				delta = Vector3.Normalize (delta);
				item.transform.position += offsetObject * delta;
				Debug.Log ("delta: " + delta);
				Vector3 offset = Quaternion.AngleAxis(90.0f, Vector3.up) * delta * 0.7f;
				Debug.Log ("offset: " + offset);
				item.transform.position += offset;
				item.transform.position += new Vector3 (0, 0.7f, 0);
				*/
				item.transform.position = empty.transform.position;
				item.transform.rotation = empty.transform.rotation;
				item.transform.parent = empty.transform;
				item.SetActive (true);
				ObjectInteract (item);
				Vector3 offset2 = Random.Range(-1f, 1f) * Vector3.forward;
				GameObject log = Instantiate (palmLog, transform.position + offset2, Quaternion.Euler(0, 0, 90)) as GameObject;
				log.SetActive (true);
				log.name = "Log";
				EventTrigger objectTrigger = item.GetComponent<EventTrigger> ();
				objectTrigger.enabled = false;
				objectsFromInventoryCount++;
			}
		}
		// If all components have been added on object, make it inactive
		if (objectsFromInventoryCount == logCount) {
			EventTrigger trigger = GetComponent<EventTrigger> ();
			trigger.enabled = false;
			gameObject.SetActive (false);
			objectComplete = true;
		}

	}

	void ObjectInteract (GameObject obj)
	{
		// Play animation of object from inventory
		obj.GetComponent<Animator>().enabled = true;
		obj.GetComponent<PullObject>().TriggerAnimation();
	}

	/*void MergeObjects()
	{
		//Merge them: Set objects as inactive & instantiate new object
		item.SetActive(false);
		//gameObject.SetActive(false);
		//GameObject newitem = Instantiate (inventory.inventory [1]) as GameObject;
		GameObject newitem = Instantiate (GameObject.FindGameObjectWithTag("CubeFinal")) as GameObject;
		newitem.SetActive (true);
		newitem.transform.position = Vector3.Lerp (newitem.transform.position, player.transform.position, fracJourney) + new Vector3 (0.25f, 0.75f, 0);
	}*/



	public void PullTowardsPlayer() {
		//Get player position (GvrViewer)
		//Add 0.95 * player position to transform
		GvrViewer player = FindObjectOfType<GvrViewer>();
		float fracJourney = 0.6f;
		transform.position = Vector3.Lerp(transform.position, player.transform.position, fracJourney) + new Vector3(0,0.75f,0);
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
		/*if (!objectPulledOnce) {
			PullTowardsPlayer ();
			objectPulledOnce = true;
		} else {*/
		itemFromInventory = true;
		inventory.reticleOnObject = false;
		//ObjectInteract ();
		//gameObject.SetActive (false);
		//}
	}

	#endregion
}
