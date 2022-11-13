using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThroughTheSeasons
{
    public class BigCoin : InstantItem {
        public float score;
        public int coinGained;

        public BigCoin(float score, int coinGained) {
            this.score = score;
            this.coinGained = coinGained;
        }

        public override void Affect() {
            base.Affect();
        }
    }
}
