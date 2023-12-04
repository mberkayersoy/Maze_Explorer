using UnityEngine;

public class ThirdPersonCameraController : MonoBehaviour
{
    private PlayerInputHandler input;

    private const float _threshold = 0.01f;

    [SerializeField] private GameObject cameraTarget;
    [SerializeField] private float TopClamp = 70.0f;
    [SerializeField] private float BottomClamp = -30.0f;

    // cinemachine
    private float _cinemachineTargetYaw;
    private float _cinemachineTargetPitch;

    private void Awake()
    {
        input = GetComponent<PlayerInputHandler>();
    }

    private void Start()
    {
        _cinemachineTargetYaw = cameraTarget.transform.rotation.eulerAngles.y;
    }

    private void LateUpdate()
    {
        CameraRotation();
    }

    private void CameraRotation()
    {   
        // if there is an input and camera position is not fixed
        if (input.look.sqrMagnitude >= _threshold)
        {
            _cinemachineTargetYaw += input.look.x;
            _cinemachineTargetPitch += input.look.y;
        }

        // clamp our rotations so our values are limited 360 degrees
        _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
        _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);


        // Cinemachine will follow this target
        cameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch,
            _cinemachineTargetYaw, 0.0f);
    }

    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }
}
