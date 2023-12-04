using _Game.Extensions.Ragdoll;
using System.Collections;
using UnityEngine;

public class PlayerDeadHandler : MonoBehaviour
{
    private RagdollController ragdollController;

    private void Awake()
    {
        ragdollController = GetComponent<RagdollController>();
    }

    private void Start()
    {
        EventBus.Subscribe<OnPlayerReviveEvent>(RevivePlayer);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out IObstacleInteraction obstacle))
        {
            // Get the collision point
            ContactPoint contactPoint = collision.contacts[0];

            Vector3 normal = contactPoint.normal;

            EventBus.Publish(new OnPlayerDeadDirectionEvent(normal));
            KillPlayer(normal * obstacle.PushForce);
        }
    }

    IEnumerator RevivePlayer()
    {
        // To ensure that the player does not become active before returning to the starting point.
        yield return new WaitForSeconds(0.5f);

        EventBus.Publish(new OnPlayerDeadEvent(false));
        ragdollController.SetRagdoll(false);
    }
    private void RevivePlayer(OnPlayerReviveEvent @event)
    {
        StartCoroutine(RevivePlayer());
    }

    private void KillPlayer(Vector2 direction)
    {
        EventBus.Publish(new OnPlayerDeadEvent(true));
        ragdollController.SetRagdoll(true);
        ragdollController.AddForceToAllParts(direction);
    }
}
