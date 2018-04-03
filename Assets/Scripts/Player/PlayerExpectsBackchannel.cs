using UnityEngine;

public class PlayerExpectsBackchannel : MonoBehaviour, IMicrophoneSubscriber
{
    private PubSubClient pubSubClient;
    private PlayerTalking playerTalking;


    void Start()
    {
        pubSubClient = GetComponent<PubSubClient>();
        playerTalking = GetComponent<PlayerTalking>();
    }


    void ReceiveBang()
    {
        Debug.Log("Player expects backchannel");
        pubSubClient.CmdPublish("backchannels");
    }
}
