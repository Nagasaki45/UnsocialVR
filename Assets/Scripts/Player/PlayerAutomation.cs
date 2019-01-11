using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAutomation : MonoBehaviour {

    public bool disabled;
    public int minimumAutomationDuration;
    public int maximumAutomationDuration;
    public float realPerAutomationDuration;
    public int initialRealDuration;

    public string[] automationModels;
    public string[] animations;
    public float animationCrossfadeSeconds;
    public float lerp;

    public string automationModel;
    public Transform trueHead;
    public Transform remoteHead;
    public Animator animator;

    // Notch in head movement data between 1 and 4 hertz
    private int sampleRate = 100;
    private float lowPassFreq = 1;
    private float highPassFreq = 4;
    private Interpolator yInterpolator;
    private Butterworth yLowpassFilter;
    private Butterworth yHighpassFilter;
    private Interpolator pitchInterpolator;
    private Butterworth pitchLowpassFilter;
    private Butterworth pitchHighpassFilter;


    void Start()
    {
        yInterpolator = new Interpolator(1.0 / sampleRate);
        yLowpassFilter = new Butterworth(lowPassFreq, sampleRate, Butterworth.PassType.Lowpass);
        yHighpassFilter = new Butterworth(lowPassFreq, sampleRate, Butterworth.PassType.Highpass);
        pitchInterpolator = new Interpolator(1.0 / sampleRate);
        pitchLowpassFilter = new Butterworth(lowPassFreq, sampleRate, Butterworth.PassType.Lowpass);
        pitchHighpassFilter = new Butterworth(lowPassFreq, sampleRate, Butterworth.PassType.Highpass);

        if (!disabled)
        {
            Invoke("StartAutomation", initialRealDuration);
        }
    }


    void Update()
    {
        float now = Time.time;
        Vector3 targetPosition;
        Quaternion targetRotation;

        // Always interpolate true head values so interpolator won't jump
        double y = (double) trueHead.position.y;
        double pitch = (double) trueHead.eulerAngles.x;
        pitch = (pitch + 90) % 360;  // So there's no 0 to cross
        foreach (double interpolated in yInterpolator.Interpolate(now, y))
        {
            // Notch by summing low pass and high pass
            y = yLowpassFilter.Filter(interpolated) + yHighpassFilter.Filter(interpolated);
        }
        foreach (double interpolated in pitchInterpolator.Interpolate(now, pitch))
        {
            // Notch by summing low pass and high pass
            pitch = pitchLowpassFilter.Filter(interpolated) + pitchHighpassFilter.Filter(interpolated);
        }

        if (!IsAutomated())
        {
            targetPosition = trueHead.position;
            targetRotation = trueHead.rotation;
        }
        else
        {
            targetPosition = new Vector3(trueHead.position.x, (float) y, trueHead.position.z);
            targetRotation = Quaternion.Euler((float) pitch - 90, trueHead.eulerAngles.y, trueHead.eulerAngles.z);
        }

        remoteHead.position = Vector3.Lerp(remoteHead.position, targetPosition, lerp * Time.deltaTime);
        remoteHead.rotation = Quaternion.Lerp(remoteHead.rotation, targetRotation, lerp * Time.deltaTime);
    }


    float AutomationDuration()
    {
        return Random.Range(minimumAutomationDuration, maximumAutomationDuration);
    }


    public bool IsAutomated()
    {
        return !System.String.IsNullOrEmpty(automationModel);
    }


    public void StartAutomation()
    {
        if (!IsAutomated())
        {
            int i = UnityEngine.Random.Range(0, automationModels.Length);
            automationModel = automationModels[i];
            Logger.Event($"Partner automation started - Model: {automationModel}");

            Invoke("StopAutomation", AutomationDuration());
        }
    }


    public void StopAutomation()
    {
        if (IsAutomated())
        {
            automationModel = null;
            Logger.Event("Partner automation stopped");

            Invoke("StartAutomation", AutomationDuration() * realPerAutomationDuration);
        }
    }


    public void Nod(string model)
    {
        if (automationModel != model)
        {
            return;
        }
        int i = UnityEngine.Random.Range(0, animations.Length);
        string animation = animations[i];
        Logger.Event($"Partner automatic nod - Model: {automationModel}. Animation: {animation}");
        animator.CrossFade(animation, animationCrossfadeSeconds);
    }
}
