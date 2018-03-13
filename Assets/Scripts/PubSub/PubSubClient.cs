using UnityEngine;
using UnityEngine.Networking;

public class PubSubClient : NetworkBehaviour
{
    private Animator animator;
    private PubSub pubSub;


    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();

        if (isServer)
        {
            pubSub = GameObject.FindGameObjectWithTag("PubSub").GetComponent<PubSub>();
        }
    }


    [Command]
    public void CmdPublish(string theory)
    {
        pubSub.Publish(theory);
    }


    [Command]
    public void CmdSubscribeToHeadNods(string theory)
    {
        pubSub.Subscribe(this, theory);
    }


    [Command]
    public void CmdUnsubscribeFromHeadNods()
    {
        pubSub.Unsubscribe(this);
    }


    [ClientRpc]
    public void RpcNod()
    {
        animator.SetTrigger("nodding");
    }
}
