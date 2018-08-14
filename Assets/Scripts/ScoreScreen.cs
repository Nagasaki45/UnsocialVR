using UnityEngine;


public class ScoreScreen : MonoBehaviour {

    public Transform bindedTransform;
    public TextMesh textMesh;
    public PlayerState playerState;


    void Start()
    {
        transform.parent = null;
    }


    void Update()
    {
        textMesh.text = playerState.GetScore().ToString();
        transform.position = bindedTransform.position;
        transform.rotation = bindedTransform.rotation;
    }
}
