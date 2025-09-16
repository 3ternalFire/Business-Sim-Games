using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PokeManager : MonoBehaviour
{
    public static PokeManager Instance;
    public EyeController leftEye;
    public EyeController rightEye;
    public HandAnimator handAnimator;
    public TextMeshProUGUI scoreText;

    private int score = 0;

    [SerializeField]
    private RoundTimer roundTimer;

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    void OnEnable()
    {
        InputManager.OnScreenTapped += TryPoke;
    }

    void OnDisable()
    {
        InputManager.OnScreenTapped -= TryPoke;
    }

    // Updated to take TapArea
    public void TryPoke(TapArea tappedArea)
    {
        EyeController targetEye = null;

        // Map TapArea to the lanes your character uses
        // Assuming lane 0 = left, 1 = middle, 2 = right
        int laneIndex = TapAreaToLane(tappedArea);

        // Check which eye is open AND in the tapped lane
        if (leftEye.IsOpen() && IsInLane(leftEye, laneIndex))
            targetEye = leftEye;
        else if (rightEye.IsOpen() && IsInLane(rightEye, laneIndex))
            targetEye = rightEye;

        if (targetEye != null)
        {
            targetEye.Poke();
            handAnimator.PokeAtLane(laneIndex);
            score++;
            scoreText.SetText($"SCORE: {score}");
        }
        else
        {
            handAnimator.PokeAtLane(laneIndex);
        }
    }

    int TapAreaToLane(TapArea area)
    {
        switch (area)
        {
            case TapArea.Left:
                return 0;
            case TapArea.Middle: 
                return 1;
            case TapArea.Right: 

                return 2;
            default: return 1;
        }
    }

    bool IsInLane(EyeController eye, int lane)
    {
        if (eye.eyeCollider == null) return false;

        // Get collider bounds in screen space
        Bounds bounds = eye.eyeCollider.bounds;
        Vector3 minScreen = Camera.main.WorldToScreenPoint(bounds.min);
        Vector3 maxScreen = Camera.main.WorldToScreenPoint(bounds.max);

        // Lane screen boundaries
        float laneWidth = Screen.width / 3f;
        float laneMin = lane * laneWidth;
        float laneMax = (lane + 1) * laneWidth;

        // Check if any part of the eye collider overlaps with lane
        if (maxScreen.x < laneMin || minScreen.x > laneMax)
            return false; // No overlap
        return true; // Overlaps
    }

    public int GetScore()
    {
        return score;
    }
}
