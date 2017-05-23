using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttention : MonoBehaviour {

	public float rayDistance;
	public int attentionTo;

	private Ray attentionRay;
	private RaycastHit attentionHit;


	void Awake () {
		attentionRay = new Ray ();
	}
	

	void Update () {

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
		else
		{
			attentionTo = -1;
		}
	}
}
