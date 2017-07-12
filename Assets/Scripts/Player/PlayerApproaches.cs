using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerApproaches : NetworkBehaviour {

	public float rayDistance;
	public Transform headTransform;

	private PlayerTalking playerTalking;
	private PlayerApproachedText playerApproachedText;

	private Ray attentionRay;
	private RaycastHit attentionHit;


	private void Start () {
		playerTalking = GetComponent<PlayerTalking> ();
		playerApproachedText = GameObject.FindGameObjectWithTag ("PlayerApproachedText").GetComponent<PlayerApproachedText> ();
		attentionRay = new Ray ();
	}
	

	private void Update () {
		attentionRay.origin = headTransform.position;
		attentionRay.direction = headTransform.forward;

		if (isLocalPlayer)
		{
			LocalPlayerAttention ();
		}
		else
		{
			RemotePlayerAttention();
		}
	}


	private void LocalPlayerAttention()
	{
		int attention = -1;
		if (Physics.Raycast (attentionRay, out attentionHit, rayDistance))
		{
			PlayerController otherPlayer = attentionHit.collider.gameObject.GetComponent<PlayerController> ();
			if (null != otherPlayer)
			{
				attention = (int) otherPlayer.netId.Value;
			}
		}
		PlayerController.localPlayerData.attention = attention;
	}


	private void RemotePlayerAttention()
	{
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
