using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;


public class PlayerApproaches : NetworkBehaviour {

	public float rayDistance;
	public Transform headTransform;
	public PlayerController attention;

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
		PlayerController temp = null;
		if (Physics.Raycast (attentionRay, out attentionHit, rayDistance))
		{
			PlayerController otherPlayer = attentionHit.collider.gameObject.GetComponent<PlayerController> ();
			if (null != otherPlayer)
			{
				temp = otherPlayer;
			}
		}
		attention = temp;
		PlayerController.localPlayerData.attention = (null != attention) ? (int) attention.netId.Value : -1;
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
