using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarMotion : MonoBehaviour
{
    [Header("Animation Settings")]
    public float riseHeight = 2f;       // How high it moves up
    public float growTime = 0.5f;       // Time to grow to full size
    public float stayTime = 0.5f;       // Time to stay at full size
    public float shrinkTime = 0.5f;     // Time to shrink back down

    [Header("Teeter Settings")]
    public float teeterAmplitude = 0.2f;  // Side-to-side distance
    public float teeterSpeed = 6f;        // Speed of wobble

    private Vector3 startPos;
    private Vector3 targetPos;
    private Vector3 fullScale = Vector3.one;

    private float timer = 0f;
    private enum State { Growing, Staying, Shrinking }
    private State currentState = State.Growing;

    void Start()
    {
        startPos = transform.position;
        targetPos = startPos + Vector3.up * riseHeight;
        transform.localScale = Vector3.zero; // start invisible
    }

    void Update()
    {
        timer += Time.deltaTime;

        switch (currentState)
        {
            case State.Growing:
                {
                    float t = Mathf.Clamp01(timer / growTime);

                    // Scale from 0 → 1
                    transform.localScale = Vector3.Lerp(Vector3.zero, fullScale, t);

                    // Vertical movement
                    Vector3 basePos = Vector3.Lerp(startPos, targetPos, t);

                    // Add sideways teeter using sine wave
                    float teeter = Mathf.Sin(Time.time * teeterSpeed) * teeterAmplitude;
                    basePos.x += teeter;

                    transform.position = basePos;

                    if (t >= 1f)
                    {
                        currentState = State.Staying;
                        timer = 0f;
                    }
                }
                break;

            case State.Staying:
                {
                    if (timer >= stayTime)
                    {
                        currentState = State.Shrinking;
                        timer = 0f;
                    }
                }
                break;

            case State.Shrinking:
                {
                    float t = Mathf.Clamp01(timer / shrinkTime);

                    // Scale down at the *current position* (no more vertical movement)
                    transform.localScale = Vector3.Lerp(fullScale, Vector3.zero, t);

                    if (t >= 1f)
                    {
                        Destroy(gameObject);
                    }
                }
                break;
        }
    }
}
