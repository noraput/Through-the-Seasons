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
        }

        public override void Expire() {
            base.Expire(); 
        }
    }
}
