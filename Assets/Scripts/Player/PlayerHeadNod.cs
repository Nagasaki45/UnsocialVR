using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerHeadNod : NetworkBehaviour {

    public Transform headTransform;
    public int bufferSize;
    public int sampleRate;
    public double noddingThreshold;  // the minimum movement to detect a nod
    public double notNoddingThreshold;  // epsilon value

    private Interpolator interpolator;
    private Butterworth lowPassFilter;
    private Butterworth highPassFilter;

    private bool readyToNod;

    private Animator animator;

    private PubSub pubSub;


    void Start()
    {
        interpolator = new Interpolator(1.0 / sampleRate);
        lowPassFilter = new Butterworth(4, sampleRate, Butterworth.PassType.Lowpass);
        highPassFilter = new Butterworth(1, sampleRate, Butterworth.PassType.Highpass);

        animator = GetComponent<Animator> ();

        if (isServer)
        {
            pubSub = GameObject.FindGameObjectWithTag("PubSub").GetComponent<PubSub>();
        }

    }


    void Update()
    {
        if (isLocalPlayer)
        {
            float now = Time.time;
            double value = (double) headTransform.position.y;
            foreach (double val in interpolator.Interpolate(now, value))
            {
                value = lowPassFilter.Filter(val);
                value = highPassFilter.Filter(value);

                // Nod happens upon negative movement, larger than threshold.
                if (readyToNod && value < -noddingThreshold)
                {
                    readyToNod = false;

                    // Tell all listeners to nod in 4 seconds delay.
                    Debug.Log("NODDING!");
                    Invoke("Nod", 4);
                }

                if (value < notNoddingThreshold && value > -notNoddingThreshold)
                {
                    readyToNod = true;
                }
            }

        }
    }


    // A `CmdNod` wrapper that makes sure the user is still the speaker.
    private void Nod()
    {
        // Only the speaker publishes!
        if (PlayerTalking.speaker == gameObject)
        {
            CmdNod();
        }
    }


    // Runs on the server (after 4 seconds delay). Tells the listeners to nod.
    [Command]
    private void CmdNod()
    {
        pubSub.Publish("mimicry");
    }


    [Command]
    public void CmdSubscribeToHeadNods()
    {
        // TODO pick a theory in random.
        string theory = "mimicry";

        Debug.Log("Faking using " + theory);
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
