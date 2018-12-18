using UnityEngine;
using UnityEngine.Networking;


public class LocalPlayer : NetworkBehaviour {

    public GameObject local;
    public GameObject remote;


    public void Start()
    {
        if (isLocalPlayer)
        {
            Destroy(remote);
        }
        else
        {
            Destroy(local);

            // Non local player shouldn't bind to VR setup but get transforms from network.
            foreach(var x in GetComponentsInChildren<BindTransform>())
            {
                Destroy(x);
            }
        }
    }
}
