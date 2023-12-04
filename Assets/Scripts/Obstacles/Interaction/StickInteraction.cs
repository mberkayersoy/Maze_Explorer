using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickInteraction : MonoBehaviour, IObstacleInteraction
{
    private float pushForce = 15f;
    public float PushForce { get => pushForce; }

}
