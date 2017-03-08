using UnityEngine;
using System.Collections;

public class Trigger : MonoBehaviour {

	public void OnGazeTrigger() {
		RaycastMovement raycastMovement = FindObjectOfType<RaycastMovement> ();
		raycastMovement.move = true;
	}
}
