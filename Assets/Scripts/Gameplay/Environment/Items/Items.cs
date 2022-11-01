using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThroughTheSeasons
{
    public abstract class Item {
        public Season season;
        public virtual void Affect() {
            // Debug.Log(GetType().Name + " is Collected");
        }
    }

    public abstract class InstantItem : Item {}
    public abstract class TemporaryItem : Item {
        public float duration;
        public abstract  void Expire();
    }
}
