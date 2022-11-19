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

        private void OnEnable() {
            GameManager.instance.OnLifeChanged += UpdateLifeUi;
        }

        private void OnDisable() {
            GameManager.instance.OnLifeChanged -= UpdateLifeUi;
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
    }

}