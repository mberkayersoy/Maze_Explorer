using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform startPoint;
    private void Start()
    {
        EventBus.Subscribe<OnGameStartedEvent>(ActivatePlayer);
        EventBus.Subscribe<OnPlayerReviveEvent>(ReSpawnPlayer);
    }

    private void ActivatePlayer(OnGameStartedEvent @event)
    {
        player.gameObject.SetActive(true);
    }

    private void ReSpawnPlayer(OnPlayerReviveEvent @event)
    {

        player.transform.position = startPoint.position;
    }

    private void OnDestroy()
    {
        EventBus.Unsubscribe<OnGameStartedEvent>(ActivatePlayer);
        EventBus.Unsubscribe<OnPlayerReviveEvent>(ReSpawnPlayer);
    }
}
