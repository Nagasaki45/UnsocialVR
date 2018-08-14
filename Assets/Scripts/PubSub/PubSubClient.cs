using UnityEngine;
using UnityEngine.Networking;

public class PubSubClient : NetworkBehaviour
{
    public Animator animator;
    public string[] animations;
    public float animationCrossfadeSeconds;

    PubSub pubSub;


    void Start()
    {
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


    public void ServerSideNod()
    {
        int i = Random.Range(0, animations.Length);
        RpcNod(animations[i]);
    }


    [ClientRpc]
    public void RpcNod(string animation)
    {
        if (isLocalPlayer)
        {
            Logger.Event("Nodding fake " + animation);
        }
        animator.CrossFade(animation, animationCrossfadeSeconds);
    }
}
