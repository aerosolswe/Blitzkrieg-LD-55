using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TotalKillsListener : NumberListener
{
    public override void UpdateText()
    {
        text.text = "" + GameManager.Numbers.TotalKills;
    }
}
