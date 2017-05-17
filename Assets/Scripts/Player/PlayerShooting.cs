using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerShooting : NetworkBehaviour {

	public GameObject bulletPrefab;
	public Transform firingTransform;
	public float shootingStrength;


	// Update is called once per frame
	void Update () {
		if (isLocalPlayer && Input.GetButtonDown ("Fire"))
		{
			CmdFire ();
		}
	}


	// This [Command] code is called from the client but runs on the server.
	[Command]
	void CmdFire()
	{
		GameObject bullet = Instantiate (bulletPrefab, firingTransform);
		bullet.transform.parent = null;  // Disconnect from the shooter
		Rigidbody bulletRB = bullet.GetComponent<Rigidbody> ();
		bulletRB.velocity = bullet.transform.forward * shootingStrength;

		// Spawn the bullet on all of the clients
		NetworkServer.Spawn (bullet);
	}
}
