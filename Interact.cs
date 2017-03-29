using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Collider))]
public class Interact: MonoBehaviour, IGvrGazeResponder {
	public GameObject player;
	public int numberOfObjectsToBeAdded = 1;
	public Color color = new Color(0.2f, 0.3f, 0.5f, 1.0f);

	private DisplayCanvas displayCanvas;
	private Vector3 startingPosition;
	private Inventory inventory;
	private ValidateObject validateObject;
	GameObject cubeFinal;
	private bool itemFromInventory = false;
	//private bool objectsToBeMerged = false;
	private GameObject item = null;
	private int objectsFromInventoryCount = 0;
	private Color objectColor;
	private string objectName;
	private int numberOfObjectsMissing;




	void Start() {
		startingPosition = transform.localPosition;
		inventory = GameObject.FindObjectOfType<Inventory>();
		validateObject = GameObject.FindObjectOfType<ValidateObject> ();
		objectColor = GetComponent<Renderer> ().material.color;
		displayCanvas = FindObjectOfType<DisplayCanvas> ();
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
		//Pull object from inventory to interact
		for (int i = 0; i < inventory.inventory.Count; i++){
			// Check if object is a valid object to interact with
			if (inventory.inventory [i]) {
				if (validateObject.IsObjectValidForInteraction (this.gameObject, inventory.inventory [i])) {
					// Increment counter
					objectsFromInventoryCount++;
					objectName = inventory.inventory [i].name;
				}
			}
		}
		// If player has all the elements, instantiate final object and destroy current one
		if (objectsFromInventoryCount >= numberOfObjectsToBeAdded) {
			item = validateObject.InstantiateFinalObject (this.gameObject) as GameObject;
			item.transform.position = transform.position;
			StartCoroutine (PlaySuccessAndDestroy ());
		} else if (objectName) {
			numberOfObjectsMissing = numberOfObjectsToBeAdded - objectsFromInventoryCount;
			StartCoroutine(displayCanvas.DisplayInteractGUI (objectName, numberOfObjectsMissing));
		}
		objectsFromInventoryCount = 0;

	}

	IEnumerator PlaySuccessAndDestroy()
	{
		MusicPicker musicPicker = FindObjectOfType<MusicPicker> ();
		AudioClip successClip = musicPicker.successClip;
		musicPicker.PlayClip (successClip);
		inventory.reticleOnObject = false;
		yield return new WaitForSeconds (successClip.length-0.1f);
		musicPicker.StopClip ();
		item.SetActive (true);
		Destroy (gameObject);
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
			itemFromInventory = true;
	}

	#endregion
}
