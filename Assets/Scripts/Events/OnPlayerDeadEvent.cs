public class OnPlayerDeadEvent 
{
    public bool IsDead { get; private set; }
    public OnPlayerDeadEvent(bool isDead)
    {
        IsDead = isDead;
    }
}
