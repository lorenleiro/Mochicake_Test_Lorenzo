using Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField]
    private CinemachineVirtualCamera gameCamera;

    [SerializeField]
    private CinemachineVirtualCamera menuCamera;

    /// <summary>
    /// Transitions to the game camera.
    /// </summary>
    public void TransitionGameCamera()
    {
        menuCamera.Priority = 0;
        gameCamera.Priority = 10;
    }
}
