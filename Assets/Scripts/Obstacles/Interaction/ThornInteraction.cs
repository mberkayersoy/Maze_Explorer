using UnityEngine;

public class ThornInteraction : MonoBehaviour, IObstacleInteraction
{
    private float pushForce = 2f;
    public float PushForce { get => pushForce; }

}
