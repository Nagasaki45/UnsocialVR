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
    public int tcpServerPort;

    private TcpClient tcpClient;
    private NetworkStream networkStream;
    private StreamReader streamReader;
    private bool ready = false;


    void Start()
    {
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
                if (ready) {
                    Logger.Event("Player disfluent");
                    GetComponentInParent<PubSubClient>().CmdPublish("disfluency");
                } else {
                    // First message should be a 'ready' message from the server
                    ready = true;

                    // Subscribe to dissonance microphone capture
                    GameObject
                        .FindGameObjectWithTag("DissonanceSetup")
                        .GetComponent<DissonanceComms>()
                        .MicrophoneCapture
                        .Subscribe(this);
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
        networkStream.WriteAsync(buffer, 0, buffer.Length);
    }

    public void Reset() {}
}
