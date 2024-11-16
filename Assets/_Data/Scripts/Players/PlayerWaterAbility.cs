using UnityEngine;

public class PlayerWaterAbility : MonoBehaviour
{
    private PlayerAnimator playerAnimator;
    private CropField currentCropField;
    private PlayerToolSelector playerToolSelector;

    private void Awake()
    {
        playerAnimator = GetComponent<PlayerAnimator>();
        playerToolSelector = GetComponent<PlayerToolSelector>();

        WaterParticles.onWaterCollided += WaterCollisionCallback;
        CropField.onFullyWatered += CropFieldFullyWatered;
        playerToolSelector.onToolSelected += ToolSelectedCallback;
    }

    private void ToolSelectedCallback(PlayerToolSelector.Tool tool)
    {
        if (!playerToolSelector.CanWater())
        {
            playerAnimator.StopSowAnimation();
        }
    }

    private void OnDestroy()
    {
        WaterParticles.onWaterCollided -= WaterCollisionCallback;
        CropField.onFullyWatered -= CropFieldFullyWatered;

        playerToolSelector.onToolSelected -= ToolSelectedCallback;
    }

    private void CropFieldFullyWatered(CropField field)
    {
        if (currentCropField == field)
        {
            playerAnimator.StopWaterAnimation();
        }
    }

    private void WaterCollisionCallback(Vector3[] waterPositions)
    {
        if (currentCropField == null) return;
        currentCropField.WaterCollisionCallback(waterPositions);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CropField") && other.GetComponent<CropField>().IsSown())
        {
            currentCropField = other.GetComponent<CropField>();
            EnteredCropField(currentCropField);
        }
    }

    private void EnteredCropField(CropField cropField)
    {
        if (playerToolSelector.CanWater())
        {
            if (currentCropField == null)
            {
                currentCropField = cropField;
            }
            playerAnimator.PlayWaterAnimation();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("CropField") && other.GetComponent<CropField>().IsSown())
        {
            EnteredCropField(other.GetComponent<CropField>());
        }


    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("CropField"))
        {
            playerAnimator.StopWaterAnimation();
            currentCropField = null;
        }
    }
}
