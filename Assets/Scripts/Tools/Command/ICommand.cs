
public interface ICommand
{
    public bool Undoable { get; }
    public void Execute();
    public void Undo();
}