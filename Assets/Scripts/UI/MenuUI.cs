using UnityEngine;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour
{

    [SerializeField] private Button playButton;
    [SerializeField] private Button quitButton;


    private void Start()
    {
        playButton.onClick.AddListener(StartGame);
        quitButton.onClick.AddListener(QuitGame);
    }

    private void StartGame()
    {
        EventBus.Publish(new OnGameStartedEvent());
    }
    private void QuitGame()
    {
        Application.Quit();
    }

}
