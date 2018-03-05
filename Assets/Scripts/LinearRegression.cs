using UnityEngine;


public class LinearRegression
{
    static public Vector2 Fit(double[] x, double[] y)
    {
        int numPoints = x.Length;

        double meanX = 0;
        double meanY = 0;
        double sumXSquared = 0;
        double sumXY = 0;

        for (int i = 0; i < numPoints; i++)
        {
            double xi = x[i];
            double yi = y[i];
            meanX += xi;
            meanY += yi;
            sumXSquared += xi * xi;
            sumXY += xi * yi;
        }

        meanX /= numPoints;
        meanY /= numPoints;

        // Slope
        double m = (sumXY / numPoints - meanX * meanY) / (sumXSquared / numPoints - meanX * meanX);

        // Intercept
        double n = (m * meanX - meanY);

        return new Vector2((float) m, (float) n);
    }
}
