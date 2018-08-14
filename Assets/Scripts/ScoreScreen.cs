using UnityEngine;


public class ScoreScreen : MonoBehaviour {

    public TextMesh textMesh;
    public PlayerState playerState;


    void Start()
    {
    }


    void Update()
    {
        textMesh.text = playerState.score.ToString();
    }
}
