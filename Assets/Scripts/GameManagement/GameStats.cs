using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ThroughTheSeasons {
    public class GameStats : PersistentObject<GameStats>
    {
        Dictionary<string, float> achievementProgress;

        protected override void Awake() {
            base.Awake();

            AchievementManager.instance.ResetAchievementState();
            
            achievementProgress = AchievementManager.instance.AchievementList
                .Select(achievementInfo => achievementInfo.Key)
                .ToDictionary(key => key, value => 0f);

            Debug.Log(achievementProgress.Count);
        }

        private void OnEnable() {
            PlayerCore.instance.OnDeath += SaveAchievementProgress;
            CollectibleItem.OnCollect += SaveItemAchievementProgress;
        }

        private void OnDisable() {
            PlayerCore.instance.OnDeath -= SaveAchievementProgress;
            CollectibleItem.OnCollect -= SaveItemAchievementProgress;
        }

        private void SaveAchievementProgress() {
            foreach (KeyValuePair<string, float> achievementKvp in achievementProgress) {
                AchievementManager.instance.AddAchievementProgress(achievementKvp.Key, achievementKvp.Value);
            }
        }

        private void SaveItemAchievementProgress(ItemType itemType) {
            string achievementKey = achievementProgress.Keys.First(key => key.Contains(itemType.ToString()));
            achievementProgress[achievementKey] += 1;
        }
    }
}
