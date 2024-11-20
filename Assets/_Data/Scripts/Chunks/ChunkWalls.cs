using System;
using UnityEngine;

public class ChunkWalls : MonoBehaviour
{
    [SerializeField] private GameObject frontWall;
    [SerializeField] private GameObject rightWall;
    [SerializeField] private GameObject backWall;
    [SerializeField] private GameObject leftWall;

    public void Configure(int configuration)
    {
        frontWall.SetActive(IsKthBitset(configuration, 0));
        rightWall.SetActive(IsKthBitset(configuration, 1));
        backWall.SetActive(IsKthBitset(configuration, 2));
        leftWall.SetActive(IsKthBitset(configuration, 3));
    }

    private bool IsKthBitset(int configuration, int k)
    {
        if ((configuration & (1 << k)) > 0) return false;
        else
            return true;
    }
}
