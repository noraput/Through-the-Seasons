using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ThroughTheSeasons
{
    public class SeasonalBackground : MonoBehaviour
    {
        [SerializeField]
        private Season season;
        
        [SerializeField]
        private float crossfadeTime;

        [SerializeField]
        private bool isShowing;

        private List<SpriteRenderer> spriteRenderers;

        private void OnEnable() {
            GameManager.instance.OnSeasonChange += UpdateBackground;
        }

        private void OnDisable() {
            GameManager.instance.OnSeasonChange -= UpdateBackground;
        }

        private void Start() {
            // GameManager.instance.OnSeasonChange += UpdateBackground;
            spriteRenderers = GetComponentsInChildren<BackgroundParallax>()
                .Where(background => background.needsFading)
                .SelectMany(background => background.GetComponentsInChildren<SpriteRenderer>())
                .ToList();

            // string debug = "BG: ";
            // spriteRenderers.ForEach(sr => debug += " " + sr.gameObject.name + ",");
            // Debug.Log(debug);

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
                // Hide();
            }
        } 

        private void Hide() {
            isShowing = false;
            foreach (SpriteRenderer renderer in spriteRenderers) {
                renderer.sortingLayerName = "HideBG";
            }
        }

        private void Show() {
            isShowing = true;
            foreach (SpriteRenderer renderer in spriteRenderers) {
                renderer.sortingLayerName = "ShowBG";
                renderer.color = Color.white;
            }
        }

        private IEnumerator WaitForFadeOut(SpriteRenderer renderer) {
            renderer.sortingLayerName = "HideBG";

            // renderer.sortingOrder = SortingLayer.NameToID("HideBG");
            // renderer.sortingOrder = 0;

            // for (float t = 0f; t <= 1f; t += Time.deltaTime / crossfadeTime) {
            //     Color newColor = new Color(1, 1, 1, Mathf.Lerp(alpha, 0f, t));
            //     renderer.color = newColor;
            //     yield return null;
            // }

            yield return new WaitForSecondsRealtime(crossfadeTime);
            yield return null;

            renderer.color = Color.clear;
        }

        private IEnumerator WaitForFadeIn(SpriteRenderer renderer) {
            renderer.color = Color.clear;
            renderer.sortingLayerName = "ShowBG";

            // renderer.sortingOrder = 10;

            for (float t = 0f; t <= 1f; t += Time.deltaTime / crossfadeTime) {
                Color newColor = new Color(1, 1, 1, Mathf.Lerp(0f, 1f, t));
                renderer.color = newColor;
                yield return null;
            }

            // renderer.color = Color.white;
        }

        private bool IsCurrentSeason() {
            return season == GameManager.instance.CurrentSeason;
        }
    }
}
