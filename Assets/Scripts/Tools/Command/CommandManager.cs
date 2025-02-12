using System.Collections.Generic;

public class CommandManager
{
    private Stack<ICommand> executedCommands = new();

    public bool ExecuteCommand(ICommand command)
    {
        if(command == null) return false;

        var executed = command.Execute();

        if(!executed) return false;

        if(command.Undoable) executedCommands.Push(command);

        return true;
    }

    public bool UndoLastCommand()
    {
        if(executedCommands.Count == 0) return false;

        executedCommands.Pop().Undo();
        return true;
    }

    public bool RemoveLastCommand()
    {
        if (executedCommands.Count == 0) return false;

        executedCommands.Pop();
        return true;
    }

    public void ClearCommands()
    {
        executedCommands.Clear();
    }

    public bool CanUndo() => executedCommands != null && executedCommands.Count > 0;
}