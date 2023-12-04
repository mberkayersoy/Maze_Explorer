using UnityEngine;

public class OnPlayerDeadDirectionEvent 
{
    public Vector2 ForceDirection { get; private set; }
    public OnPlayerDeadDirectionEvent(Vector2 forceDirection)
    {
        ForceDirection = forceDirection;
    }
}
