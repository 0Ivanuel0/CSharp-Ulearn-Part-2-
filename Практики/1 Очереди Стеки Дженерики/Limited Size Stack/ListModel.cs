using NUnit.Framework.Constraints;
using System;
using System.Collections.Generic;
using System.Data;

namespace LimitedSizeStack;

public class ListModel<TItem>
{
	public List<TItem> Items { get; }
	public int UndoLimit;

	LimitedSizeStack<Command> UndoStack;

	private struct Command
	{
		public readonly string Type;
		public readonly TItem Item;
		public readonly int Index;

		public Command(string type, TItem item, int index)
		{
            Type = type;
            Item = item;
            Index = index;
        }
	}

    public ListModel(int undoLimit) : this(new List<TItem>(), undoLimit)
	{
	}

	public ListModel(List<TItem> items, int undoLimit)
	{
		Items = items;
		UndoLimit = undoLimit;
		UndoStack = new LimitedSizeStack<Command>(undoLimit);
    }

	public void AddItem(TItem item)
	{
		UndoStack.Push(new Command("ADD", item, Items.Count - 1));
        Items.Add(item);
	}
	
	public void RemoveItem(int index)
	{
        UndoStack.Push(new Command("REMOVE", Items[index], index));
        Items.RemoveAt(index);
	}

	public bool CanUndo()
	{
		return UndoStack.Count > 0;
	}

	public void Undo()
	{
		if (CanUndo() is false) 
			throw new InvalidOperationException("Cancel cant be done");

		var currentCommand = UndoStack.Pop();

		if (currentCommand.Type == "ADD")
			Items.RemoveAt(Items.Count - 1);
		else Items.Insert(currentCommand.Index, currentCommand.Item);
	}
}