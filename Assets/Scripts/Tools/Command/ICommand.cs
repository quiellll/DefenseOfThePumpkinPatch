
public interface ICommand
{
    public bool Undoable { get; }
    public bool Execute();
    public void Undo();
}