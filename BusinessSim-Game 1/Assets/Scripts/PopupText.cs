using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PopupText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text; // assign in inspector
    [SerializeField] private float duration = 1.5f;
    [SerializeField] private float maxScale = 1.5f;
    [SerializeField] private AudioClip clip;
    [SerializeField] private AudioSource source;

    private void OnEnable()
    {
        GameManager.OnHighScoreAchieved += ShowText;
    }
    private void OnDisable()
    {
        GameManager.OnHighScoreAchieved -= ShowText;
    }
    public void ShowText(string message, Color color)
    {
        text.text = message;
        text.color = color;
        StartCoroutine(AnimateText());
    }

    private IEnumerator AnimateText()
    {
        float time = 0f;
        Vector3 startScale = Vector3.zero;
        Vector3 endScale = Vector3.one * maxScale;
        Debug.Log("HighScore Text");

        text.transform.localScale = startScale;
        text.enabled = true;
        source.clip = clip;
        source.Play();

        // Grow + fade in
        while (time < duration * 0.5f)
        {
            time += Time.deltaTime;
            float t = time / (duration * 0.5f);

            text.transform.localScale = Vector3.Lerp(startScale, endScale, t);
            text.color = new Color(text.color.r, text.color.g, text.color.b, t);
            yield return null;
        }

        // Shrink + fade out
        time = 0f;
        while (time < duration * 0.5f)
        {
            time += Time.deltaTime;
            float t = time / (duration * 0.5f);

            text.transform.localScale = Vector3.Lerp(endScale, startScale, t);
            text.color = new Color(text.color.r, text.color.g, text.color.b, 1f - t);
            yield return null;
        }

        text.enabled = false;
    }
}
