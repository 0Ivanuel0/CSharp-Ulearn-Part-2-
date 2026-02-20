using System.Collections.Generic;

namespace yield;

public static class ExpSmoothingTask
{
	public static IEnumerable<DataPoint> SmoothExponentialy(this IEnumerable<DataPoint> data, double alpha)
	{
		var previousY = double.NaN;

		foreach (var point in data)
		{
			if (double.IsNaN(previousY))
			{
				previousY = point.OriginalY;
				yield return point.WithExpSmoothedY(previousY);
			}
			else
			{
				var currentY = point.OriginalY;
				var smoothedY = (alpha * currentY) + ((1 - alpha) * previousY);
				yield return point.WithExpSmoothedY(smoothedY);

				previousY = smoothedY;
			}
        }
	}
}
