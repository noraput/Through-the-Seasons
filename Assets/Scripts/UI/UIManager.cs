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

        
        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            scoreText.text = "Score : " + GameManager.instance.score;
            shadowScoreText.text = "Score : " + GameManager.instance.score;
            
            if (GameManager.instance.life > 0)
            {
                lifeImage.sprite = lifeSprites[GameManager.instance.life -1];
            }
            else
            {
                lifeImage.enabled = false;
            }
        }
    }

}