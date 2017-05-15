using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class star : MonoBehaviour {

	public static float starRightPower = 1f;
	private ParticleSystem ps;
	private Color defaultColor = new Color(0f, 0f, 0f, 0f);
	private Vector3 defaultLocalScale = new Vector3(0f, 0f, 0f);
	void Start(){
		ps = GetComponent<ParticleSystem> ();
		defaultColor = ps.startColor;
		defaultLocalScale = transform.localScale;
	}
	void Update(){
		ps.startColor = defaultColor * starRightPower;
		transform.localScale = defaultLocalScale * starRightPower;
	}
}