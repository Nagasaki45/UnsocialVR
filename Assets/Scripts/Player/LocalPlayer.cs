using UnityEngine;
using UnityEngine.Networking;


public class LocalPlayer : NetworkBehaviour {

    public GameObject performative;
    public GameObject self;

    public void Start()
    {
        if (isLocalPlayer)
        {
            // Hide performative
            foreach (var x in performative.GetComponentsInChildren<MeshRenderer>()) {
                Destroy(x);
            }

            // No collider for local performative to ensure I'm not gazing at myself.
            Destroy(performative.GetComponentInChildren<SphereCollider>());
        }
        else
        {
            // Destroy Self, only Performative is used
            Destroy(self);

            // No faking generators
            Destroy(GetComponentInChildren<LookAtSpeaker>());
            foreach(var x in GetComponentsInChildren<NaturalMovement>())
            {
                Destroy(x);
            }

            // No need for animator and gaze tracker
            Destroy(GetComponentInChildren<PlayerAnimator>());
            Destroy(GetComponentInChildren<Animator>());
            Destroy(GetComponentInChildren<PlayerGaze>());
        }
    }
}
