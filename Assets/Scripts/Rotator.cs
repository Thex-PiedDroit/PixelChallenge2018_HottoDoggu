// Floater v0.0.2
// by Donovan Keith
//
// [MIT License](https://opensource.org/licenses/MIT)

// Makes objects float up & down while gently spinning.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour {

	// User Inputs
	//public float degreesPerSecond = 15.0f;
	public float amplitudeX = 0.5f;
	public float amplitudeY = 0.5f;
	public float frequencyX = 1f;
	public float frequencyY = 1f;

	// Position Storage Variables
	Vector3 posOffset = new Vector3 ();
	Vector3 tempPos = new Vector3 ();

	// Use this for initialization
	void Start () {
		// Store the starting position & rotation of the object
		posOffset = transform.position;
	}

	// Update is called once per frame
	void Update () {
		// Spin object around Y-Axis
		//transform.Rotate(new Vector3(0f, Time.deltaTime * degreesPerSecond, 0f), Space.World);
	
		// Float up/down with a Sin()
		tempPos = posOffset;
		tempPos.x += Mathf.Sin (Time.fixedTime * Mathf.PI * frequencyX) * amplitudeX;
		tempPos.y += Mathf.Sin (Time.fixedTime * Mathf.PI * frequencyY) * amplitudeY;

		transform.position = tempPos;
}
}