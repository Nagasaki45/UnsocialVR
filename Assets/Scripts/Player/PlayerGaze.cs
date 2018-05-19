using UnityEngine;
using UnityEngine.Networking;


public class PlayerGaze : MonoBehaviour {

    public GameObject gazedObj = null;
    public float rayDistance;

    private Ray attentionRay;
    private RaycastHit attentionHit;


    private void Start () {
        attentionRay = new Ray ();
    }


    private void Update () {
        attentionRay.origin = transform.position;
        attentionRay.direction = transform.forward;

        if (Physics.Raycast (attentionRay, out attentionHit, rayDistance))
        {
            gazedObj = attentionHit.collider.gameObject;
        }
        else
        {
            gazedObj = null;
        }
    }


    public int GetGazedNetId()
    {
        if (null != gazedObj)
        {
            NetworkIdentity id = gazedObj.GetComponent<NetworkIdentity>();
            if (null != id)
            {
                return (int) id.netId.Value;
            }
        }
        return -1;
    }
}
