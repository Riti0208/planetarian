using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class debugCamera : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.D)) {
			transform.Rotate (transform.up * Time.deltaTime * 50f);
		}
		if (Input.GetKey (KeyCode.A)) {
			transform.Rotate (-transform.up * Time.deltaTime * 50f);
		}
	}
}
