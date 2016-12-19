using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour {
	public List<GameObject> inventory = new List<GameObject>();
	public bool reticleOnObject { 
		get;
		set;	}

}
