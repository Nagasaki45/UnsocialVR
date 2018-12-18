using System;
using UnityEngine;


public class PlayerScore : MonoBehaviour {

    int score;
    TextMesh scoreTextMesh;


    void Start()
    {
        score = 0;
        scoreTextMesh = GetComponent<TextMesh>();
    }


    void Update()
    {
        scoreTextMesh.text = "Score: " + score;
    }


    public void Add(int value)
    {
        score += value;
    }
}
