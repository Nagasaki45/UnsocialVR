using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutopilotMarkerAnimation : MonoBehaviour {

	public float speed;
	public float randomness;


	void Update () {
		float randX = speed * Time.deltaTime + randomness * Random.value;
		float randY = speed * Time.deltaTime + randomness * Random.value;
		float randZ = speed * Time.deltaTime + randomness * Random.value;
		transform.Rotate (new Vector3 (randX, randY, randZ));
	}
}
