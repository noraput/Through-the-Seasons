using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThroughTheSeasons
{
    public abstract class Item {
        public Season season;
        public ItemType itemType;

        public abstract void Apply();

        public virtual void Affect() {
            Debug.Log(GetType().Name + " is Collected");
        }
    }

    public abstract class InstantItem : Item {
        public override void Apply() {
            Affect();
        }
    }

    public class TemporaryItem : Item {
        public float lastTimeUsed;
        public float duration;
        public bool isExpired;

        public TemporaryItem(float duration = 7.5f) {
            this.duration = duration;
        }

        public override void Apply() {
            PlayerCore.instance.CheckItem(this);
        }

        public override void Affect() {
            if (isExpired)
                return;

            lastTimeUsed = Time.time;
            base.Affect();
        }

        public virtual void TryTick() {
            if (isExpired)
                return;
            
            // Debug.Log(GetType().Name + " is ticking: " + (lastTimeUsed + duration) + " | " + Time.time);

            if (lastTimeUsed + duration < Time.time) {
                Expire();
                return;
            }

            Tick();
        }

        public virtual void Tick() {}

        public virtual void Reset() {
            lastTimeUsed = Time.time;
            Debug.Log(GetType().Name + " duration is reset, since player is already using it.");
        }

        public virtual void Expire() {
            isExpired = true;
            Debug.Log(GetType().Name + " is expired");
        }
    }
}
