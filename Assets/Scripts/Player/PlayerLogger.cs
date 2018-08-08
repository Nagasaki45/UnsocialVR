using System;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;


public class PlayerLogger : MonoBehaviour {

    public float frequency;
    public Transform head;
    public Transform leftHand;
    public Transform rightHand;

    private PlayerTalking playerTalking;
    private PlayerGaze playerGaze;

    private StreamWriter continuousWriter;
    private string[] continuousHeader = {
        "t",
        "headX", "headY", "headZ",
        "headRotX", "headRotY", "headRotZ",
        "leftHandX", "leftHandY", "leftHandZ",
        "leftHandRotX", "leftHandRotY", "leftHandRotZ",
        "rightHandX", "rightHandY", "rightHandZ",
        "rightHandRotX", "rightHandRotY", "rightHandRotZ",
        "talking", "gazedNetId"
    };


    void Awake()
    {
        playerTalking = GetComponentInParent<PlayerTalking>();
        playerGaze = GetComponentInChildren<PlayerGaze>();
    }


    void Start()
    {
        string id = GetComponentInParent<NetworkIdentity>().netId.ToString();
        string filename = DateTime.Now.ToString("yyyyMMdd_HHmmss") + "_" + id;
        Logger.filename = filename + ".log";
        continuousWriter = new StreamWriter(filename + ".csv");
        continuousWriter.WriteLine(String.Join(",", continuousHeader) + "\n");
        InvokeRepeating("WriteContinuously", 0f, 1 / frequency);
    }


    void WriteContinuously()
    {
        string[] values = {
            Time.time.ToString(),
            head.position.x.ToString(),
            head.position.y.ToString(),
            head.position.z.ToString(),
            head.eulerAngles.x.ToString(),
            head.eulerAngles.y.ToString(),
            head.eulerAngles.z.ToString(),
            leftHand.position.x.ToString(),
            leftHand.position.y.ToString(),
            leftHand.position.z.ToString(),
            leftHand.eulerAngles.x.ToString(),
            leftHand.eulerAngles.y.ToString(),
            leftHand.eulerAngles.z.ToString(),
            rightHand.position.x.ToString(),
            rightHand.position.y.ToString(),
            rightHand.position.z.ToString(),
            rightHand.eulerAngles.x.ToString(),
            rightHand.eulerAngles.y.ToString(),
            rightHand.eulerAngles.z.ToString(),
            playerTalking.isTalking ? "1" : "0",
            playerGaze.GetGazedNetId().ToString(),
        };
        string csv = String.Join(",", values);
        continuousWriter.WriteLine(csv + "\n");
    }


    void OnDestroy()
    {
        if (null != continuousWriter)
        {
            continuousWriter.Close();
        }
    }
}
