using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerSimulatorControl : MonoBehaviour {

	public float angularSpeed;
	public float speed;


	private void Update ()
	{
    float x = Input.GetAxis ("Horizontal") * angularSpeed * Time.deltaTime;
    float z = Input.GetAxis ("Vertical") * speed * Time.deltaTime;

    transform.Rotate (0, x, 0);
    transform.Translate (0, 0, z);
	}
}
