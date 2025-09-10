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

    public void Poke()
    {
        if (currentState != EyeState.Open) return;

        pokeCount++;
        currentState = EyeState.Hurt;
        Debug.Log(name + " eye poked!");
        eyeAnimator.SetBool("EyeClosed", true);

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
        eyeAnimator.SetBool("EyeClosed", false);
        currentState = EyeState.Open;
    }

    private void CloseEye()
    {
        currentState = EyeState.Closed;
        eyeAnimator.SetBool("EyeClosed", true);
        Debug.Log(name + " eye closed!");
    }
    public void ResetEyeCounter()
    {
        pokeCount = 0;
    }

    public bool IsOpen() => currentState == EyeState.Open;

}
