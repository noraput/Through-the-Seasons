using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThroughTheSeasons
{
    public class Coin : Collectible {

        public static int score = 100;
        public static Action<int> OnCollect;

        public override void Collect()
        {
            OnCollect?.Invoke(1);
            base.Collect();
        }
    }
}

