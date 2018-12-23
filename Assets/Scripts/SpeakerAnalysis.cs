using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpeakerAnalysis : MonoBehaviour
{
    public float minRatioForSpeakerChange;
    public float hysteresis;

    PlayerTalking speaker;


    void Update()
    {
        foreach (var player in GameObject.FindGameObjectsWithTag("Player"))
        {
            PlayerTalking talker = player.GetComponent<PlayerTalking>();
            if (talker == speaker)
            {
                continue;
            }
            if (null == speaker)
            {
                talker.speaker = true;
                speaker = talker;
            }
            else if ((talker.talkingRatio > speaker.talkingRatio + hysteresis) && (talker.talkingRatio > minRatioForSpeakerChange))
            {
                speaker.speaker = false;
                talker.speaker = true;
                speaker = talker;
            }
        }
    }
}
