using UnityEngine;

public class GoalInteraction : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            EventBus.Publish(new OnGoalReachedEvent());
        }
    }
}
