using UnityEngine;
using System.Collections;

public class WaterAttack : MonoBehaviour
{
    public float maxHeight = 40f;
    public float minHeight = 1f;
    public float speed = 2f;

    private bool isGrowing = true;

    void Start()
    {
        StartCoroutine(GrowAndShrink());
    }

    IEnumerator GrowAndShrink()
    {
        while (true)
        {
            float startHeight = transform.localScale.y;
            float endHeight = isGrowing ? maxHeight : minHeight;
            float duration = Mathf.Abs(endHeight - startHeight) / speed;
            float countdown = 0;

            while (countdown < duration)
            {
                countdown += Time.deltaTime;
                float newYScale = Mathf.Lerp(startHeight, endHeight, countdown / duration);
                transform.localScale = new Vector3(transform.localScale.x, newYScale, transform.localScale.z);
                yield return null;
            }

            isGrowing = !isGrowing;
        }
    }
}