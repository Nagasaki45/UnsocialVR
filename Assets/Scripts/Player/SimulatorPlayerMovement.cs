using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulatorPlayerMovement : MonoBehaviour {

	public float speed;
	public float angularSpeed;
	public float defaultHeight;


	private void Update () {
		float x = Input.GetAxis ("Horizontal") * angularSpeed * Time.deltaTime;
		float z = Input.GetAxis ("Vertical") * speed * Time.deltaTime;

		transform.Rotate (0, x, 0);
		transform.Translate (0, 0, z);

		transform.position = new Vector3 (transform.position.x, defaultHeight, transform.position.z);
	}
}
