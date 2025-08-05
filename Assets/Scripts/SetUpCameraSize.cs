using System.Collections.Generic;
using UnityEngine;

public class SetUpCameraSize : MonoBehaviour
{
    public static SetUpCameraSize Instance;
    [SerializeField] private float desiredCameraWidth; 
    [SerializeField] private float targetAspectRatio = 9f / 16f; 
    [SerializeField] private Camera mainCamera;
    [SerializeField] float anchorOthoSize;
    [SerializeField] public float othorSize;
    [SerializeField] public float curDesiredCameraWidth;

    private void Awake()
    {
        Instance = this;
        mainCamera = Camera.main;
        othorSize = mainCamera.orthographicSize;
        curDesiredCameraWidth = desiredCameraWidth / 2;
        anchorOthoSize = othorSize;
        SetCameraOrthoSizeMapNormal();
    }

    public void SetDesiredCamWidth(float value)
    {
        desiredCameraWidth = value;
    }

    public void SetCameraOrthoSizeMapNormal()
    {
        float currentAspectRatio = (float)Screen.width / Screen.height;
        if (currentAspectRatio < targetAspectRatio)           
        {
            var orthoSize = desiredCameraWidth / currentAspectRatio / 2;
            mainCamera.orthographicSize = orthoSize;
            othorSize = mainCamera.orthographicSize;
            anchorOthoSize = mainCamera.orthographicSize;
            currentAspectRatio = (float)Screen.width / Screen.height;
            curDesiredCameraWidth = mainCamera.orthographicSize * currentAspectRatio;
        }
    }

    public void SetCameraOrthoSizeMapHard()
    {
        float currentAspectRatio = (float)Screen.width / Screen.height;
        if (currentAspectRatio > targetAspectRatio)         
        {
            var orthoSize = desiredCameraWidth / currentAspectRatio / 2;
            mainCamera.orthographicSize = orthoSize;
            othorSize = orthoSize;
            Debug.LogError("Width");
        }
    }

    public void SetCameraSize(float sizeXLevel, float sizeYLevel)
    {
        if (sizeXLevel > sizeYLevel - 1)
        {
            float currentAspectRatio = (float)Screen.width / Screen.height;
            var orthoSize = (sizeXLevel + 1) * 2 / currentAspectRatio / 2;
            mainCamera.orthographicSize = orthoSize;
            Debug.LogError("Width");
        }
        else
        {
            var orthoSize = sizeYLevel + 4.5f;
            mainCamera.orthographicSize = orthoSize;
            Debug.LogError("Height");
        }
    }
}