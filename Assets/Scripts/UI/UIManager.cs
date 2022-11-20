using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace ThroughTheSeasons
{
    public class UIManager : MonoBehaviour
    {
        public TextMeshProUGUI scoreText;
        public TextMeshProUGUI shadowScoreText;
        public TextMeshProUGUI lifeText;
        public TextMeshProUGUI shadowLifeText;
        public Sprite[] lifeSprites;
        public Image lifeImage;

        public string[] resultScreenWords;
        public GameObject resultScreen;
        public GameObject resultTextGameObject;
        public TextMeshProUGUI resultScreenWordsText;
        public TextMeshProUGUI resultText;
        public float fadeinWaitTime;
        private bool isReadyToStartNewGame;

        private void OnEnable() {
            GameManager.instance.OnLifeChanged += UpdateLifeUi;
            PlayerCore.instance.OnDeath += ShowResultScreen;
        }

        private void OnDisable() {
            GameManager.instance.OnLifeChanged -= UpdateLifeUi;
            PlayerCore.instance.OnDeath -= ShowResultScreen;
        }
 
        void Update() {
            scoreText.text = "Score : " + GameManager.instance.score;
            shadowScoreText.text = "Score : " + GameManager.instance.score;
        }

        private void UpdateLifeUi(int life) {
            if (life > 0) {
                lifeImage.enabled = true;
                lifeImage.sprite = lifeSprites[life -1];
            }
            else {
                lifeImage.enabled = false;
            }
        }

        private void ShowResultScreen() {
            resultScreen.SetActive(true);
            resultScreenWordsText.text = resultScreenWords.PickRandom();
            StartCoroutine(WaitForFadeIn());
        }

        private IEnumerator WaitForFadeIn() {
            yield return new WaitForSecondsRealtime(fadeinWaitTime);
            resultTextGameObject.SetActive(true);
            resultText.text = $"Coins Collected: "
                + $"\nSeasons Passed: "
                + $"\nYears Passed: "
                + $"\n\nFinal Score:";
        }
    }

}