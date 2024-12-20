using System;
using System.Collections.Generic;
using UnityEngine;

public class WaterParticles : MonoBehaviour
{
    public static Action<Vector3[]> onWaterCollided;

    private void OnParticleCollision(GameObject other)
    {
        ParticleSystem ps = GetComponent<ParticleSystem>();

        List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();

        int collisionAmount = ps.GetCollisionEvents(other, collisionEvents);

        Vector3[] collisionPosition = new Vector3[collisionAmount];

        for (int i = 0; i < collisionAmount; i++)
        {
            collisionPosition[i] = collisionEvents[i].intersection;
        }

        onWaterCollided?.Invoke(collisionPosition);
    }
}
