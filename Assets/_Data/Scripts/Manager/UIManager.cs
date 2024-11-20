using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private GameObject treeModePanel;

    [SerializeField] private GameObject treeButton;
    [SerializeField] private GameObject toolButtonContainer;

    private void Awake()
    {
        PlayerDetection.onEnteredTreeZone += EnteredTreeZoneCallback;
        PlayerDetection.onExitedTreeZone += ExitedTreeZoneCallback;
        AppleTreeManager.onTreeModeStarted += SetTreeMode;
        AppleTreeManager.onTreeModeEnded += SetGameMode;
    }

    private void Start()
    {
        treeButton.SetActive(false);
        toolButtonContainer.SetActive(true);
        SetGameMode();
    }

    private void OnDestroy()
    {
        PlayerDetection.onEnteredTreeZone -= EnteredTreeZoneCallback;
        PlayerDetection.onExitedTreeZone -= ExitedTreeZoneCallback;
        AppleTreeManager.onTreeModeStarted -= SetTreeMode;
        AppleTreeManager.onTreeModeEnded -= SetGameMode;
    }

    private void EnteredTreeZoneCallback(AppleTree tree)
    {
        treeButton.SetActive(true);
        toolButtonContainer.SetActive(false);
    }

    private void ExitedTreeZoneCallback(AppleTree tree)
    {
        treeButton.SetActive(false);
        toolButtonContainer.SetActive(true);
    }

    private void SetGameMode()
    {
        treeModePanel.SetActive(false);
        gamePanel.SetActive(true);
    }

    private void SetTreeMode(AppleTree appleTree)
    {
        treeModePanel.SetActive(true);
        gamePanel.SetActive(false);
    }
}
