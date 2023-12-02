using Cinemachine;
using System;
using UnityEngine;

public class CinemachinePOVExtension : CinemachineExtension
{

    [SerializeField] private float clampAngle = 80f;
    [SerializeField] private float horizontalSpeed = 10f;
    [SerializeField] private float verticalSpeed = 10f;

    private Vector3 startingRotation;
    private Vector2 mouseDelta;
    protected override void Awake ()
    {
        base.Awake ();
    }
    protected override void OnEnable()
    {
        if (startingRotation == null)
        {
            startingRotation = transform.localRotation.eulerAngles;
        }
        EventBus.Subscribe<OnMouseDeltaEvent>(HandleMouseDeltaEvent);

    }

    protected void OnDisable()
    {
        EventBus.Unsubscribe<OnMouseDeltaEvent>(HandleMouseDeltaEvent);
    }

    private void HandleMouseDeltaEvent(OnMouseDeltaEvent onMouseDeltaEvent)
    {
        mouseDelta = onMouseDeltaEvent.Delta;
    }

    protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        if (vcam.Follow)
        {
            if (stage == CinemachineCore.Stage.Aim)
            {
                startingRotation.x += mouseDelta.x *  verticalSpeed *Time.deltaTime;
                startingRotation.y += mouseDelta.y * horizontalSpeed * Time.deltaTime;

                startingRotation.y = Mathf.Clamp(startingRotation.y, -clampAngle, clampAngle);
                state.RawOrientation = Quaternion.Euler(startingRotation.y, startingRotation.x, 0f);
            }
        }   
    }

}
