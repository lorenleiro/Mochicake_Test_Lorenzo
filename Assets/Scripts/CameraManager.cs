using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField]
    private CinemachineVirtualCamera gameCamera;

    [SerializeField]
    private CinemachineVirtualCamera menuCamera;

    public void TransitionGameCamera()
    {
        menuCamera.Priority = 0;
        gameCamera.Priority = 10;
    }
}
