using UnityEngine;

public class PlayerAnimatorEvents : MonoBehaviour
{
    [SerializeField] private ParticleSystem seedParticle;

    private void PlaySeedParticalSystem()
    {
        seedParticle.Play();
    }
}
