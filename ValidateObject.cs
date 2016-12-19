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
			break;
		case "Raft":
			if (sourceObject.name == "Parachute") {
				return true;
			} else
				return false;
			break;
		case "Branch":
			if (sourceObject.name == "String") {
				return true;
			} else
				return false;
			break;
		case "Tree":
			if (sourceObject.name == "Axe") {
				return true;
			} else
				return false;
			break;
		case "Rock":
			if (sourceObject.name == "Log") {
				return true;
			} else
				return false;
			break;
		case "Fire":
			if (sourceObject.name == "Moss") {
				return true;
			} else
				return false;
			break;
		case "Pole":
			if (sourceObject.name == "String") {
				return true;
			} else
				return false;
			break;
		case "Fishing Area":
			if (sourceObject.name == "Fishing Pole") {
				return true;
			} else
				return false;
			break;
		default:
			print ("Non-referrenced object");
			return false;
			break;
		}
	}

	public GameObject InstantiateFinalObject(GameObject originalObject)
	{
		switch (originalObject.name) {
		case "FixedLog":
			return objectsToInstantiate [0];
			break;
		case "Raft":
			return objectsToInstantiate [1];
			break;
		case "Branch":
			return objectsToInstantiate [2];
			break;
		case "Tree":
			return objectsToInstantiate [3];
			break;
		case "Rock":
			return objectsToInstantiate [4]; 
			break;
		case "Fire":
			return objectsToInstantiate [5]; 
			break;
		case "Pole":
			return objectsToInstantiate [6]; 
			break;
		case "Fishing Area":
			return objectsToInstantiate [7]; 
			break;
		default:
			print ("Non-referrenced object");
			return null;
			break;
		}
	}

	public string nameFinalObject (GameObject originalObject)
	{
		switch (originalObject.name) {
		case "FixedLog":
			return "Raft";
			break;
		case "Raft":
			return "Finished Raft";
			break;
		case "Branch":
			return "Pole";
			break;
		case "Tree":
			return "Log";
			break;
		case "Rock":
			return "Fire"; 
			break;
		case "Fire":
			return "Food"; 
			break;
		case "Pole":
			return "Fishing Pole"; 
			break;
		case "Fishing Area":
			return "Fish"; 
			break;
		default:
			print ("Non-referrenced object");
			return null;
			break;
		}
	}
}
