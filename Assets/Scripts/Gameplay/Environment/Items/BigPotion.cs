using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThroughTheSeasons
{
    public class BigPotion : TemporaryItem {
        public float sizeMultiplier;

        private Vector3 normalSize;
        private Vector3 bigSize;

        private float sizeLerpDuration;
        private float sizeLerpTime;
        private float t;

        public BigPotion(float sizeMultiplier) {
            itemType = ItemType.BigPotion;
            this.sizeMultiplier = sizeMultiplier;
            
            sizeLerpDuration = 0.4f;
        }

        public override void Affect() {
            base.Affect();
            normalSize = GameManager.instance.PlayerTransform.localScale;
            bigSize = GameManager.instance.PlayerTransform.localScale * sizeMultiplier;

            GameManager.instance.PlayerTransform.Find("GroundCheck").localPosition += Vector3.up * 0.1f;
            PlayerCore.instance.CharBase.Jumping.groundCheckRadius *= 3f;
        }

        public override void Tick() {
            sizeLerpTime += Time.deltaTime;
            t = Mathf.Clamp01(sizeLerpTime / sizeLerpDuration);

            GameManager.instance.PlayerTransform.localScale = normalSize * Mathf.Lerp(1, sizeMultiplier, t); 
        }

        public override void Expire() {
            PlayerCore.instance.CharBase.Jumping.groundCheckRadius /= 3f;
            GameManager.instance.PlayerTransform.Find("GroundCheck").localPosition += Vector3.down * 0.1f;
            GameManager.instance.PlayerTransform.localScale = normalSize;
            base.Expire(); 
        }
    }
}
