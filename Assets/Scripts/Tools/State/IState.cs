
public interface IState
{
    public void Enter(IState previousState);
    public void Exit(IState nextState);
    public void Update();
    public void FixedUpdate();
}
