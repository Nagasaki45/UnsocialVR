using UnityEngine;
using UnityEngine.Networking;


public class LocalPlayer : NetworkBehaviour {

    public void Start()
    {
        if (isLocalPlayer)
        {
            // Faking generators
            GetComponentInChildren<LookAtSpeaker>().enabled = true;
            foreach(var naturalMovement in GetComponentsInChildren<NaturalMovement>())
            {
                naturalMovement.enabled = true;
            }

            // Hide performative
            GameObject performative = transform.Find("Performative").gameObject;
            foreach (var r in performative.GetComponentsInChildren<MeshRenderer>()) {
                r.enabled = false;
            }

            // No collider for local performative to ensure I'm not gazing at myself.
            performative.GetComponentInChildren<SphereCollider>().enabled = false;
        }
        else
        {
            // Destroy Self, only Performative is used
            Destroy(transform.Find("Self").gameObject);
        }

    }
}
