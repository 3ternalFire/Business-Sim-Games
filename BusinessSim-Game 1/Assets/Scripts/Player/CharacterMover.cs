using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMover : MonoBehaviour
{
    public Transform[] lanePositions; // 0 = left, 1 = middle, 2 = right
    public float moveSpeed = 5f;
    private int currentLane = 1;

    private EyeController[] eyes;

    void Start()
    {
        eyes = GetComponentsInChildren<EyeController>();
        StartCoroutine(MoveRoutine());
    }

    IEnumerator MoveRoutine()
    {
        while (true)
        {
            // Wait until both eyes are closed
            yield return new WaitUntil(() => BothEyesClosed());

            // Pick a new lane (0 = left+mid, 1 = mid+right)
            int newLane = Random.Range(0, 2);
            currentLane = newLane;

            // Move character to correct lane position
            Vector3 targetPos = lanePositions[newLane].position;
            while (Vector3.Distance(transform.position, targetPos) > 0.05f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
                yield return null;
            }

            // Small pause after moving
            yield return new WaitForSeconds(0.5f);

            // Reopen both eyes
            foreach (var eye in eyes)
            {
                eye.OpenEye();
                eye.ResetEyeCounter();
            }

            // Wait a random duration before next possible close sequence
            yield return new WaitForSeconds(Random.Range(1.5f, 3f));
        }
    }

    bool BothEyesClosed()
    {
        foreach (var eye in eyes)
        {
            if (eye.currentState != EyeState.Closed)
                return false;
        }
        return true;
    }
}

