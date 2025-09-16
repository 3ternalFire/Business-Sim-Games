using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMover : MonoBehaviour
{
    public Transform[] lanePositions; // 0 = left, 1 = middle, 2 = right
    public float moveSpeed = 10f;
    private int currentLane = 1;

    private bool isMoving = false;

    private EyeController[] eyes;

    public RoundTimer timer;

    public List<AudioClip> pokedSounds;

    private AudioSource audioSource;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        eyes = GetComponentsInChildren<EyeController>();
        foreach (EyeController e in eyes)
        {
            e.SetCharacterMover(this);
        }
        StartCoroutine(MoveRoutine());
        transform.position = lanePositions[Random.Range(0,lanePositions.Length)].position;
    }

    IEnumerator MoveRoutine()
    {
        while (true)
        {
            if (!timer.CanScore()) yield return null;
            // Wait until both eyes are closed
            yield return new WaitUntil(() => BothEyesClosed());

            // Go into moving state
            isMoving = true;
            ForceCloseEyes(); // make sure eyes are closed

            // Pick a new lane different from current
            int newLane = currentLane;
            while (newLane == currentLane)
            {
                newLane = Random.Range(0, lanePositions.Length);
            }
            currentLane = newLane;

            // Move character to target lane
            Vector3 targetPos = lanePositions[newLane].position;
            while (Vector3.Distance(transform.position, targetPos) > 0.05f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
                yield return null;
            }

            // Small pause after moving
            yield return new WaitForSeconds(0.5f);

            // Movement finished
            isMoving = false;

            // Reopen both eyes
            foreach (var eye in eyes)
            {
                eye.OpenEye();
                eye.ResetEyeCounter();
            }

            // Wait a random duration before next possible close sequence
            yield return new WaitForSeconds(Random.Range(0.5f, 1.5f));
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
    public bool IsMoving()
    {
        return isMoving;
    }

    public void ForceCloseEyes()
    {
        foreach (var eye in eyes)
        {
            eye.CloseEye();
        }
    }

    // Called by EyeController when poked
    public void OnEyePoked()
    {
        if (isMoving) return; // ignore pokes while moving

        AudioClip currentScreech = pokedSounds[Random.Range(0, pokedSounds.Count)];
        audioSource.clip = currentScreech;
        audioSource.Play();

        // 30% chance to trigger movement
        if (Random.value < 0.3f)
        {
            // Close both eyes and movement will happen in MoveRoutine
            ForceCloseEyes();
        }
    }
}

