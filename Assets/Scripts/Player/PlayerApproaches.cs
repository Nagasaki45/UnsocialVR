using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerApproaches : NetworkBehaviour {

	public float rayDistance;

	private PlayerTalking playerTalking;
	private PlayerApproachedText playerApproachedText;

	private Ray attentionRay;
	private RaycastHit attentionHit;


	void Start () {
		playerTalking = GetComponent<PlayerTalking> ();
		playerApproachedText = GameObject.FindGameObjectWithTag ("PlayerApproachedText").GetComponent<PlayerApproachedText> ();
		attentionRay = new Ray ();
	}
	

	void Update () {
		if (!isLocalPlayer)
		{
			attentionRay.origin = transform.position;
			attentionRay.direction = transform.forward;

			if (Physics.Raycast (attentionRay, out attentionHit, rayDistance))
			{
				if (attentionHit.collider.gameObject.tag == "AutopilotMarker" && playerTalking.isTalking)
				{
					Debug.Log ("Someone is talking with localPlayer!");
					playerApproachedText.Flash ();
				}
			}
		}
	}
}
