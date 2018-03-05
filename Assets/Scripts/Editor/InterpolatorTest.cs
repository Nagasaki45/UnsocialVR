using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;


public class InterpolatorTest {

    [Test]
    public void TestInterpolate() {
        Interpolator i = new Interpolator(0.5);
        Assert.AreEqual(new double[] {100},                i.Interpolate(1,   100));
        Assert.AreEqual(new double[] {150, 200},           i.Interpolate(2,   200));
        Assert.AreEqual(new double[] {250},                i.Interpolate(2.5, 250));
        Assert.AreEqual(new double[] {300},                i.Interpolate(3.1, 310));
        Assert.AreEqual(new double[] {},                   i.Interpolate(3.3, 330));
        Assert.AreEqual(new double[] {350, 400, 450, 500}, i.Interpolate(5,   500));
    }
}
