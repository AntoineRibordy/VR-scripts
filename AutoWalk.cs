// This script moves your player automatically in the direction he is looking at. You can 
// activate the autowalk function by pull the cardboard trigger, by define a threshold angle 
// or combine both by selecting both of these options.
// The threshold is an value in degree between 0° and 90°. So for example the threshold is 
// 30°, the player will move when he is looking 31° down to the bottom and he will not move 
// when the player is looking 29° down to the bottom. This script can easally be configured
// in the Unity Inspector.Attach this Script to your CardboardMain-GameObject. If you 
// haven't the Cardboard Unity SDK, download it from https://developers.google.com/cardboard/unity/download

using UnityEngine;
using System.Collections;

public class AutoWalk : MonoBehaviour 
{
	private const int RIGHT_ANGLE = 90; 

	// This variable determinates if the player will move or not 
	private bool isWalking = false;
	private Inventory inventory;
	private Palm palm;

	GvrHead head = null;
	GvrViewer gvrViewer = null;

	//This is the variable for the player speed
	[Tooltip("With this speed the player will move.")]
	public float speed;

	[Tooltip("Activate this checkbox if the player shall move when the Cardboard trigger is pulled.")]
	public bool walkWhenTriggered;

	[Tooltip("Activate this checkbox if the player shall move when he looks below the threshold.")]
	public bool walkWhenLookDown;

	[Tooltip("This has to be an angle from 0° to 90°")]
	public double thresholdAngle;

	[Tooltip("Activate this Checkbox if you want to freeze the y-coordiante for the player. " +
		"For example in the case of you have no collider attached to your CardboardMain-GameObject" +
		"and you want to stay in a fixed level.")]
	public bool freezeYPosition; 

	[Tooltip("This is the fixed y-coordinate.")]
	public float yOffset;

	void Start () 
	{
		head = GameObject.FindObjectOfType<GvrHead> ();
		gvrViewer = GameObject.FindObjectOfType<GvrViewer> ();
		inventory = GameObject.FindObjectOfType<Inventory>();
		palm = GameObject.FindObjectOfType<Palm> ();
		inventory.reticleOnObject = false;
	}

	void Update () 
	{
		// Walk when the Cardboard Trigger is used 
		if (walkWhenTriggered && !walkWhenLookDown && !isWalking && gvrViewer.Triggered && !inventory.reticleOnObject && !palm.stopInteracting) 
		{
			isWalking = true;
		} 
		else if (walkWhenTriggered && !walkWhenLookDown && isWalking && gvrViewer.Triggered) 
		{
			isWalking = false;
		}

		// Walk when player looks below the threshold angle 
		if (walkWhenLookDown && !walkWhenTriggered && !isWalking &&  
			head.transform.eulerAngles.x >= thresholdAngle && 
			head.transform.eulerAngles.x <= RIGHT_ANGLE && !inventory.reticleOnObject && !palm.stopInteracting) 
		{
			isWalking = true;
		} 
		else if (walkWhenLookDown && !walkWhenTriggered && isWalking && 
			(head.transform.eulerAngles.x <= thresholdAngle ||
				head.transform.eulerAngles.x >= RIGHT_ANGLE)) 
		{
			isWalking = false;
		}

		// Walk when the Cardboard trigger is used and the player looks down below the threshold angle
		if (walkWhenLookDown && walkWhenTriggered && !isWalking &&  
			head.transform.eulerAngles.x >= thresholdAngle && 
			gvrViewer.Triggered &&
			head.transform.eulerAngles.x <= RIGHT_ANGLE) 
		{
			isWalking = true;
		} 
		else if (walkWhenLookDown && walkWhenTriggered && isWalking && 
			head.transform.eulerAngles.x >= thresholdAngle &&
			(gvrViewer.Triggered || 
				head.transform.eulerAngles.x >= RIGHT_ANGLE)) 
		{
			isWalking = false;
		}

		if (isWalking) 
		{
			Vector3 direction = new Vector3(head.transform.forward.x, 0, head.transform.forward.z).normalized * speed * Time.deltaTime;
			Quaternion rotation = Quaternion.Euler(new Vector3(0, -transform.rotation.eulerAngles.y, 0));
			transform.Translate(rotation * direction);
		}

		if(freezeYPosition)
		{
			transform.position = new Vector3(transform.position.x, yOffset, transform.position.z);
		}
	}
}