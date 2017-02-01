using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Collider))]
public class Interact: MonoBehaviour, IGvrGazeResponder {
	public GameObject player;
	//public GameObject finalObjectToInstantiate;
	public int numberOfObjectsToBeAdded = 1;

	private Vector3 startingPosition;
	private bool objectPulledOnce = false;
	private bool objectComplete = false;
	private Inventory inventory;
	private ValidateObject validateObject;
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
		validateObject = GameObject.FindObjectOfType<ValidateObject> ();
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
		Debug.Log("Inventory.Count: " + inventory.inventory.Count);
		for (int i = 0; i < inventory.inventory.Count; i++){
			// Check if object is a valid object to interact with
			if (inventory.inventory [i]) {
				if (validateObject.IsObjectValidForInteraction (this.gameObject, inventory.inventory [i])) {
					// Increment counter
					objectsFromInventoryCount++;
				}
			}
		}
		Debug.Log ("# items from inventory: " + objectsFromInventoryCount);
		// If player has all the elements, instantiate final object and destroy current one
		if(objectsFromInventoryCount >= numberOfObjectsToBeAdded){
			item = validateObject.InstantiateFinalObject(this.gameObject) as GameObject;
			Debug.Log("Item: " + item);
			item.transform.position = transform.position;
			item.SetActive (true);
			Destroy (gameObject);
			inventory.reticleOnObject = false;
			/*for (int i = 0; i < inventory.inventory.Count; i++) {
				// Check if object is a valid object to interact with
				if (inventory.inventory [i]) {
					if (validateObject.IsObjectValidForInteraction (this.gameObject, inventory.inventory [i])) {
						inventory.inventory.Remove (inventory.inventory [i]);
						// Removing items for inventory afterwards creates issues when trying to access gameobjects later
					}
				}
			}
			*/
			//ObjectInteract (item);
		}
		objectsFromInventoryCount = 0;
		/*else {
			objectsFromInventoryCount = 0;
		}
		//Debug.Log ("item: " + item);
		//Position object to the right & make it active
		//item.transform.position = Vector3.Lerp (item.transform.position, player.transform.position, fracJourney) + new Vector3 (0.25f, 0.75f, 0) * i;

		// Disable object interaction
		EventTrigger objectTrigger = item.GetComponent<EventTrigger> ();
		objectTrigger.enabled = false;
		objectsFromInventoryCount++;

			
		// If all components have been added on object, make it inactive
		if (objectsFromInventoryCount == numberOfObjectsToBeAdded) {
			EventTrigger trigger = GetComponent<EventTrigger> ();
			trigger.enabled = false;
			objectComplete = true;
		}
		*/

	}

	void ObjectInteract (GameObject obj)
	{
		//Move object from inventory towards current object (will need to put that in an update loop with a timer so the movement is progressive)
		//item.transform.position = Vector3.Lerp (item.transform.position, transform.position, 0.4f);
		// Play animation
		Animation anim = obj.GetComponent<Animation>();
		anim.Play("AntennaFixing"); // Need to generalise an play whatever animation is on the object
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
			//inventory.reticleOnObject = false;
			//ObjectInteract ();
			//gameObject.SetActive (false);
		//}
	}

	#endregion
}
