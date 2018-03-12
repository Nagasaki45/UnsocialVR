using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerHeadNod : NetworkBehaviour {

    public Transform headTransform;
    public int bufferSize;
    public int sampleRate;
    public double noddingThreshold;  // the minimum speed to detect a nod

    private CircularBuffer<double> readings;
    private Interpolator interpolator;
    private Butterworth lowPassFilter;
    private Butterworth highPassFilter;

    private Animator animator;

    private PubSub pubSub;


    void Start()
    {
        readings = new CircularBuffer<double>(bufferSize);
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
            float value = headTransform.position.y;
            foreach (double val in interpolator.Interpolate(now, value))
            {
                double v = lowPassFilter.Filter(val);
                v = highPassFilter.Filter(v);
                readings.Add(v);
            }

            float speed = HeadSpeedMetersPerSecond(readings.ToArray());

            // Nod happens upon negative speed, larger than threshold.
            if (speed < -noddingThreshold)
            {
                // Tell all listeners to nod in 4 seconds delay.
                Debug.Log("NODDING!");
                Invoke("Nod", 4);
            }
        }
    }


    private float HeadSpeedMetersPerSecond(double[] readings)
    {
        // x is the relative time of the sample
        double[] x = new double[readings.Length];
        for (int i = 0; i < x.Length; i++)
        {
            x[i] = ((double) i) / sampleRate;
        }

        // fit.x is slope and fit.y is intercept.
        Vector2 fit = LinearRegression.Fit(x, readings);

        return fit.x;
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
