using System.Collections;
using System.Net.Sockets;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerExpectsBackchannel : MonoBehaviour
{
    public int serverPort;

    private PubSubClient pubSubClient;


    void Start()
    {
        pubSubClient = GetComponent<PubSubClient>();
        StartCoroutine("RunForever");
    }


    IEnumerator RunForever()
    {
        UdpClient sock = new UdpClient(serverPort);

        while(true)
        {
            Task task = sock.ReceiveAsync();
            while (!task.IsCompleted)
            {
                yield return null;
            }
            Debug.Log("Expecting a backchannel");
            // Only the speaker publishes!
            if (PlayerTalking.speaker == gameObject)
            {
                pubSubClient.CmdPublish("backchannels");
            }
        }
    }
}
