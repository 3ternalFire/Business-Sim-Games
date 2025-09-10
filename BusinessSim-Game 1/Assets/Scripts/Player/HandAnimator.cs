using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandAnimator : MonoBehaviour
{
    public Animator animator;
    public float pokeDuration = 0.2f; // Duration of the poke animation

    public void PokeAtLane(int lane)
    {
        // Move hand quickly to tapped lane
        Vector3 targetPos = new Vector3(LaneToWorldX(lane), transform.position.y, 0);
        transform.position = targetPos;

        // Trigger poke animation
        animator.SetTrigger("PokeHand");

        // Start coroutine to reset hand to idle after animation
        StopAllCoroutines(); // Stop any previous poke reset
        StartCoroutine(ResetHandAfterPoke());
    }

    private IEnumerator ResetHandAfterPoke()
    {
        yield return new WaitForSeconds(pokeDuration);
        animator.SetTrigger("Idle");
    }

    float LaneToWorldX(int lane)
    {
        float section = Screen.width / 3f;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3((lane + 0.5f) * section, Screen.height / 2, 10));
        return worldPos.x;
    }
}
