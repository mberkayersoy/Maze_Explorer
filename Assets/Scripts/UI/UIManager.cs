using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Transform MenuUI;
    [SerializeField] private Transform GameUI;
    [SerializeField] private Transform EndUI;

    private void Awake()
    {
        SetActivePanel(MenuUI.name);
    }

    private void Start()
    {
        EventBus.Subscribe<OnGameStartedEvent>(SetGameUI);
        EventBus.Subscribe<OnGoalReachedEvent>(SetEndUI);
    }

    private void SetEndUI(OnGoalReachedEvent @event)
    {
        SetActivePanel(EndUI.name);
    }

    private void SetGameUI(OnGameStartedEvent @event)
    {
        SetActivePanel(GameUI.name);
    }

    private void SetActivePanel(string uiName)
    {
        MenuUI.gameObject.SetActive(uiName.Equals(MenuUI.name));
        GameUI.gameObject.SetActive(uiName.Equals(GameUI.name));
        EndUI.gameObject.SetActive(uiName.Equals(EndUI.name));
    }
}
