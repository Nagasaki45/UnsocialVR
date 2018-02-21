using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;


public class EndlessLoopingBufferTest {

    [Test]
    public void Simple() {
        EndlessLoopingBuffer<int> buffer = new EndlessLoopingBuffer<int>(new int[4]);
        buffer.Write(0);
        buffer.Write(1);
        buffer.Write(2);
        buffer.Write(3);
        Assert.AreEqual(2, buffer.Read());
        Assert.AreEqual(1, buffer.Read());
        Assert.AreEqual(0, buffer.Read());
        Assert.AreEqual(1, buffer.Read());
        Assert.AreEqual(2, buffer.Read());
        Assert.AreEqual(3, buffer.Read());
        Assert.AreEqual(2, buffer.Read());
    }


    [Test]
    public void ZeroPointNotZero() {
        EndlessLoopingBuffer<int> buffer = new EndlessLoopingBuffer<int>(new int[3]);
        buffer.Write(0);
        buffer.Write(1);
        buffer.Write(2);
        buffer.Write(3);
        Assert.AreEqual(2, buffer.Read());
        Assert.AreEqual(1, buffer.Read());
        Assert.AreEqual(2, buffer.Read());
        Assert.AreEqual(3, buffer.Read());
        Assert.AreEqual(2, buffer.Read());
        Assert.AreEqual(1, buffer.Read());
        Assert.AreEqual(2, buffer.Read());
    }


    [Test]
    public void MultipleWritesReads() {
        EndlessLoopingBuffer<int> buffer = new EndlessLoopingBuffer<int>(new int[4]);
        buffer.Write(0);
        buffer.Write(1);
        buffer.Write(2);
        buffer.Write(3);
        Assert.AreEqual(2, buffer.Read());
        Assert.AreEqual(1, buffer.Read());
        Assert.AreEqual(0, buffer.Read());
        Assert.AreEqual(1, buffer.Read());
        Assert.AreEqual(2, buffer.Read());
        buffer.Write(100);  // Overriding index 2 with value 2
        Assert.AreEqual(1, buffer.Read());
        Assert.AreEqual(0, buffer.Read());
        Assert.AreEqual(3, buffer.Read());
        Assert.AreEqual(0, buffer.Read());
        Assert.AreEqual(1, buffer.Read());
        Assert.AreEqual(100, buffer.Read());
    }
}
