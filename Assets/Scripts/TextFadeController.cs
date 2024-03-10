using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextFadeController : MonoBehaviour
{
    public TextMeshProUGUI displayText;
    public float fadeDuration = 2f;
    public float displayDuration = 10f;

    void Start()
    {
        Invoke("FadeText", displayDuration);
    }

    void FadeText()
    {
        StartCoroutine(FadeOut(displayText, fadeDuration));
    }

    IEnumerator FadeOut(TextMeshProUGUI textElement, float duration)
    {
        Color originalColor = textElement.color;
        float startTime = Time.time;

        while (Time.time - startTime < duration)
        {
            float t = (Time.time - startTime) / duration;
            textElement.color = Color.Lerp(originalColor, Color.clear, t);
            yield return null;
        }

        // Ensure the text is completely transparent
        textElement.color = Color.clear;
    }
}