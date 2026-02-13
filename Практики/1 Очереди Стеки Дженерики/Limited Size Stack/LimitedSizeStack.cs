using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;

namespace LimitedSizeStack;

public class LimitedSizeStack<T>
{
	LinkedList<T> stack;
	int limit;

	public LimitedSizeStack(int undoLimit)
	{
		if (limit < 0) throw new ArgumentException("limit cant be negative");

		limit = undoLimit;
		stack = new LinkedList<T>();
	}

	public void Push(T item)
	{
		if (limit == 0) return;

		if (stack.Count == limit)
			stack.RemoveLast();

		stack.AddFirst(item);
	}

	public T Pop()
	{
		if (stack.Count == 0)
			throw new InvalidOperationException("stack is empty");

		var popElement = stack.First.Value;
		stack.RemoveFirst();

		return popElement;
	}

	public int Count => stack.Count;
}