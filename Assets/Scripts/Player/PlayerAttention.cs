using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerAttention : NetworkBehaviour {

	public float rayDistance;

	private Ray attentionRay;
	private RaycastHit attentionHit;


	void Awake () {
		attentionRay = new Ray ();
	}
	

	void Update () {
		if (isLocalPlayer)
		{
			int attentionTo = -1;

			attentionRay.origin = transform.position;
			attentionRay.direction = transform.forward;

			if (Physics.Raycast (attentionRay, out attentionHit, rayDistance))
			{
				PlayerController pc = attentionHit.collider.gameObject.GetComponent<PlayerController> ();
				if (pc != null)
				{
					attentionTo = (int) pc.netId.Value;
				}
			}
			PlayerController.localPlayerData.attentionTo = attentionTo;
		}
	}
}
