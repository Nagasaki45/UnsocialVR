using Dissonance;
using Dissonance.Audio.Capture;
using LibPDBinding;
using NAudio.Wave;
using System;
using System.Collections;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerExpectsBackchannel : MonoBehaviour, IMicrophoneSubscriber
{
    public string patch;

    private int patchHandle;
    private bool isPdReady;
    private int numberOfTicks;

    private PubSubClient pubSubClient;
    private PlayerTalking playerTalking;


    void Start()
    {
        pubSubClient = GetComponent<PubSubClient>();
        playerTalking = GetComponent<PlayerTalking>();
    }


    void Awake()
    {
        // Initialise Pd with 1 in and out and Unity's samplerate.
        int returnCode = LibPD.OpenAudio(1, 1, AudioSettings.outputSampleRate);
        if (returnCode != 0)
        {
            Debug.LogError("Pd failed to initialize");
        }

        // Subscribe to messages from Pd.
        LibPD.Print += ReceivePrint;
        LibPD.Bang += ReceiveBang;
        LibPD.Subscribe("backchannel");

        // Load the patch.
        string patchPath = Application.streamingAssetsPath + patch;
        Debug.Log("Pd loading patch: " + patchPath);
        patchHandle = LibPD.OpenPatch(patchPath);

        LibPD.ComputeAudio(true);

        // Subscribe to dissonance microphone capture
        DissonanceComms comms = GameObject.FindGameObjectWithTag("DissonanceSetup").GetComponent<DissonanceComms>();
        comms.MicrophoneCapture.Subscribe(this);
    }


    void Update()
    {
        LibPD.SendFloat("talking", playerTalking.isTalking ? 1 : 0);
    }


    void OnDestroy()
    {
        LibPD.ClosePatch(patchHandle);
        LibPD.Release();
    }


    // Pd receivers

    void ReceivePrint(string msg)
    {
        Debug.Log("Pd print: " + msg);
    }


    void ReceiveBang(string nameOfSend)
    {
        Debug.Log("Player expects backchannel");
        pubSubClient.CmdPublish("backchannels");
    }


    // IMicrophoneSubscriber interface callbacks

    public void ReceiveMicrophoneData(ArraySegment<float> segment, WaveFormat format)
    {
        // Get the values from the segment.
        float[] buffer = new float[segment.Count];
        for (int i = 0; i < segment.Count; i++)
        {
            buffer[i] = segment.Array[segment.Offset + i];
        }

        // libpd has a block size of 64 samples, the buffers are 960 samples,
        // hence 15 ticks are sent to Pd every time.
        LibPD.Process(15, buffer, buffer);
    }

    public void Reset() {}
}
