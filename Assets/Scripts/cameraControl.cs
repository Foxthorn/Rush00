using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraControl : MonoBehaviour {

	public GameObject player;

	Vector3 offset;

	void Start () {
		offset = transform.position - player.transform.position;
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if (player != null)
			transform.position = player.transform.position + offset;
	}
}
