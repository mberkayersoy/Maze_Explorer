using UnityEngine;

public class WreckingBallInteraction : MonoBehaviour, IObstacleInteraction
{
    private float pushForce = 10f;
    public float PushForce { get => pushForce; }

}
