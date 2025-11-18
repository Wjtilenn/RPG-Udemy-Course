public abstract class StateBase
{
    public abstract void Init(IStateMachineOwner owner);
    public abstract void Enter();
    public abstract void Exit();
    public abstract void Update();
    public abstract void Destroy();
}
