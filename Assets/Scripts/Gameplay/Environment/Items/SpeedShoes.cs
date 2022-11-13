using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThroughTheSeasons
{
    public class SpeedShoes : TemporaryItem {
        float speedMultiplier;

        public SpeedShoes(float speedMultiplier) {
            this.speedMultiplier = speedMultiplier;
        }

        public override void Affect() {
            base.Affect();
        }

        public override void Expire() {
            base.Expire(); 
        }
    }
}
