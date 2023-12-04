using UnityEngine;
using UnityEngine.UI;

public class EndUI : MonoBehaviour
{
    [SerializeField] private Button quitGameButton;

    private void Start()
    {
        quitGameButton.onClick.AddListener(QuitGame);
        Cursor.lockState = CursorLockMode.None;
    }

    private void QuitGame()
    {
        Application.Quit();
    }
}
