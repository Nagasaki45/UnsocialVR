using System.Collections;
using System.Net.Sockets;
using UnityEngine;

public class Backchannels : MonoBehaviour
{
    public string tcpServerAddress;
    public int tcpServerPort;

    private PlayerPartner playerPartner;
    private PlayerTalking playerTalking;
    private TcpClient tcpClient;
    private NetworkStream networkStream;


    void Start()
    {
        playerPartner = GetComponent<PlayerPartner>();
        playerTalking = GetComponentInParent<PlayerTalking>();

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
                Logger.Event("Expecting a backchannel");
                playerPartner.Nod("backchannels");
            }
            // Let the engine run for a frame.
            yield return null;
        }
    }
}
