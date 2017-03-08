using UnityEngine;
using System.Collections;

public class ValidateObject : MonoBehaviour {

	public GameObject[] objectsToInstantiate;

	public bool IsObjectValidForInteraction (GameObject targetObject, GameObject sourceObject){
		switch (targetObject.name) {
		case "FixedLog":
			if (sourceObject.name == "Log") {
				return true;
			} else {
				return false;
			}
		case "Raft":
			if (sourceObject.name == "Parachute") {
				return true;
			} else
				return false;
		case "Branch":
			if (sourceObject.name == "String") {
				return true;
			} else
				return false;
		case "Tree":
			if (sourceObject.name == "Axe") {
				return true;
			} else
				return false;
		case "Rock":
			if (sourceObject.name == "Log") {
				return true;
			} else
				return false;
		case "Fire":
			if (sourceObject.name == "Moss") {
				return true;
			} else
				return false;
		case "Fishing Area":
			if (sourceObject.name == "Fishing Pole") {
				return true;
			} else
				return false;
		case "LiveFire":
			if (sourceObject.name == "Fish") {
				return true;
			} else
				return false;
		default:
			print ("Non-referrenced object");
			return false;
		}
	}

	public GameObject InstantiateFinalObject(GameObject originalObject)
	{
		switch (originalObject.name) {
		case "FixedLog":
			return objectsToInstantiate [0];
		case "Raft":
			return objectsToInstantiate [1];
		case "Branch":
			return objectsToInstantiate [2];
		case "Tree":
			return objectsToInstantiate [3];
		case "Rock":
			return objectsToInstantiate [4]; 
		case "Fire":
			return objectsToInstantiate [5]; 
		case "Fishing Area":
			return objectsToInstantiate [6]; 
		case "LiveFire":
			return objectsToInstantiate [7]; 
		default:
			print ("Non-referrenced object");
			return null;
		}
	}

	public string nameFinalObject (GameObject originalObject)
	{
		switch (originalObject.name) {
		case "FixedLog":
			return "Raft";
		case "Raft":
			return "Raft_final";
		case "Branch":
			return "Pole";
		case "Tree":
			return "Log";
		case "Rock":
			return "Fire"; 
		case "Fire":
			return "Food"; 
		case "Pole":
			return "Fishing Pole"; 
		case "Fishing Area":
			return "Fish"; 
		case "LiveFire":
			return "Fish";
		default:
			print ("Non-referrenced object");
			return null;
		}
	}
}
