using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Collider))]
public class Raft: MonoBehaviour, IGvrGazeResponder {
	public GameObject player;
	public int numberOfObjectsToBeAdded = 1;
	public Color color = new Color(0.2f, 0.3f, 0.5f, 1.0f);
	public GameObject canvas;

	private Vector3 startingPosition;
	private Inventory inventory;
	private ValidateObject validateObject;
	GameObject cubeFinal;
	private Color objectColor;

	void Start() {
		startingPosition = transform.localPosition;
		SetGazedAt(false);
		objectColor = GetComponent<Renderer> ().material.color;
		inventory = GameObject.FindObjectOfType<Inventory>();
	}
		

		void LateUpdate() {
			GvrViewer.Instance.UpdateState();
		}

		public void SetGazedAt(bool gazedAt) {
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
			inventory.reticleOnObject = false;
		}

		/// Called when the viewer's trigger is used, between OnGazeEnter and OnGazeExit.
	public void OnGazeTrigger() {
		// Call EndTheGame script
		EndGame endgame = FindObjectOfType<EndGame>();
		endgame.EndTheGame ();

	}

		#endregion
}
