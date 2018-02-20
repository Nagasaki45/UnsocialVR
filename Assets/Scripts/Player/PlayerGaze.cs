using UnityEngine;


public class PlayerGaze : MonoBehaviour {

    public GameObject gazedObj = null;
    public float rayDistance;
    public Transform headTransform;

    private Ray attentionRay;
    private RaycastHit attentionHit;


    private void Start () {
        attentionRay = new Ray ();
    }


    private void Update () {
        attentionRay.origin = headTransform.position;
        attentionRay.direction = headTransform.forward;

        if (Physics.Raycast (attentionRay, out attentionHit, rayDistance))
        {
            gazedObj = attentionHit.collider.gameObject;
        }
        else
        {
            gazedObj = null;
        }
	}
}
