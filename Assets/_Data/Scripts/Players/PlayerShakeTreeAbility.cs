using System;
using UnityEngine;

public class PlayerShakeTreeAbility : MonoBehaviour
{
    [SerializeField] private float distanceToTree;
    [SerializeField] private float shakeThreshold = 0.01f;

    private AppleTree currentTree;
    private PlayerAnimator playerAnimator;
    private Vector2 previousMousePosition;
    private bool isActive;
    private bool isShaking;


    private void Awake()
    {
        AppleTreeManager.onTreeModeStarted += TreeModeStartedCallback;
        AppleTreeManager.onTreeModeEnded += TreeModeEndedCallback;
        playerAnimator = GetComponent<PlayerAnimator>();
    }

    private void Update()
    {
        if (isActive && !isShaking) ManageTreeShaking();
    }

    private void OnDestroy()
    {
        AppleTreeManager.onTreeModeStarted -= TreeModeStartedCallback;
        AppleTreeManager.onTreeModeEnded -= TreeModeEndedCallback;
    }


    private void TreeModeStartedCallback(AppleTree tree)
    {
        currentTree = tree;
        isActive = true;
        MoveTowardsTree();
    }

    private void TreeModeEndedCallback()
    {
        currentTree = null;

        isActive = false;
        isShaking = false;
        
        LeanTween.delayedCall(.1f, () => playerAnimator.StopShakeTreeAnimation());
    }

    private void MoveTowardsTree()
    {
        Vector3 treePos = currentTree.transform.position;
        Vector3 dir = transform.position - treePos;

        Vector3 flatDir = dir;
        flatDir.y = 0;

        Vector3 targetPosition = treePos + flatDir.normalized * distanceToTree;
        playerAnimator.ManageAnimations(-flatDir);

        LeanTween.move(gameObject, targetPosition, .5f);
    }

    private void ManageTreeShaking()
    {
        if (!Input.GetMouseButton(0))
        {
            currentTree.StopShaking();
            return;
        }

        float shakeMagnitude = Vector2.Distance(Input.mousePosition, previousMousePosition);

        if (ShouldShake(shakeMagnitude))
        {
            Shake();
        }
        else
        {
            currentTree.StopShaking();
        }

        previousMousePosition = Input.mousePosition;
    }

    private bool ShouldShake(float shakeMagnitude)
    {
        float screenPercent = shakeMagnitude / Screen.width;
        return screenPercent >= shakeThreshold;
    }

    private void Shake()
    {
        isShaking = true;
        playerAnimator.PlayShakeTreeAnimation();
        currentTree.Shake();
        LeanTween.delayedCall(.1f, () => isShaking = false);
    }
}
