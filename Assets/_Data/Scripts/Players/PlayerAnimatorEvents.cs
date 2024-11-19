using UnityEngine;
using UnityEngine.Events;

public class PlayerAnimatorEvents : MonoBehaviour
{
    [SerializeField] private ParticleSystem seedParticle;
    [SerializeField] private ParticleSystem waterParticle;

    [SerializeField] private UnityEvent startHarvestingEvent;
    [SerializeField] private UnityEvent stoptHarvestingEvent;

    private void PlaySeedParticalSystem()
    {
        seedParticle.Play();
    }

    private void PlayWaterParticalSystem()
    {
        waterParticle.Play();
    }

    private void StartHarvestCallback()
    {
        startHarvestingEvent?.Invoke();
    }

    private void StopHarvestCallback()
    {
        stoptHarvestingEvent?.Invoke();
    }
}
