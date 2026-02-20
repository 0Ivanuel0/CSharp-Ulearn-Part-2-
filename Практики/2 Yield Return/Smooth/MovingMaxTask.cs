using System;
using System.Collections.Generic;
using System.Linq;

namespace yield;

public static class MovingMaxTask
{
	public static IEnumerable<DataPoint> MovingMax(this IEnumerable<DataPoint> data, int windowWidth)
	{
        var queue = new Queue<double>();
        var possibleMaxList = new LinkedList<double>();

        foreach (var point in data)
        {
            var currentY = point.OriginalY;
            queue.Enqueue(currentY);

            while (possibleMaxList.Count > 0 && possibleMaxList.Last.Value < currentY)
                possibleMaxList.RemoveLast();

            possibleMaxList.AddLast(currentY);

            if (queue.Count > windowWidth && possibleMaxList.First.Value == queue.Dequeue())
                possibleMaxList.RemoveFirst();

            yield return point.WithMaxY(possibleMaxList.First.Value);
        }
    }
}