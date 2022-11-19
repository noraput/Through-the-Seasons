using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThroughTheSeasons
{
    public class SpeedShoes : TemporaryItem {
        float speedMultiplier;

        public SpeedShoes(float speedMultiplier) {
            itemType = ItemType.SpeedShoes;
            this.speedMultiplier = speedMultiplier;
        }

        public override void Affect() {
            base.Affect();
            PlayerCore.instance.CharBase.Movement.speedMultiplier = speedMultiplier;
        }

        public override void Expire() {
            PlayerCore.instance.CharBase.Movement.speedMultiplier = 1f;
            base.Expire(); 
        }
    }
}
