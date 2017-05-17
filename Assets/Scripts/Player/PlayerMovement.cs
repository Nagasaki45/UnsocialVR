using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerMovement : NetworkBehaviour {

	public float speed;
	public float angularSpeed;


	// Update is called once per frame
	void Update ()
	{
		if (!isLocalPlayer)
			return;

		float x = Input.GetAxis ("Horizontal") * angularSpeed * Time.deltaTime;
		float z = Input.GetAxis ("Vertical") * speed * Time.deltaTime;

		transform.Rotate (0, x, 0);
		transform.Translate (0, 0, z);
	}
}
