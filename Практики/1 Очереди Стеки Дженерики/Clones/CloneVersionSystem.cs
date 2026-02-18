using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace Clones;

public class StackItem<T>
{
	public T Value { get; set; }
	public StackItem<T> Previous {  get; set; }

	public StackItem(T value, StackItem<T> previous)
    {
        Value = value;
        Previous = previous;
    }
}

public class LinkedStack<T>
{
	StackItem<T> head;
	private int count;

    public LinkedStack()
    {
        head = null;
        count = 0;
    }

    private LinkedStack(StackItem<T> head, int count)
	{
		this.head = head;
		this.count = count;
	}

	public LinkedStack<T> Push(T value)
	{
		var newHead = new StackItem<T>(value, head);
		return new LinkedStack<T>(newHead, count + 1);
	}

	public LinkedStack<T> Pop(out T value)
	{
		if (count == 0) 
			throw new System.InvalidOperationException("stack is empty");

        value = head.Value;
		return new LinkedStack<T>(head.Previous, count - 1);
	}

	public T Peek()
	{
		if (count == 0)
			throw new System.InvalidOperationException("stack is empty");
		return head.Value;
	}

	public int Count => count;
}


public class Clone
{
	LinkedStack<string> learnedCommands;
    LinkedStack<string> undoCommands;

	public Clone()
	{
		learnedCommands = new LinkedStack<string>();
		undoCommands = new LinkedStack<string>();
	}

	private Clone(LinkedStack<string> learnedStack, LinkedStack<string> undoStack)
	{
		learnedCommands = learnedStack;
		undoCommands = undoStack;
	}

	public void LearnCommand(string programm)
	{
		learnedCommands = learnedCommands.Push(programm);
		undoCommands = new LinkedStack<string>();
	}

	public void RollBackCommand()
	{
		if (learnedCommands.Count == 0)
			return;

		learnedCommands = learnedCommands.Pop(out string deleteCommand);
		undoCommands = undoCommands.Push(deleteCommand);
	}

	public void RelearnCommand()
	{
		if (undoCommands.Count == 0)
			return;

		undoCommands = undoCommands.Pop(out string programm);
        learnedCommands = learnedCommands.Push(programm);
    }

	public Clone CloneCommand()
	{
		return new Clone(learnedCommands, undoCommands);
	}

	public string CheckCommand()
	{
        if (learnedCommands.Count == 0)
            return "basic";
		return learnedCommands.Peek();
	}
}

public class CloneVersionSystem : ICloneVersionSystem
{
	private List<Clone> CloneList;

	public CloneVersionSystem()
	{
		CloneList = new List<Clone>();
		CloneList.Add(new Clone());
	}

	public string Execute(string query)
	{
		var parts = query.Split(' ').ToArray();
		var command = parts[0];
		var cloneNumber = int.Parse(parts[1]);	
		var currentClone = CloneList[cloneNumber - 1];

		switch (command)
		{
			case "learn":
				var programm = parts[2];
                currentClone.LearnCommand(programm);
				return null;

			case "rollback":
				currentClone.RollBackCommand();
				return null;

			case "relearn":
				currentClone.RelearnCommand();
				return null;

			case "clone":
				var newClone = currentClone.CloneCommand();
				CloneList.Add(newClone);
				return null;

			case "check":
				var lastLearnedProgramm = currentClone.CheckCommand();
				return lastLearnedProgramm;

			default: return null;
		}
	}
}