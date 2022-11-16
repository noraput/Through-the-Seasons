using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThroughTheSeasons
{
    public class Coin : Collectible {

        public static int score = 100;
        public override void Collect()
        {
            GameManager.instance.coin += 1;
            base.Collect();
        }
    }
}

