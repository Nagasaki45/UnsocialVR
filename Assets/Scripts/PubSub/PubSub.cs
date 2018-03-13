using System.Collections.Generic;
using UnityEngine;


public class PubSub : MonoBehaviour
{

    private List<SubscriberTopicPair> subscribers;


    void Start()
    {
        subscribers = new List<SubscriberTopicPair>();
    }


    public void Subscribe(PubSubClient subscriber, string topic)
    {
        subscribers.Add(new SubscriberTopicPair(subscriber, topic));
    }


    public void Unsubscribe(PubSubClient subscriber)
    {
        subscribers.RemoveAll(x => x.subscriber == subscriber);
    }


    public void Publish(string topic)
    {
        // Remove dead clients
        subscribers.RemoveAll(x => x.subscriber == null);

        // Tell the players registered for this topic to nod.
        foreach (var gameObjectTheoryPair in subscribers)
        {
            if (gameObjectTheoryPair.topic.Equals(topic))
            {
                gameObjectTheoryPair.subscriber.RpcNod();
            }
        }
    }


    class SubscriberTopicPair
    {
        public PubSubClient subscriber;
        public string topic;

        public SubscriberTopicPair(PubSubClient subscriber, string topic)
        {
            this.subscriber = subscriber;
            this.topic = topic;
        }
    }
}
