using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace ThroughTheSeasons
{
    public class DebugCanvas : PersistentObject<DebugCanvas>
    {
        public TextMeshProUGUI debugText;

        private void Update()
        {
            debugText.text = GameManager.instance.GetDebugInfo();
        }
    }
}
