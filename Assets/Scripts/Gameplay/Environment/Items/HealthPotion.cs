using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThroughTheSeasons
{
    public class HealthPotion : InstantItem {
        public int healthGain;

        public HealthPotion(int healthGain) {
            this.healthGain = healthGain;
        }

        public override void Affect() {
            base.Affect();
        }
    }
}
