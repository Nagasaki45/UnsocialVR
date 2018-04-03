using System.Collections;
using System.Net.Sockets;
using UnityEngine;

public class PlayerExpectsBackchannel : MonoBehaviour
{
    public string tcpServerAddress;
    public int tcpServerPort;

    private PubSubClient pubSubClient;
    private PlayerTalking playerTalking;
    private TcpClient tcpClient;
    private NetworkStream networkStream;


    void Start()
    {
        pubSubClient = GetComponent<PubSubClient>();
        playerTalking = GetComponent<PlayerTalking>();
    }

    void Awake()
    {
        // Connect to TCP server
        tcpClient = new TcpClient(tcpServerAddress, tcpServerPort);
        networkStream = tcpClient.GetStream();

        StartCoroutine(PollStream());
    }


    void Update()
    {
        byte message = (byte) (playerTalking.isTalking ? 1 : 0);
        networkStream.WriteByte(message);
    }


    IEnumerator PollStream()
    {
        while (true)
        {
            if (networkStream.DataAvailable)
            {
                byte[] throwaway = new byte[1024];
                networkStream.Read(throwaway, 0, throwaway.Length);
                Debug.Log("Player expects backchannel");
                pubSubClient.CmdPublish("backchannels");
            }
            // Let the engine run for a frame.
            yield return null;
        }
    }


    void OnDestroy()
    {
        networkStream.Close();
        tcpClient.Close();
    }
}
