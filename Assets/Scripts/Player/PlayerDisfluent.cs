using Dissonance;
using Dissonance.Audio.Capture;
using NAudio.Wave;
using System;
using System.Collections;
using System.IO;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerDisfluent : MonoBehaviour, IMicrophoneSubscriber
{
    public string[] repairTags;
    public int tcpServerPort;

    private PlayerLogger logger;
    private PubSubClient pubSubClient;
    private TcpClient tcpClient;
    private NetworkStream networkStream;
    private StreamReader streamReader;


    void Start()
    {
        logger = GetComponent<PlayerLogger>();
        pubSubClient = GetComponent<PubSubClient>();

        // Subscribe to dissonance microphone capture
        DissonanceComms comms = GameObject.FindGameObjectWithTag("DissonanceSetup").GetComponent<DissonanceComms>();
        comms.MicrophoneCapture.Subscribe(this);

        // Connect to TCP server
        NetworkManager nm = GameObject.FindGameObjectWithTag("Network").GetComponent<NetworkManager>();
        tcpClient = new TcpClient(nm.networkAddress, tcpServerPort);
        networkStream = tcpClient.GetStream();
        streamReader = new StreamReader(networkStream);

        StartCoroutine(PollStream());
    }


    IEnumerator PollStream()
    {
        while (true)
        {
            while (networkStream.DataAvailable)
            {
                string line = streamReader.ReadLine();
                foreach (string repairTag in repairTags)
                {
                    if (line.Contains(repairTag))
                    {
                        logger.Event("Player disfluent");
                        pubSubClient.CmdPublish("disfluency");
                    }
                }
            }
            // Let the engine run for a frame.
            yield return null;
        }
    }


    // IMicrophoneSubscriber interface callbacks

    public void ReceiveMicrophoneData(ArraySegment<float> segment, WaveFormat format)
    {
        // Get the values from the segment.
        // Two bytes in each 16 bit sample.
        byte[] buffer = new byte[segment.Count * 2];
        for (int i = 0; i < segment.Count; i++)
        {
            short sample = (short) (32768 * segment.Array[segment.Offset + i]);
            buffer[2 * i]     = (byte) sample;
            buffer[2 * i + 1] = (byte) (sample >> 8);
        }

        // Send the buffer to the deep disfluency tagger TCP server.
        networkStream.Write(buffer, 0, buffer.Length);
    }

    public void Reset() {}
}
