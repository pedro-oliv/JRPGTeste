using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DamageEffect
{
    MonoBehaviour coroutineRunner;

    public DamageEffect(MonoBehaviour runner)
    {
        this.coroutineRunner = runner;
    }

    public void PlayEffect(Image targetImage, Color color, float duration = 0.5f)
    {
        coroutineRunner.StartCoroutine(EffectCoroutine(targetImage, color, duration));
    }

    private IEnumerator EffectCoroutine(Image target, Color color, float duration)
    {
        Color originalColor = target.color;

        target.color = color;

        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            target.color = Color.Lerp(color, originalColor, elapsed / duration);
            yield return null;
        }

        target.color = originalColor;
    }
}
