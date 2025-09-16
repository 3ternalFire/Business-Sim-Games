using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EyeState { Open, Closed, Hurt }

public class EyeController : MonoBehaviour
{
    public EyeState currentState = EyeState.Open;
    public Collider2D eyeCollider;
    public int pokesNeededToClose = 3;

    private int pokeCount = 0;
    public Animator eyeAnimator;

    private CharacterMover mover;

    public void SetCharacterMover(CharacterMover cm)
    {
        mover = cm;
    }

    public void Poke()
    {
        // Don’t allow poking if moving or already closed/hurt
        if (currentState != EyeState.Open || (mover != null && mover.IsMoving()))
            return;

        pokeCount++;
        currentState = EyeState.Hurt;
        eyeAnimator.SetBool("EyeClosed", true);
        mover?.OnEyePoked();

        if (pokeCount >= pokesNeededToClose)
        {
            CloseEye();

        }
        else
        {
            // reset back to open after short delay
            Invoke(nameof(OpenEye), 0.3f);
        }
    }

    public void OpenEye()
    {
        if (mover != null && mover.IsMoving())
            return; // don't open if face is moving

        eyeAnimator.SetBool("EyeClosed", false);
        currentState = EyeState.Open;
    }

    public void CloseEye()
    {
        currentState = EyeState.Closed;
        eyeAnimator.SetBool("EyeClosed", true);
    }

    public void ResetEyeCounter()
    {
        pokeCount = 0;
    }

    public bool IsOpen() => currentState == EyeState.Open;

}
