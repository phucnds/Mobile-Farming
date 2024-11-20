using System;
using UnityEngine;
using UnityEngine.UI;

public class AppleTreeManager : MonoBehaviour
{

    [SerializeField] private Slider shakeSlider;

    private AppleTree lastTriggerTree;

    public static Action<AppleTree> onTreeModeStarted;
    public static Action onTreeModeEnded;

    private void Awake()
    {
        PlayerDetection.onEnteredTreeZone += EnteredTreeZoneCallback;
    }

    private void OnDestroy()
    {
        PlayerDetection.onEnteredTreeZone -= EnteredTreeZoneCallback;
    }

    private void EnteredTreeZoneCallback(AppleTree tree)
    {
        lastTriggerTree = tree;
    }

    public void TreeButtonCallback()
    {
        if (!lastTriggerTree.IsReady()) return;

        StartTreeMode();
    }

    private void StartTreeMode()
    {
        lastTriggerTree.Intialize(this);
        onTreeModeStarted?.Invoke(lastTriggerTree);
        UpdateShakeSlider(0);
    }

    public void UpdateShakeSlider(float value)
    {
        shakeSlider.value = value;
    }

    public void ExitTreeMode()
    {
        onTreeModeEnded?.Invoke();
    }
}
