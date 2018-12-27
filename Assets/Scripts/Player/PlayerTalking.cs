using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using Dissonance;
using Dissonance.VAD;


public class PlayerTalking : NetworkBehaviour {

    [SyncVar(hook = "OnTalkingChange")]
    public bool isTalking;

    [SyncVar(hook = "OnSpeakerChange")]
    public bool speaker;

    public double talkingRatio;

    public float talkingAmplitudeThreshold;
    public int sampleRate;
    public float lowPassCutoffFrequency;

    DissonanceComms comms;

    // Only on the server
    Interpolator interpolator;
    Butterworth lowPassFilter;


    private void Start()
    {
        comms = GameObject.FindGameObjectWithTag ("DissonanceSetup").GetComponent<DissonanceComms> ();

        if (isServer)
        {
            interpolator = new Interpolator(1.0 / sampleRate);
            lowPassFilter = new Butterworth(lowPassCutoffFrequency, sampleRate, Butterworth.PassType.Lowpass);
        }
    }


    private void Update ()
    {
        // Update talking ratio on the server
        if (isServer)
        {
            float now = Time.time;
            foreach (double val in interpolator.Interpolate(now, isTalking ? 1.0 : 0.0))
            {
                talkingRatio = lowPassFilter.Filter(val);
            }
        }

        // Local players to update isTalking
        if (!isLocalPlayer)
        {
            return;
        }
        bool speechDetected = IsThereSpeechInAudio();
        if (speechDetected && !isTalking)
        {
            CmdSetTalkingState(true);
        }
        else if (!speechDetected && isTalking)
        {
            CmdSetTalkingState(false);
        }
    }


    [Command]
    void CmdSetTalkingState (bool onOff)
    {
        isTalking = onOff;
    }


    void OnTalkingChange(bool newState)
    {
        isTalking = newState;
        if (isLocalPlayer)
        {
            Logger.Event($"Talking - State: {newState}");
        }
    }


    void OnSpeakerChange(bool newState)
    {
        speaker = newState;
        if (isLocalPlayer)
        {
            Logger.Event($"Speaking - State: {newState}");
            GetComponentInChildren<PlayerPartner>().ToggleAutomation();
        }
    }


    bool IsThereSpeechInAudio () {
        foreach (VoicePlayerState voicePlayerState in comms.Players)
        {
            // UglyHack (c) to check the type of a private class.
            if (voicePlayerState.GetType ().Name == "LocalVoicePlayerState")
            {
                return voicePlayerState.IsSpeaking && (voicePlayerState.Amplitude > talkingAmplitudeThreshold);
            }
        }
        Debug.LogError("Failed to detect talking state");
        return false;
    }
}
