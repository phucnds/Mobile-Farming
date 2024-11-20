using System;
using UnityEngine;

public class AppleTree : MonoBehaviour
{
    [SerializeField] private GameObject treeCam;
    [SerializeField] private Renderer renderer;
    [SerializeField] private float maxShakeMagnitude;
    [SerializeField] private float shakeIncrement;
    [SerializeField] private Transform appleParent;

    private float shakeMagnitude;
    private bool isShaking;
    private float shakeSliderValue;
    private AppleTreeManager appleTreeManager;

    public static Action<CropType> onAppHarvested;

    public void EnableTreeCam()
    {
        treeCam.SetActive(true);
    }

    public void DisableTreeCam()
    {
        treeCam.SetActive(false);
    }

    public void Shake()
    {
        isShaking = true;
        TweenShake(maxShakeMagnitude);
        UpdateShakeSlider();
    }

    public void StopShaking()
    {
        if (!isShaking) return;

        isShaking = false;

        TweenShake(0);
    }

    private void TweenShake(float targetMagnitude)
    {
        LeanTween.cancel(renderer.gameObject);
        LeanTween.value(renderer.gameObject, UpdateShakeMagnitude, shakeMagnitude, targetMagnitude, 1);
    }

    private void UpdateShakeMagnitude(float value)
    {
        shakeMagnitude = value;
        UpdateMaterials();
    }

    public void Intialize(AppleTreeManager appleTreeMgr)
    {
        EnableTreeCam();
        appleTreeManager = appleTreeMgr;
    }

    private void UpdateShakeSlider()
    {
        shakeSliderValue += shakeIncrement;
        appleTreeManager.UpdateShakeSlider(shakeSliderValue);

        for (int i = 0; i < appleParent.childCount; i++)
        {
            float applePercent = (float)i / appleParent.childCount;

            Apple currentApple = appleParent.GetChild(i).GetComponent<Apple>();

            if (shakeSliderValue > applePercent && !currentApple.IsFree())
            {
                ReleaseApple(currentApple);
            }
        }

        if (shakeSliderValue >= 1)
        {
            ExitTreeMode();
        }
    }

    private void ExitTreeMode()
    {
        appleTreeManager.ExitTreeMode();

        DisableTreeCam();

        TweenShake(0);

        ResetApple();

        shakeSliderValue = 0;
    }

    private void ResetApple()
    {
        for (int i = 0; i < appleParent.childCount; i++)
        {
            appleParent.GetChild(i).GetComponent<Apple>().Reset();
        }
    }

    private void ReleaseApple(Apple apple)
    {
        apple.Release();
        onAppHarvested?.Invoke(CropType.Apple);
    }

    private void UpdateMaterials()
    {
        renderer.material.SetFloat("_Magnitude", shakeMagnitude);

        foreach (Transform appleT in appleParent)
        {
            Apple apple = appleT.GetComponent<Apple>();

            if (apple.IsFree()) continue;
            apple.Shake(shakeMagnitude);
        }
    }

    public bool IsReady()
    {
        for (int i = 0; i < appleParent.childCount; i++)
        {
            if (!appleParent.GetChild(i).GetComponent<Apple>().IsReady()) return false;
        }
        return true;
    }
}
