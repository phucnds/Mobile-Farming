using System.Collections;
using UnityEngine;

public class TransactionEffectManager : MonoBehaviour
{
    [SerializeField] private ParticleSystem coinPS;
    [SerializeField] private RectTransform coinRectTransform;
    [SerializeField] private float moveSpeed = 5f;

    public static TransactionEffectManager Instance;

    private int coinAmount;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    [NaughtyAttributes.Button]
    private void PlayTEST()
    {
        PlayCoinParticlesystem(100);
    }

    public void PlayCoinParticlesystem(int amount)
    {
        if (coinPS.isPlaying) return;
        ParticleSystem.Burst burst = coinPS.emission.GetBurst(0);
        burst.count = amount;
        coinPS.emission.SetBurst(0, burst);
        ParticleSystem.MainModule main = coinPS.main;
        main.gravityModifier = 2f;
        coinPS.Play();
        coinAmount = amount;
        StartCoroutine(PlayCoinParticlesystemCoroutine());
    }

    private IEnumerator PlayCoinParticlesystemCoroutine()
    {
        yield return new WaitForSeconds(1f);

        ParticleSystem.MainModule main = coinPS.main;
        main.gravityModifier = 0;

        ParticleSystem.Particle[] particles = new ParticleSystem.Particle[coinAmount];
        coinPS.GetParticles(particles);

        Vector3 direction = (coinRectTransform.position - Camera.main.transform.position).normalized;


        while (coinPS.isPlaying)
        {
            coinPS.GetParticles(particles);
            Vector3 targetPosition = Camera.main.transform.position + direction * Vector3.Distance(Camera.main.transform.position, coinPS.transform.position);
            for (int i = 0; i < particles.Length; i++)
            {
                if (particles[i].remainingLifetime <= 0) continue;

                particles[i].position = Vector3.MoveTowards(particles[i].position, targetPosition, moveSpeed * Time.deltaTime);
                if (Vector3.Distance(particles[i].position, targetPosition) < 0.01f)
                {
                    particles[i].position += Vector3.up * 100 * 100;
                    CashManager.Instance.AddCoins(1);
                }
            }

            coinPS.SetParticles(particles);

            yield return null;
        }


    }
}
