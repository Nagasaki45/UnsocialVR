using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TokenRotator : MonoBehaviour {

	public Vector3 rotation;


	private void Update ()
	{
		transform.Rotate (Time.deltaTime * rotation);
	}
}
