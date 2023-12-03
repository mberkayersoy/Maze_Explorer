using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WreckingBallInteraction : MonoBehaviour
{
    [SerializeField] private float forceMagnitude;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Get the collision point
            ContactPoint contactPoint = collision.contacts[0];

            // Get the normal vector at the collision point
            Vector3 normal = contactPoint.normal;

            // Check that player has rigidbody
            if (collision.gameObject.TryGetComponent(out Rigidbody playerRb))
            {
                playerRb.AddForce(normal * forceMagnitude, ForceMode.Impulse);
                EventBus.Publish(new OnPlayerDeadEvent(true));
            }
        }
    }
}
