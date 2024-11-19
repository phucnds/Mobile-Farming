using UnityEngine;

public class PlayerHarvestAbility : MonoBehaviour
{
    [SerializeField] private Transform harvestSphere;

    private PlayerAnimator playerAnimator;
    private CropField currentCropField;
    private PlayerToolSelector playerToolSelector;

    private bool canHarvest;

    private void Awake()
    {
        playerAnimator = GetComponent<PlayerAnimator>();
        playerToolSelector = GetComponent<PlayerToolSelector>();

        // WaterParticles.onWaterCollided += WaterCollisionCallback;
        CropField.onFullyHarvested += CropFieldFullyHarvested;
        playerToolSelector.onToolSelected += ToolSelectedCallback;
    }

    private void OnDestroy()
    {
        // WaterParticles.onWaterCollided -= WaterCollisionCallback;
        CropField.onFullyHarvested -= CropFieldFullyHarvested;
        playerToolSelector.onToolSelected -= ToolSelectedCallback;
    }

    private void ToolSelectedCallback(PlayerToolSelector.Tool tool)
    {
        if (!playerToolSelector.CanHarvest())
        {
            playerAnimator.StopHarvestAnimation();
            canHarvest = false;
        }
    }

    private void CropFieldFullyHarvested(CropField field)
    {
        if (currentCropField == field)
        {
            playerAnimator.StopHarvestAnimation();
            canHarvest = false;
        }
    }

    private void WaterCollisionCallback(Vector3[] waterPositions)
    {
        if (currentCropField == null) return;
        currentCropField.WaterCollisionCallback(waterPositions);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CropField") && other.GetComponent<CropField>().IsWatered())
        {
            currentCropField = other.GetComponent<CropField>();
            EnteredCropField(currentCropField);
        }
    }

    private void EnteredCropField(CropField cropField)
    {
        if (playerToolSelector.CanHarvest())
        {
            if (currentCropField == null)
            {
                currentCropField = cropField;
            }

            playerAnimator.PlayHarvestAnimation();

            if(canHarvest)
            {
                currentCropField.Harvest(harvestSphere);
            }

            // harvestSphere.GetComponent<MeshRenderer>().enabled = canHarvest;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("CropField") && other.GetComponent<CropField>().IsWatered())
        {
            EnteredCropField(other.GetComponent<CropField>());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("CropField"))
        {
            playerAnimator.StopHarvestAnimation();
            currentCropField = null;
            canHarvest = false;
        }
    }

    public void HarvestingStartedCallback()
    {
        canHarvest = true;
    }

    public void HarvestingStoppedCallback()
    {
        canHarvest = false;
    }
}
