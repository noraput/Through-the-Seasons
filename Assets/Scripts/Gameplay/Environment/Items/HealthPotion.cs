using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThroughTheSeasons
{
    public class HealthPotion : InstantItem {
        public int healthGain;

        public HealthPotion(int healthGain) {
            itemType = ItemType.HealthPotion;
            this.healthGain = healthGain;
        }

        public override void Affect() {
            GameManager.instance.UpdateLife(healthGain);
            base.Affect();
        }
    }
}
