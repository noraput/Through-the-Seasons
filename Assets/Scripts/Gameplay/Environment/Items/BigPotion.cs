using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThroughTheSeasons
{
    public class BigPotion : TemporaryItem {
        public float sizeMultiplier;

        public BigPotion(float sizeMultiplier) {
            this.sizeMultiplier = sizeMultiplier;
        }

        public override void Affect() {
            base.Affect();
            GameManager.instance.PlayerTransform.localScale *= sizeMultiplier;
            GameManager.instance.PlayerTransform.Find("GroundCheck").position += Vector3.up * 0.1f;
            PlayerCore.instance.CharBase.Jumping.groundCheckRadius *= 3f;
        }

        public override void Expire() {
            PlayerCore.instance.CharBase.Jumping.groundCheckRadius /= 3f;
            GameManager.instance.PlayerTransform.Find("GroundCheck").position += Vector3.down * 0.1f;
            GameManager.instance.PlayerTransform.localScale /= sizeMultiplier;
            base.Expire(); 
        }
    }
}
