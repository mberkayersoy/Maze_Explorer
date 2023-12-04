using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameUI : MonoBehaviour
{
    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeDuration;

    private Color originalColor;

    private void Start()
    {
        originalColor = fadeImage.color;
        EventBus.Subscribe<OnPlayerDeadDirectionEvent>(FadeAnimation);
    }
    private void FadeAnimation(OnPlayerDeadDirectionEvent playerDeadEvent)
    {
        fadeImage.DOColor(Color.black, fadeDuration).OnComplete(() =>
        {
            fadeImage.DOColor(originalColor, fadeDuration);
            EventBus.Publish(new OnPlayerReviveEvent());
        });

    }

    private void OnDestroy()
    {
        EventBus.Unsubscribe<OnPlayerDeadDirectionEvent>(FadeAnimation);
    }
}