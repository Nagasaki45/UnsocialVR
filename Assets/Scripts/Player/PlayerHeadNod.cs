using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerHeadNod : NetworkBehaviour {

    public Transform headTransform;
    public int bufferSize;
    public int sampleRate;
    public double noddingThreshold;

    private CircularBuffer<double> readings;
    private Interpolator interpolator;
    private Butterworth lowPassFilter;
    private Butterworth highPassFilter;


    void Start()
    {
        readings = new CircularBuffer<double>(bufferSize);
        interpolator = new Interpolator(1.0 / sampleRate);
        lowPassFilter = new Butterworth(4, sampleRate, Butterworth.PassType.Lowpass);
        highPassFilter = new Butterworth(1, sampleRate, Butterworth.PassType.Highpass);
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

            // Speed moving down is negative
            if (speed < -noddingThreshold)
            {
                // TODO nod
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

        return fit.x * sampleRate;
    }
}
