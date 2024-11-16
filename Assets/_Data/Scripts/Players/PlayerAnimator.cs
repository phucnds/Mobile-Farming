using System;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private float moveSpeedMultipler = 40f;



    public void ManageAnimations(Vector3 moveVector)
    {
        if (moveVector.magnitude > 0)
        {
            anim.SetFloat("moveSpeed", moveVector.magnitude * moveSpeedMultipler);

            PlayRunAnimation();

            anim.transform.forward = moveVector.normalized;
        }
        else
        {
            PlayIdleAnimation();
        }
    }

    private void PlayRunAnimation()
    {

        anim.Play("Run");
    }

    private void PlayIdleAnimation()
    {
        anim.Play("Idle");
    }

    public void PlaySowAnimation()
    {
        anim.SetLayerWeight(1, 1);
    }

    public void StopSowAnimation()
    {
        anim.SetLayerWeight(1, 0);
    }

    public void PlayWaterAnimation()
    {
        anim.SetLayerWeight(1, 1);
    }

    public void StopWaterAnimation()
    {
        anim.SetLayerWeight(1, 0);
    }
}
