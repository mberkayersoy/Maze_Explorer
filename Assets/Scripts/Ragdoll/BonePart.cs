using System;
using UnityEngine;

namespace _Game.Extensions.Ragdoll
{
    [Serializable]
    public class BonePart
    {
        public BoneName BoneName;
        public Transform Transform;
        public Rigidbody Rigidbody;
        public Collider Collider;
        [HideInInspector] public GameObject GameObject;
        [HideInInspector] public Joint Joint;
    }
}