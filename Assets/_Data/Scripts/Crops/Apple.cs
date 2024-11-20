using UnityEngine;

public class Apple : MonoBehaviour
{
    private enum State { Ready, Growing }

    private State state;

    [SerializeField] private Renderer renderer;
    [SerializeField] private float shakeMultiplier;

    private Rigidbody rb;
    private Vector3 initialPos;
    private Quaternion initialRot;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        initialPos = transform.position;
        initialRot = transform.rotation;

        state = State.Ready;
    }

    public void Shake(float magnitude)
    {
        float realShakeMagnitude = magnitude * shakeMultiplier;

        renderer.material.SetFloat("_Magnitude", realShakeMagnitude);
    }

    public void Release()
    {
        rb.isKinematic = false;

        state = State.Growing;

        renderer.material.SetFloat("_Magnitude", 0);
    }

    public bool IsFree()
    {
        return !rb.isKinematic;
    }

    public void Reset()
    {
        LeanTween.scale(gameObject, Vector3.zero, 1).setDelay(2).setOnComplete(ForceReset);
    }

    public bool IsReady()
    {
        return state == State.Ready;
    }

    private void ForceReset()
    {
        transform.position = initialPos;
        transform.rotation = initialRot;

        rb.isKinematic = true;

        float randomScaleTime = Random.Range(5f, 10f);
        LeanTween.scale(gameObject, Vector3.one, randomScaleTime).setOnComplete(SetReady);

    }

    private void SetReady()
    {
        state = State.Ready;
    }
}
