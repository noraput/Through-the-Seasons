using System.Collections;
using UnityEngine;

namespace ThroughTheSeasons
{
    public class SeasonalBackground : MonoBehaviour
    {
        [SerializeField]
        private Season season;
        
        [SerializeField]
        private float crossfadeTime;

        private SpriteRenderer[] spriteRenderers;
        private bool isShowing;

        private void OnEnable() {
            GameManager.instance.OnSeasonChange += UpdateBackground;
        }

        private void OnDisable() {
            GameManager.instance.OnSeasonChange -= UpdateBackground;
        }

        private void Start() {
            // GameManager.instance.OnSeasonChange += UpdateBackground;
            spriteRenderers = GetComponentsInChildren<SpriteRenderer>();

            if (IsCurrentSeason()) {
                Show();
            }
            else {
                Hide();
            }

            // Debug.Log(GameManager.instance.CurrentSeason);
        }

        private void UpdateBackground(Season season) {
            if (IsCurrentSeason() && !isShowing) {
                FadeIn();
            }
            else if (!IsCurrentSeason() && isShowing) {
                FadeOut();
            }
        }

        private void FadeIn() {
            isShowing = true;
            foreach (SpriteRenderer renderer in spriteRenderers) {
                StartCoroutine(WaitForFadeIn(renderer));
            }
        }

        private void FadeOut() {
            isShowing = false;
            foreach (SpriteRenderer renderer in spriteRenderers) {
                StartCoroutine(WaitForFadeOut(renderer));
            }
        } 

        private void Hide() {
            isShowing = false;
            foreach (SpriteRenderer renderer in spriteRenderers) {
                renderer.color = Color.clear;
            }
        }

        private void Show() {
            isShowing = true;
            foreach (SpriteRenderer renderer in spriteRenderers) {
                renderer.color = Color.white;
            }
        }

        private IEnumerator WaitForFadeOut(SpriteRenderer renderer) {
            float alpha = renderer.color.a;

            for (float t = 0f; t <= 1f; t += Time.deltaTime / crossfadeTime) {
                Color newColor = new Color(1, 1, 1, Mathf.Lerp(alpha, 0f, t));
                renderer.color = newColor;
                yield return null;
            }

            renderer.color = Color.clear;
        }

        private IEnumerator WaitForFadeIn(SpriteRenderer renderer) {
            float alpha = renderer.color.a;

            for (float t = 0f; t <= 1f; t += Time.deltaTime / crossfadeTime) {
                Color newColor = new Color(1, 1, 1, Mathf.Lerp(alpha, 1f, t));
                renderer.color = newColor;
                yield return null;
            }

            renderer.color = Color.white;
        }

        private bool IsCurrentSeason() {
            return season == GameManager.instance.CurrentSeason;
        }
    }
}
