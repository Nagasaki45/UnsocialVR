using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerSimulatorControl : MonoBehaviour {

	public float angularSpeed;


	private void Update ()
	{
    float x = Input.GetAxis ("Horizontal") * angularSpeed * Time.deltaTime;

    transform.Rotate (0, x, 0);
	}
}
