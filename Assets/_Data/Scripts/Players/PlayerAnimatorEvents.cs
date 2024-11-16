using UnityEngine;

public class PlayerAnimatorEvents : MonoBehaviour
{
    [SerializeField] private ParticleSystem seedParticle;
    [SerializeField] private ParticleSystem waterParticle;

    private void PlaySeedParticalSystem()
    {
        seedParticle.Play();
    }

    private void PlayWaterParticalSystem()
    {
        waterParticle.Play();
    }
}
