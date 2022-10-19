using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DebugCanvas : PersistentObject<DebugCanvas>
{
    public TextMeshProUGUI debugText;

    private void Update()
    {
        debugText.text = GameManager.instance.GetDebugInfo();
    }
}
