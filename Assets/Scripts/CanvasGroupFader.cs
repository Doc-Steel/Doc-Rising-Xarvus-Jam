using System.Collections;
using UnityEngine;

namespace VaudinGames.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class CanvasGroupFader : MonoBehaviour
    {
        private CanvasGroup canvasGroup;
        private Coroutine currentFadeRoutine = null;

        private void Awake() => canvasGroup = GetComponent<CanvasGroup>();

        public Coroutine FadeIn(float time) => Fade(1, time);

        public Coroutine FadeOut(float time) => Fade(0, time);

        public void FadeInThenOut(float time, float waitTime) => StartCoroutine(FadeInThenOutRoutine(time, waitTime));

        private IEnumerator FadeInThenOutRoutine(float fadeTime, float waitTime)
        {
            FadeIn(fadeTime / 2);
            yield return new WaitForSeconds((fadeTime / 2) + waitTime);
            FadeOut(fadeTime / 2);
        }

        public Coroutine Fade(float targetAlpha, float time)
        {
            // Stops any active fade before starting new fade routine
            if (currentFadeRoutine != null) { StopCoroutine(currentFadeRoutine); }
            currentFadeRoutine = StartCoroutine(FadeRoutine(targetAlpha, time));
            return currentFadeRoutine;
        }

        private IEnumerator FadeRoutine(float targetAlpha, float time)
        {
            while (!Mathf.Approximately(canvasGroup.alpha, targetAlpha))
            {
                canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, targetAlpha, Time.unscaledDeltaTime / time);
                yield return null;
            }
        }
    }
}

