using System;
using UnityEngine;

public class PlayerDetection : MonoBehaviour
{
    public static Action<AppleTree> onEnteredTreeZone;
    public static Action<AppleTree> onExitedTreeZone;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("ChunkTrigger"))
        {
            Chunk chunk = other.GetComponentInParent<Chunk>();
            chunk.TryUnlock();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<AppleTree>(out AppleTree tree))
        {
            TriggererAppleTree(tree);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<AppleTree>(out AppleTree tree))
        {
            ExitedAppleTree(tree);
        }
    }

    private void TriggererAppleTree(AppleTree tree)
    {
        onEnteredTreeZone?.Invoke(tree);
    }

    private void ExitedAppleTree(AppleTree tree)
    {
        onExitedTreeZone?.Invoke(tree);
    }


}
