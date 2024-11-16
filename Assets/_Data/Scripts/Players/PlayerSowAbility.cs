using System;
using UnityEngine;

public class PlayerSowAbility : MonoBehaviour
{
    private PlayerAnimator playerAnimator;
    private CropField currentCropField;
    private PlayerToolSelector playerToolSelector;

    private void Awake()
    {
        playerAnimator = GetComponent<PlayerAnimator>();
        playerToolSelector = GetComponent<PlayerToolSelector>();

        SeedParticles.onSeedCollided += SeedCollisionCallback;
        CropField.onFullySown += CropFieldFullySown;
        playerToolSelector.onToolSelected += ToolSelectedCallback;
    }

    private void ToolSelectedCallback(PlayerToolSelector.Tool tool)
    {
        if (!playerToolSelector.CanSow())
        {
            playerAnimator.StopSowAnimation();
        }
    }

    private void OnDestroy()
    {
        SeedParticles.onSeedCollided -= SeedCollisionCallback;
        CropField.onFullySown -= CropFieldFullySown;

        playerToolSelector.onToolSelected -= ToolSelectedCallback;
    }

    private void CropFieldFullySown(CropField field)
    {
        if (currentCropField == field)
        {
            playerAnimator.StopSowAnimation();
        }
    }

    private void SeedCollisionCallback(Vector3[] seedPositions)
    {
        if (currentCropField == null) return;
        currentCropField.SeedCollisionCallback(seedPositions);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CropField") && other.GetComponent<CropField>().IsEmpty())
        {
            currentCropField = other.GetComponent<CropField>();
            EnteredCropField(currentCropField);
        }
    }

    private void EnteredCropField(CropField currentCropField)
    {
        if (playerToolSelector.CanSow())
        {
            playerAnimator.PlaySowAnimation();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("CropField") && other.GetComponent<CropField>().IsEmpty())
        {
            EnteredCropField(other.GetComponent<CropField>());
        }


    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("CropField"))
        {
            playerAnimator.StopSowAnimation();
            currentCropField = null;
        }
    }
}
