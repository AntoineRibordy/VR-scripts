using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DisplayEndCanvas : MonoBehaviour {

	public float offsetObjectFront = 8.0f;
	public GameObject player;

	// Use this for initialization
	void Start () {
		PositionCanvas (gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private void PositionCanvas(GameObject canvas){
		Vector3 offset = offsetObjectFront * Vector3.forward;
		canvas.transform.position = player.transform.position + new Vector3 (0, 1.5f, 0) + player.transform.rotation * offset;
		canvas.transform.rotation = player.transform.rotation;
		canvas.transform.parent = player.transform;
	}
}
