using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SnakeSpawner : MonoBehaviour {

    public GameObject snakePrefab;
    public GameObject foodPrefab;


    void Start()
    {
        Restart();
    }


    public void Restart() {
        foreach(GameObject g in GameObject.FindGameObjectsWithTag("Food"))
        {
            Destroy(g);
        }
        GameObject snake = Instantiate(snakePrefab, transform);
        snake.transform.parent = transform;
        SpawnFood();
    }


    public void SpawnFood()
    {
        float randX = ((int) Random.Range(-9, 9));
        float randZ = ((int) Random.Range(-9, 9));
        Vector3 randPos = new Vector3(randX, 0, randZ);
        Transform t = Instantiate(foodPrefab, transform).transform;
        t.parent = transform;
        t.localPosition = randPos;
        t.localScale = transform.localScale;
    }
}
