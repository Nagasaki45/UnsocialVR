using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SnakeController : MonoBehaviour {

    public GameObject bodyPartPrefab;
    public GameObject foodPrefab;
    public float movementPeriod;
    public int initialSize;
    public float touchpadThreshold;

    SnakeSpawner snakeSpawner;
    List<Transform> parts;
    Vector3 direction;
    Vector3 previousDirection;
    bool shouldGrow;
    SteamVR_TrackedObject trackedObj;

    SteamVR_Controller.Device device
    {
        get { return SteamVR_Controller.Input((int) trackedObj.index); }
    }


    void Start()
    {
        trackedObj = GameObject.FindGameObjectWithTag("LeftHand").GetComponent<SteamVR_TrackedObject>();
        Init();
        InvokeRepeating("Move", 0f, movementPeriod);
    }


    void Init()
    {
        snakeSpawner = GetComponentInParent<SnakeSpawner>();
        parts = new List<Transform>();
        direction = Vector3.forward;
        previousDirection = Vector3.forward;
        shouldGrow = false;

        for (int i = 1; i < initialSize; i++)
        {
            Grow(transform.position - transform.forward * transform.lossyScale[0] * i);
        }
    }


    void Update()
    {
        Vector2 touch = device.GetAxis();
        if (touch.x > touchpadThreshold && previousDirection != Vector3.left)
        {
            direction = Vector3.right;
        }
        else if (touch.x < -touchpadThreshold && previousDirection != Vector3.right)
        {
            direction = Vector3.left;
        }
        else if (touch.y > touchpadThreshold && previousDirection != Vector3.back)
        {
            direction = Vector3.forward;
        }
        else if (touch.y < -touchpadThreshold && previousDirection != Vector3.forward)
        {
            direction = Vector3.back;
        }
    }


    void Grow(Vector3 pos)
    {
        Transform t = Instantiate(bodyPartPrefab, pos, transform.rotation).transform;
        t.parent = transform.parent;
        t.localScale = transform.localScale;
        parts.Add(t);
    }


    void Move()
    {
        Vector3 growHere = parts[parts.Count - 1].position;

        for (int i = parts.Count - 1; i > 0; i--)
        {
            parts[i].position = parts[i - 1].position;
        }
        parts[0].position = transform.position;

        if (shouldGrow)
        {
            Grow(growHere);
        }

        transform.Translate(direction * transform.lossyScale[0]);

        shouldGrow = false;
        previousDirection = direction;
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Food")
        {
            Destroy(other.gameObject);
            Eat();
        }
        else
        {
            Die();
        }
    }


    void Eat()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerState>().score += 1;
        shouldGrow = true;
        snakeSpawner.SpawnFood();
    }


    void Die()
    {
        foreach (Transform t in parts)
        {
            Destroy(t.gameObject);
        }
        Destroy(gameObject);
        snakeSpawner.Restart();
    }
}
