using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;

public class TimeListener : NumberListener
{
    public override void UpdateText()
    {
        text.text = "" + GameManager.Numbers.TotalTime;
    }
}
