using DG.Tweening;
using UnityEngine;

public class StickMovement : MonoBehaviour
{
    [SerializeField] private float distance = 2f;
    [SerializeField] private float duration = 5f;

    void Start()
    {
        MoveUpDownLoop();
    }

    void MoveUpDownLoop()
    {
        transform.DOLocalMoveY(transform.localPosition.y + distance, duration)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                transform.DOLocalMoveY(transform.localPosition.y - distance, duration)
                    .SetEase(Ease.Linear)
                    .OnComplete(() => MoveUpDownLoop());
            });
    }

    private void OnDestroy()
    {
        DOTween.Kill(this);
    }
}