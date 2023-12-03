using UnityEngine;
using DG.Tweening;

public class WreckingBallMovement : MonoBehaviour
{
    [SerializeField] private float rotationDuration = 2.0f;
    [SerializeField] private Ease rotationEase = Ease.InOutSine;
    [SerializeField] private float rotationAngle;
    private void Start()
    {
        RotateWreckingBall();
    }

    private void RotateWreckingBall()
    {
        transform.DORotate(new Vector3(0f, 0f, rotationAngle), rotationDuration)
            .SetEase(rotationEase)
            .OnComplete(() =>
            {
                transform.DORotate(new Vector3(0f, 0f, -rotationAngle), rotationDuration)
                    .SetEase(rotationEase)
                    .OnComplete(() => RotateWreckingBall());
            });
    }

    private void OnDestroy()
    {
        DOTween.Kill(this);
    }
}
