using UnityEngine;
public class OnMouseDeltaEvent
{
    public Vector2 Delta { get; private set; }

    public OnMouseDeltaEvent(Vector2 delta)
    {
        Delta = delta;
    }
}
