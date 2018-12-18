using UnityEngine;

public class Mimicry : MonoBehaviour {

    public Transform headTransform;
    public int bufferSize;
    public int sampleRate;
    public double noddingThreshold;  // the minimum movement to detect a nod
    public double notNoddingThreshold;  // epsilon value

    private PlayerPartner playerPartner;
    private Interpolator interpolator;
    private Butterworth lowPassFilter;
    private Butterworth highPassFilter;

    private bool readyToNod;


    void Start()
    {
        playerPartner = GetComponent<PlayerPartner>();
        interpolator = new Interpolator(1.0 / sampleRate);
        lowPassFilter = new Butterworth(4, sampleRate, Butterworth.PassType.Lowpass);
        highPassFilter = new Butterworth(1, sampleRate, Butterworth.PassType.Highpass);
    }


    void Update()
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

                // Tell all listeners to nod after delay.
                Logger.Event("Nodding real");
                Invoke("Nod", 0.6f);
            }

            if (value < notNoddingThreshold && value > -notNoddingThreshold)
            {
                readyToNod = true;
            }
        }
    }


    private void Nod()
    {
        playerPartner.Nod("mimicry");
    }
}
