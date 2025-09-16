using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum TapArea { Left, Middle, Right }

public class InputManager : MonoBehaviour
{
    public static UnityAction<TapArea> OnScreenTapped;

    public RoundTimer timer;

    void Update()
    {
        if (!timer.CanScore()) return;
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                CheckTapArea(touch.position);
            }
        }
    }

    void CheckTapArea(Vector2 tapPos)
    {
        float screenWidth = Screen.width;

        float leftBoundary = screenWidth / 3f;
        float rightBoundary = 2f * screenWidth / 3f;

        if (tapPos.x <= leftBoundary) // Left third
        {
            OnScreenTapped?.Invoke(TapArea.Left);
        }
        else if (tapPos.x >= rightBoundary) // Right third
        {
            OnScreenTapped?.Invoke(TapArea.Right);
        }
        else // Middle third
        {
            OnScreenTapped?.Invoke(TapArea.Middle);
        }
    }
}
