using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThroughTheSeasons
{
    public class BigCoin : InstantItem {
        public float score;
        public int coinGained;

        public BigCoin(float score, int coinGained) {
            itemType = ItemType.BigCoin;
            
            this.score = score;
            this.coinGained = coinGained;
        }

        public override void Affect() {
            GameManager.instance.UpdateCoin(10);
            base.Affect();
        }
    }
}
