using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class starRightPowerControl : MonoBehaviour {

	private Slider slider;

	// Use this for initialization
	void Start () {
		slider = GetComponent<Slider> ();
		slider.value = star.starRightPower;
		slider.maxValue = 2f;
		slider.minValue = 0f;
		slider.value = 1.25f;
	}
	
	// Update is called once per frame
	void Update () {
		star.starRightPower = slider.value;
	}
	void OnDrag(){
		
	}
}
