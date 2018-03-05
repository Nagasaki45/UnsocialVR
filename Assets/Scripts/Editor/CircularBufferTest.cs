using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;


public class CircularBufferTest {

    [Test]
    public void Simple() {
        CircularBuffer<int> buffer = new CircularBuffer<int>(3);
        buffer.Add(0);
        buffer.Add(1);
        buffer.Add(2);
        buffer.Add(3);

        Assert.AreEqual(1, buffer.Get(0));
        Assert.AreEqual(2, buffer.Get(1));
        Assert.AreEqual(3, buffer.Get(2));
    }


    [Test]
    public void Length() {
        CircularBuffer<int> buffer = new CircularBuffer<int>(5);
        Assert.AreEqual(5, buffer.Length);
    }

    [Test]
    public void ToArray() {
        CircularBuffer<int> buffer = new CircularBuffer<int>(3);
        buffer.Add(0);
        buffer.Add(1);
        buffer.Add(2);
        buffer.Add(3);

        int[] expected = {1, 2, 3};
        Assert.AreEqual(expected, buffer.ToArray());
    }
}
