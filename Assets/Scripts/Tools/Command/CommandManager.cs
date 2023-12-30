using System.Collections.Generic;

public static class CommandManager
{
    private static Stack<ICommand> executedCommands = new();

    public static void ExecuteCommand(ICommand command)
    {
        command.Execute();

        if(command.Undoable) executedCommands.Push(command);
    }

    public static bool UndoLastCommand()
    {
        if(executedCommands.Count == 0) return false;

        executedCommands.Pop().Undo();
        return true;
    }

    public static bool RemoveLastCommand()
    {
        if (executedCommands.Count == 0) return false;

        executedCommands.Pop();
        return true;
    }

    public static void ClearCommands()
    {
        executedCommands.Clear();
    }
}