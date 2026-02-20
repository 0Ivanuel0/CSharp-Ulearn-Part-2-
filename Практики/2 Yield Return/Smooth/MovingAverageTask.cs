using System.Collections.Generic;

namespace yield;

public static class MovingAverageTask
{
	public static IEnumerable<DataPoint> MovingAverage(this IEnumerable<DataPoint> data, int windowWidth)
	{
        var queue = new Queue<double>();
        var currentSum = 0d;

        foreach (var point in data)
        {
            queue.Enqueue(point.OriginalY);
            currentSum += point.OriginalY;

            if (queue.Count > windowWidth)
                currentSum -= queue.Dequeue();

            var avgY = queue.Count < windowWidth ? (currentSum / queue.Count) : (currentSum / windowWidth);
            yield return point.WithAvgSmoothedY(avgY);
        }
    }
}